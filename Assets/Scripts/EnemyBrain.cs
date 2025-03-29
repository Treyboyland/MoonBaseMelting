using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyBrain : ScriptableObject
{
    public List<MiningPlot> GetEmptyPlots(List<MiningPlot> plots, List<MiningPump> pumps)
    {
        var pumpIndices = pumps.Select(x => x.LocationIndex).ToList();

        return plots.Where(x => !pumpIndices.Contains(x.LocationIndex) && !x.AreAllFull()).ToList();
    }

    protected bool CanRemoveSlimeFromLocation(MiningPlot plot, List<MiningPump> cpuPumps)
    {
        return plot.GetSlimedLocations().Count != 0 && cpuPumps.Any(x => x.LocationIndex == plot.LocationIndex);
    }

    protected bool LocationContainsPump(List<MiningPump> playerPumps, List<MiningPump> cpuPumps, int location)
    {
        return playerPumps.Any(x => x.LocationIndex == location) || cpuPumps.Any(x => x.LocationIndex == location);
    }

    public abstract void DetermineMoveAction(List<MiningPlot> plots, List<MiningPump> cpuPumps, List<MiningPump> playerPumps);

    public abstract void DetermineOozePlacementAction(List<MiningPlot> plots, List<MiningPump> cpuPumps, List<MiningPump> playerPumps);

    public abstract void DetermineOozeRemovalAction(List<MiningPlot> plots, List<MiningPump> cpuPumps, List<MiningPump> playerPumps);
}
