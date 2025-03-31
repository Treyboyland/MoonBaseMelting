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
    GameEventGeneric<string> invalidMoveSound;

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
                string invalidString;
                if (!playerPumpsAtLocation)
                {
                    invalidString = "You don't have a pump there";
                }
                else if (!plotCheckPassed)
                {
                    invalidString = "This pump has been consumed";
                }
                else
                {
                    invalidString = "This move is invalid";
                }
                invalidMoveSound.Invoke(invalidString);
            }
        }
        else
        {
            //Valid move if this is not the same as the first location, and the destination is empty
            if (plotSelectionIndex == plotLocationIndex)
            {
                invalidMoveSound.Invoke("You have selected the same position");
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
                    invalidMoveSound.Invoke(manager.IsPumpAtLocation(plotLocationIndex) ?
                        "There is already a pump there" : "The ooze has consumed this location");
                }
            }
        }
    }


    public void HandlePlotSelection(int plotLocationIndex)
    {
        if (!manager.WaitingForUserSelection)
        {
            return;
        }
        if (manager.CurrentGameState == moveState)
        {
            HandleMoveStateOptions(plotLocationIndex);
        }
        else if (manager.CurrentGameState == removeState || manager.CurrentGameState == placeState)
        {
            if (plotLocationIndex == -1)
            {
                if (manager.CurrentGameState == removeState)
                {
                    invalidMoveSound.Invoke("The center base does not have ooze");
                }
                else
                {
                    invalidMoveSound.Invoke("The center base cannot have ooze");
                }
                navigation.CancelSelection();
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
        if (!manager.WaitingForUserSelection)
        {
            return;
        }
        if (manager.CurrentGameState == placeState)
        {
            if (plotSelectionIndex == -1)
            {
                invalidMoveSound.Invoke("The central base cannot have ooze");
            }
            else
            {
                if (mineLocationIndex == -1)
                {
                    bool placedCpu = manager.CpuPumps.Where(x => x.LocationIndex == plotSelectionIndex).Any();
                    bool placedPlayer = manager.PlayerPumps.Where(x => x.LocationIndex == plotSelectionIndex).Any();
                    if (placedCpu)
                    {
                        invalidMoveSound.Invoke("You need more ooze to halt a pump");
                    }
                    else if (placedPlayer)
                    {
                        invalidMoveSound.Invoke("Are you sure you want to do that?");
                    }
                    else
                    {
                        invalidMoveSound.Invoke("Ooze pools already have ooze");
                    }

                    return;
                }
                var plot = manager.MiningPlots.Where(x => x.LocationIndex == plotSelectionIndex).First();
                var mine = plot.GetMinLocationAtLocationIndex(mineLocationIndex);
                if (mine.HasSlime)
                {
                    invalidMoveSound.Invoke("The ooze has already consumed this spot");
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
                invalidMoveSound.Invoke("There is no ooze on the central base");
            }
            else
            {
                if (mineLocationIndex == -1)
                {
                    invalidMoveSound.Invoke("Leave the pools to the pumps");
                    return;
                }
                var plot = manager.MiningPlots.Where(x => x.LocationIndex == plotSelectionIndex).First();
                bool isValid = manager.IsPumpAtLocation(plotSelectionIndex)
                    && manager.PlayerPumps.Where(x => x.LocationIndex == plotSelectionIndex).Any()
                    && !manager.MiningPlots.Where(x => x.LocationIndex == plotSelectionIndex).First().AreAllFull();
                var mine = plot.GetMinLocationAtLocationIndex(mineLocationIndex);
                if (mine.HasSlime && isValid)
                {
                    mine.HasSlime = false;
                    validMoveSound.Invoke();
                    manager.HasPlayerMadeSelection = true;
                    //Prevents player from having selected plot for move
                    navigation.CancelSelection();
                    navigation.CancelSelection();
                }
                else
                {
                    invalidMoveSound.Invoke(!isValid ? "You can only remove ooze from plots where you have a pump" : "There is no ooze here");
                }
            }
        }
    }
}
