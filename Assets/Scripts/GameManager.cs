using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    OozeSpread oozeSpread;

    [SerializeField]
    EnemyBrain brain;

    [SerializeField]
    List<MiningPlot> miningPlots;

    [SerializeField]
    List<MiningPump> playerPumps;

    [SerializeField]
    List<MiningPump> cpuPumps;

    [SerializeField]
    float secondsBetweenMoves;

    bool playerFirst;

    [SerializeField]
    GameStateSO moveState, generateState, placeState, removeState;

    [SerializeField]
    GameStateSO currentState;

    [SerializeField]
    GameEventGeneric<GameStateSO> stateUpdateEvent;

    [SerializeField]
    GameEventGeneric<bool> gameEndEvent;

    bool hasUserMadeSelection = false;
    bool endGameStarted = false;

    bool waitingForUserSelection = false;

    public GameStateSO CurrentGameState { get => currentState; }

    public bool HasPlayerMadeSelection { get => hasUserMadeSelection; set => hasUserMadeSelection = value; }
    public bool WaitingForUserSelection { get => waitingForUserSelection; set => waitingForUserSelection = value; }
    public List<MiningPump> PlayerPumps { get => playerPumps; }
    public List<MiningPlot> MiningPlots { get => miningPlots; }
    public List<MiningPump> CpuPumps { get => cpuPumps; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerFirst = Random.Range(0.0f, 1.0f) <= .5f;
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (oozeSpread.ShouldEndGame() && !endGameStarted)
        {
            endGameStarted = true;
            StopAllCoroutines();
            RunEndGameStuff();
        }
    }

    void UpdateCurrentState(GameStateSO newState)
    {
        currentState = newState;
        stateUpdateEvent.Invoke(currentState);
    }

    IEnumerator WaitForUserSelection(int timesToWait)
    {
        waitingForUserSelection = true;
        for (int i = 0; i < timesToWait; i++)
        {
            hasUserMadeSelection = false;
            while (!hasUserMadeSelection)
            {
                if (!PlayerHasValidMove())
                {
                    waitingForUserSelection = false;
                    break;
                }
                yield return null;
            }
            hasUserMadeSelection = false;
        }
        waitingForUserSelection = false;
    }

    private bool PlayerHasValidMove()
    {
        if (currentState == moveState)
        {
            var pumpIndicesOut = playerPumps.Select(x => x.LocationIndex).Where(x => x != -1);
            var pumpIndicesHub = playerPumps.Select(x => x.LocationIndex).Where(x => x == -1);
            var cpuIndices = cpuPumps.Select(x => x.LocationIndex).Where(x => x != -1);
            var fullPlots = miningPlots.Where(x => x.AreAllFull()).Select(x => x.LocationIndex);
            return pumpIndicesOut.Where(x => !fullPlots.Contains(x)).Any() || //Out pumps can always move to base
                (fullPlots.Union(cpuIndices).Count() < 8 && pumpIndicesHub.Count() != 0); //Pump at base can move out
        }
        if (currentState == generateState)
        {
            return false;
        }
        if (currentState == placeState)
        {
            var fullPlots = miningPlots.Where(x => x.AreAllFull());
            return fullPlots.Count() < miningPlots.Count;
        }
        if (currentState == removeState)
        {
            var pumpIndices = playerPumps.Select(x => x.LocationIndex);
            return miningPlots.Where(x => pumpIndices.Contains(x.LocationIndex) && !x.AreAllFull()).Any();
        }
        return false;
    }

    IEnumerator GameSteps(bool playerFirst)
    {
        //MOVE
        UpdateCurrentState(moveState);
        if (playerFirst)
        {
            yield return StartCoroutine(WaitForUserSelection(1));
            yield return new WaitForSeconds(secondsBetweenMoves);
            brain.DetermineMoveAction(miningPlots, cpuPumps, playerPumps);
            yield return new WaitForSeconds(secondsBetweenMoves);
        }
        else
        {
            brain.DetermineMoveAction(miningPlots, cpuPumps, playerPumps);
            yield return new WaitForSeconds(secondsBetweenMoves);
            yield return StartCoroutine(WaitForUserSelection(1));
            yield return new WaitForSeconds(secondsBetweenMoves);
        }

        //GENERATE
        UpdateCurrentState(generateState);
        for (int i = 0; i < 4; i++)
        {
            oozeSpread.BeginSpread();
        }
        yield return new WaitForSeconds(1.5f);

        //PLACE
        UpdateCurrentState(placeState);
        if (playerFirst)
        {
            yield return WaitForUserSelection(1);
            yield return new WaitForSeconds(secondsBetweenMoves);
            brain.DetermineOozePlacementAction(miningPlots, cpuPumps, playerPumps);
            yield return new WaitForSeconds(secondsBetweenMoves);
        }
        else
        {
            brain.DetermineOozePlacementAction(miningPlots, cpuPumps, playerPumps);
            yield return new WaitForSeconds(secondsBetweenMoves);
            yield return WaitForUserSelection(1);
            yield return new WaitForSeconds(secondsBetweenMoves);
        }

        //REMOVE - Base instructions say *can*, I am changing that to must, I think
        UpdateCurrentState(removeState);
        if (playerFirst)
        {
            yield return WaitForUserSelection(1);
            yield return new WaitForSeconds(secondsBetweenMoves);
            brain.DetermineOozeRemovalAction(miningPlots, cpuPumps, playerPumps);
            yield return new WaitForSeconds(secondsBetweenMoves);
        }
        else
        {
            brain.DetermineOozeRemovalAction(miningPlots, cpuPumps, playerPumps);
            yield return new WaitForSeconds(secondsBetweenMoves);
            yield return WaitForUserSelection(1);
            yield return new WaitForSeconds(secondsBetweenMoves);
        }

    }

    IEnumerator GameLoop()
    {
        while (!oozeSpread.ShouldEndGame())
        {
            yield return StartCoroutine(GameSteps(playerFirst));
            playerFirst = !playerFirst;
        }
    }

    void RunEndGameStuff()
    {
        gameEndEvent.Invoke(oozeSpread.ArePumpsConsumed(cpuPumps));
    }

    public bool IsPumpAtLocation(int plotLocationIndex)
    {
        return playerPumps.Where(x => x.LocationIndex == plotLocationIndex).Any()
            || cpuPumps.Where(x => x.LocationIndex == plotLocationIndex).Any();
    }
}
