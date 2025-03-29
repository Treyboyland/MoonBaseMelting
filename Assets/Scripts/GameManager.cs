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

    public GameStateSO CurrentGameState { get => currentState; }

    public bool HasPlayerMadeSelection { get => hasUserMadeSelection; set => hasUserMadeSelection = value; }
    public List<MiningPump> PlayerPumps { get => playerPumps; }
    public List<MiningPlot> MiningPlots { get => miningPlots; }


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
        for (int i = 0; i < timesToWait; i++)
        {
            hasUserMadeSelection = false;
            while (!hasUserMadeSelection)
            {
                yield return null;
            }
            hasUserMadeSelection = false;
        }
    }

    IEnumerator GameSteps(bool playerFirst)
    {
        //MOVE
        UpdateCurrentState(moveState);
        if (playerFirst)
        {
            yield return StartCoroutine(WaitForUserSelection(1));
            brain.DetermineMoveAction(miningPlots, cpuPumps, playerPumps);
        }
        else
        {
            brain.DetermineMoveAction(miningPlots, cpuPumps, playerPumps);
            yield return StartCoroutine(WaitForUserSelection(1));
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
            brain.DetermineOozePlacementAction(miningPlots, cpuPumps, playerPumps);
        }
        else
        {
            brain.DetermineOozePlacementAction(miningPlots, cpuPumps, playerPumps);
            yield return WaitForUserSelection(1);
        }

        //REMOVE - Base instructions say *can*, I am changing that to must, I think
        UpdateCurrentState(removeState);
        if (playerFirst)
        {
            yield return WaitForUserSelection(1);
            brain.DetermineOozeRemovalAction(miningPlots, cpuPumps, playerPumps);
        }
        else
        {
            brain.DetermineOozeRemovalAction(miningPlots, cpuPumps, playerPumps);
            yield return WaitForUserSelection(1);
        }

    }

    IEnumerator GameLoop()
    {
        while (!oozeSpread.ShouldEndGame())
        {
            yield return StartCoroutine(GameSteps(playerFirst));
            playerFirst = !playerFirst;
        }
        Debug.LogWarning("GAME OVER");
    }

    void RunEndGameStuff()
    {
        Debug.LogWarning("GAME OVER!!!");
        gameEndEvent.Invoke(oozeSpread.ArePumpsConsumed(cpuPumps));
    }

    public bool IsPumpAtLocation(int plotLocationIndex)
    {
        return playerPumps.Where(x=> x.LocationIndex == plotLocationIndex).Any() 
            || cpuPumps.Where(x=> x.LocationIndex == plotLocationIndex).Any();
    }
}
