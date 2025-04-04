using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OozeSpread : MonoBehaviour
{
    [SerializeField]
    List<MiningPlot> miningPlots;

    [SerializeField]
    bool randomizeStartingOoze;

    [SerializeField]
    List<MiningPump> miningPumps;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetStartingOoze();
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool AllPlotsFull()
    {
        foreach (var plot in miningPlots)
        {
            if (!plot.AreAllFull())
            {
                return false;
            }
        }

        return true;
    }

    public bool ArePumpsConsumed(IEnumerable<MiningPump> pumps)
    {
        foreach (var pump in pumps)
        {
            var pumpLocation = miningPlots.Where(x => x.LocationIndex == pump.LocationIndex).FirstOrDefault();
            if (pumpLocation == default(MiningPump)) //Pump has not been placed
            {
                return false;
            }
            if (!pumpLocation.AreAllFull()) //The ooze can still spread at this pump location
            {
                return false;
            }
        }

        return true;
    }

    bool EitherPlayerPumpsLost()
    {
        var player = miningPumps.Where(x => x.IsPlayer);
        var enemy = miningPumps.Where(x => !x.IsPlayer);

        return ArePumpsConsumed(player) || ArePumpsConsumed(enemy);
    }


    /// <summary>
    /// True if game should end
    /// </summary>
    /// <returns></returns>
    public bool ShouldEndGame()
    {
        //TODO: Game ends when all of a player's ooze pumps have been consumed...
        //Is there a situation where both players cannot place (i.e. slime has
        // spread in such a way that their based pump cannot move?)

        return AllPlotsFull() || EitherPlayerPumpsLost();
    }

    bool Cascade(int index, List<int> alreadyCheckedPlots)
    {
        if (ShouldEndGame())
        {
            return true;
        }

        if (!alreadyCheckedPlots.Contains(index))
        {
            alreadyCheckedPlots.Add(index);
        }

        var chosenLoc = miningPlots.Where(x => x.LocationIndex == index).First();
        if (chosenLoc.AreAllFull())
        {
            var adjacents = GetAdjacentMiningPlots(index);
            bool oozePlaced = false;
            foreach (var pos in adjacents)
            {
                if (!alreadyCheckedPlots.Contains(pos))
                {
                    oozePlaced |= Cascade(pos, alreadyCheckedPlots);
                }
            }
            return oozePlaced;
        }
        else
        {
            chosenLoc.PlaceOozeAtRandomLocation(true);
            return true;
        }
    }

    public void BeginSpread()
    {
        int chosenLocation = Random.Range(1, 9); //Roll 1-8
        Cascade(chosenLocation, new List<int>());
    }

    List<int> GetAdjacentMiningPlots(int locationIndex)
    {
        /*
        1 2 3
        4 X 5
        6 7 8
        */
        switch (locationIndex)
        {
            case 1:
                return new List<int> { 2, 4 };
            case 2:
                return new List<int> { 1, 3 };
            case 3:
                return new List<int> { 2, 5 };
            case 4:
                return new List<int> { 1, 6 };
            case 5:
                return new List<int> { 3, 8 };
            case 6:
                return new List<int> { 4, 7 };
            case 7:
                return new List<int> { 6, 8 };
            case 8:
                return new List<int> { 5, 7 };
            default:
                return new List<int>();
        }
    }

    void SetStartingOoze()
    {
        foreach (var plot in miningPlots)
        {
            if (randomizeStartingOoze)
            {
                plot.PlaceOozeAtRandomLocation();
            }
            else
            {
                plot.PlaceOozeAtLocation(plot.LocationIndex);
            }
        }
    }
}
