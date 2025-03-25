using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiningPlot : MonoBehaviour
{
    [SerializeField]
    List<MineLocation> locations = new List<MineLocation>();

    [SerializeField]
    ParticleSystem coverParticle;

    [SerializeField]
    int locationIndex;

    public int LocationIndex => locationIndex;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (AreAllFull() && !coverParticle.isPlaying)
        {
            coverParticle.Play();
        }
    }

    public bool AreAllFull()
    {
        foreach (var location in locations)
        {
            if (!location.HasSlime)
            {
                return false;
            }
        }

        return true;
    }

    public static List<int> GetAdjacentMiningPlots(int locationIndex)
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

    bool Cascade(int index, List<int> alreadyCheckedPlots)
    {
        if (AreAllFull())
        {
            return true;
        }

        if (!alreadyCheckedPlots.Contains(index))
        {
            alreadyCheckedPlots.Add(index);
        }

        var chosenLoc = locations.Where(x => x.LocationIndex == index).First();
        if (chosenLoc.HasSlime)
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
            chosenLoc.HasSlime = true;
            return true;
        }
    }

    public void PlaceOozeAtRandomLocation()
    {
        int chosenIndex = UnityEngine.Random.Range(1, 9);
        Cascade(chosenIndex, new List<int>());
    }

    public void PlaceOozeAtLocation(int index)
    {
        Cascade(index, new List<int>());
    }
}
