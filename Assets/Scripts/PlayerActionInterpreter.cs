using System.Linq;
using UnityEngine;

public class PlayerActionInterpreter : MonoBehaviour
{
    [SerializeField]
    GameManager manager;

    [SerializeField]
    GameStateSO moveState, placeState, removeState;

    [SerializeField]
    SiteNavigation navigation;

    [SerializeField]
    GameEvent validMoveSound;

    [SerializeField]
    GameEvent invalidMoveSound;

    int plotSelectionIndex = int.MinValue;

    int mineSelectionIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    bool IsPlotOnlyState()
    {
        return manager.CurrentGameState == moveState;
    }

    // Update is called once per frame
    void Update()
    {
        navigation.PreventMineSelection = IsPlotOnlyState();
    }

    public void CancelPlotSelection()
    {
        plotSelectionIndex = int.MinValue;
    }

    void MoveMine(int startLocation, int endLocation)
    {
        var mine = manager.PlayerPumps.Where(x => x.LocationIndex == startLocation).First();
        mine.LocationIndex = endLocation;
    }

    void HandleMoveStateOptions(int plotLocationIndex)
    {
        if (plotSelectionIndex == int.MinValue)
        {
            //Valid move if there is a player pump at the location and the location has not been covered
            var playerPumpsAtLocation = manager.PlayerPumps.Where(x => x.LocationIndex == plotLocationIndex).Any();
            bool plotCheckPassed = plotLocationIndex == -1 || manager.MiningPlots.Where(x
                => x.LocationIndex == plotLocationIndex && !x.AreAllFull()).Any();
            if (playerPumpsAtLocation && plotCheckPassed)
            {
                plotSelectionIndex = plotLocationIndex;
                validMoveSound.Invoke();
            }
            else
            {
                invalidMoveSound.Invoke();
            }
        }
        else
        {
            //Valid move if this is not the same as the first location, and the destination is empty
            if (plotSelectionIndex == plotLocationIndex)
            {
                invalidMoveSound.Invoke();
            }
            else if (plotLocationIndex == -1)
            {
                MoveMine(plotSelectionIndex, plotLocationIndex); //Are mines allowed to be moved back?
                validMoveSound.Invoke();
                manager.HasPlayerMadeSelection = true;
            }
            else
            {
                bool plotCheckPassed = manager.MiningPlots.Where(x
                    => x.LocationIndex == plotLocationIndex && !x.AreAllFull()
                    && !manager.IsPumpAtLocation(plotLocationIndex)).Any();
                if (plotCheckPassed)
                {
                    MoveMine(plotSelectionIndex, plotLocationIndex);
                    validMoveSound.Invoke();
                    manager.HasPlayerMadeSelection = true;
                }
                else
                {
                    invalidMoveSound.Invoke();
                }
            }
        }
    }


    public void HandlePlotSelection(int plotLocationIndex)
    {
        if (manager.CurrentGameState == moveState)
        {
            HandleMoveStateOptions(plotLocationIndex);
        }
        else if (manager.CurrentGameState == removeState)
        {
            if (plotLocationIndex == -1)
            {
                invalidMoveSound.Invoke();
            }
            else
            {
                plotSelectionIndex = plotLocationIndex;
            }
        }
        else
        {
            plotSelectionIndex = plotLocationIndex;
        }
    }

    public void HandleMineSelection(int mineLocationIndex)
    {
        if (manager.CurrentGameState == placeState)
        {
            if (plotSelectionIndex == -1)
            {
                invalidMoveSound.Invoke();
            }
            else
            {
                var plot = manager.MiningPlots.Where(x => x.LocationIndex == plotSelectionIndex).First();
                var mine = plot.GetMinLocationAtLocationIndex(mineLocationIndex);
                if (mine.HasSlime)
                {
                    invalidMoveSound.Invoke();
                }
                else
                {
                    mine.HasSlime = true;
                    validMoveSound.Invoke();
                    manager.HasPlayerMadeSelection = true;
                }
            }

        }
        else if (manager.CurrentGameState == removeState)
        {
            if (plotSelectionIndex == -1)
            {
                invalidMoveSound.Invoke();
            }
            else
            {
                var plot = manager.MiningPlots.Where(x => x.LocationIndex == plotSelectionIndex).First();
                var mine = plot.GetMinLocationAtLocationIndex(mineLocationIndex);
                if (mine.HasSlime)
                {
                    mine.HasSlime = false;
                    manager.HasPlayerMadeSelection = true;
                    validMoveSound.Invoke();
                    manager.HasPlayerMadeSelection = true;
                }
                else
                {
                    invalidMoveSound.Invoke();
                }
            }
        }
    }
}
