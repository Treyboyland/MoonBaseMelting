using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiningPlot : CoordinateHaver
{
    [SerializeField]
    List<MineLocation> locations = new List<MineLocation>();

    [SerializeField]
    List<CoordinateHaver> plotCoordinates;

    [SerializeField]
    ParticleSystem coverParticle;

    [SerializeField]
    int locationIndex;

    public int LocationIndex => locationIndex;

    [SerializeField]
    GameEventGeneric<Bounds> onSlimePlaced;


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

    public List<int> GetSlimedLocations()
    {
        List<int> toReturn = new List<int>();
        foreach (var location in locations)
        {
            if (location.HasSlime)
            {
                toReturn.Add(location.LocationIndex);
            }
        }

        return toReturn;
    }

    public List<int> GetNotSlimedLocations()
    {
        List<int> toReturn = new List<int>();
        foreach (var location in locations)
        {
            if (!location.HasSlime)
            {
                toReturn.Add(location.LocationIndex);
            }
        }

        return toReturn;
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

    bool Cascade(int index, List<int> alreadyCheckedPlots, bool fireEvent = false)
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
                    oozePlaced |= Cascade(pos, alreadyCheckedPlots, fireEvent);
                }
            }
            return oozePlaced;
        }
        else
        {
            chosenLoc.HasSlime = true;
            if (fireEvent)
            {
                onSlimePlaced.Invoke(GetBoundsOfLocationIndex(chosenLoc.LocationIndex));
            }
            return true;
        }
    }

    public void PlaceOozeAtRandomLocation(bool shouldFireEvent = false)
    {
        int chosenIndex = UnityEngine.Random.Range(1, 9);
        Cascade(chosenIndex, new List<int>(), shouldFireEvent);
    }

    public void PlaceOozeAtLocation(int index, bool shouldFireEvent = false)
    {
        Cascade(index, new List<int>(), shouldFireEvent);
    }


    public Bounds GetBoundsOfCoordinate(Vector2Int coord)
    {
        var coordObj = plotCoordinates.Where(x => x.Coordinates == coord).First();
        var renderer = coordObj.gameObject.GetComponent<Renderer>();

        return renderer.bounds;
    }

    public Bounds GetBoundsOfLocationIndex(int index)
    {
        if (index == -1)
        {
            return GetBoundsOfCoordinate(Vector2Int.zero);
        }
        return locations.Where(x => x.LocationIndex == index).First().GetComponent<Renderer>().bounds;
    }

    public MineLocation GetMineLocationAtCoordinate(Vector2Int coord)
    {
        return plotCoordinates.Where(x => x.Coordinates == coord).First().GetComponent<MineLocation>();
    }

    public MineLocation GetMinLocationAtLocationIndex(int index)
    {
        return locations.Where(x => x.LocationIndex == index).First();
    }

    public Bounds GetBounds()
    {
        Vector3 max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        Vector3 min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        var bounds = plotCoordinates.Select(x => x.GetComponent<Renderer>().bounds);

        Vector3 center = Vector3.zero;

        foreach (var bound in bounds)
        {
            center += bound.center;

            max.x = Mathf.Max(max.x, bound.max.x);
            max.y = Mathf.Max(max.y, bound.max.y);
            max.z = Mathf.Max(max.z, bound.max.z);

            min.x = Mathf.Min(min.x, bound.min.x);
            min.y = Mathf.Min(min.y, bound.min.y);
            min.z = Mathf.Min(min.z, bound.min.z);


        }

        Vector3 size = max - min;

        return new Bounds(center / plotCoordinates.Count, size);
    }

    public void RemoveOozeAtLocation(int chosenIndex)
    {
        var chosen = locations.Where(x => x.LocationIndex == chosenIndex).First();
        chosen.HasSlime = false;
    }
}
