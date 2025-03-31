using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SiteNavigation : MonoBehaviour
{
    [SerializeField]
    SelectorController selectorController;

    [SerializeField]
    List<CoordinateHaver> plots;

    [SerializeField]
    bool preventMineSelection;

    [SerializeField]
    GameEventGeneric<int> onPlotSelected;

    [SerializeField]
    GameEventGeneric<int> onMineSelected;

    [SerializeField]
    GameEvent onCancelPlotSelection;

    [SerializeField]
    GameEvent onPlayMoveSound;

    Vector2Int plotLocation = Vector2Int.zero;


    Vector2Int mineLocation = Vector2Int.zero;

    bool currentlySelectingPlot = true;

    public bool PreventMineSelection { get => preventMineSelection; set => preventMineSelection = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePlotSelection();
    }

    public void Move(Vector2Int movementDirection)
    {
        if (currentlySelectingPlot)
        {
            var init = plotLocation;
            plotLocation += movementDirection;
            //Would probably be better to calculate this...
            plotLocation = Vector2Int.Max(Vector2Int.Min(new Vector2Int(1, 1), plotLocation), new Vector2Int(-1, -1));
            if (init != plotLocation)
            {
                onPlayMoveSound.Invoke();
            }
            UpdatePlotSelection();
        }
        else
        {
            var init = mineLocation;
            mineLocation += movementDirection;
            mineLocation = Vector2Int.Max(Vector2Int.Min(new Vector2Int(1, 1), mineLocation), new Vector2Int(-1, -1));
            if (init != mineLocation)
            {
                onPlayMoveSound.Invoke();
            }
            UpdateMineSelection();
        }
    }

    void UpdatePlotSelection()
    {
        var selectedPlot = plots.Where(x => x.Coordinates == plotLocation).First();
        selectorController.SetPosition(selectedPlot.transform.position);
        if (plotLocation == Vector2Int.zero)
        {
            var calc = selectedPlot.GetComponent<BoundCalculator>();
            selectorController.SetScale(calc.GetBounds().size);
        }
        else
        {
            var plot = selectedPlot.GetComponent<MiningPlot>();
            selectorController.SetScale(plot.GetBounds().size);
        }

    }

    void UpdateMineSelection()
    {
        if (plotLocation == Vector2Int.zero)
        {
            //No plots on mining station
            return;
        }
        var selectedPlot = plots.Where(x => x.Coordinates == plotLocation).First();
        var plot = selectedPlot.GetComponent<MiningPlot>();
        var selectedMine = plot.GetBoundsOfCoordinate(mineLocation);
        selectorController.SetPosition(selectedMine.center);
        selectorController.SetScale(selectedMine.size);
    }

    public void SetToPlotSelection()
    {
        currentlySelectingPlot = true;
        mineLocation = Vector2Int.zero;
        UpdatePlotSelection();
    }


    public void SelectPlot()
    {
        if (currentlySelectingPlot)
        {
            //This will need to change for both base selection, and also swapping of drills 
            if (preventMineSelection)
            {
                //Fire Event With Selected Plot
                if (plotLocation == Vector2Int.zero)
                {
                    onPlotSelected.Invoke(-1);
                }
                else
                {
                    var plot = plots.Where(x => x.Coordinates == plotLocation).First().GetComponent<MiningPlot>();
                    onPlotSelected.Invoke(plot.LocationIndex);
                }
            }
            else
            {
                if (plotLocation == Vector2Int.zero)
                {
                    onPlotSelected.Invoke(-1);
                }
                else
                {
                    var plot = plots.Where(x => x.Coordinates == plotLocation).First().GetComponent<MiningPlot>();
                    onPlotSelected.Invoke(plot.LocationIndex);
                    currentlySelectingPlot = false;
                    mineLocation = Vector2Int.zero;
                    UpdateMineSelection();
                }
            }
        }
        else
        {
            //Fire Event with selected plot
            var plot = plots.Where(x => x.Coordinates == plotLocation).First().GetComponent<MiningPlot>();
            var mine = plot.GetMineLocationAtCoordinate(mineLocation);

            onMineSelected.Invoke(mine != default(MineLocation) ? mine.LocationIndex : -1);
        }
    }

    public void CancelSelection()
    {
        if (!currentlySelectingPlot)
        {
            SetToPlotSelection();
        }
        else
        {
            onCancelPlotSelection.Invoke();
        }
    }
}
