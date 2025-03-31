using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBrain-Random", menuName = "Enemy Brains/Random")]
public class EnemyBrainRandom : EnemyBrain
{
    public override void DetermineMoveAction(List<MiningPlot> plots, List<MiningPump> cpuPumps, List<MiningPump> playerPumps)
    {
        //Prioritize getting pumps out, and then moving them around
        var fullPlotIndices = plots.Where(x => x.AreAllFull()).Select(x => x.LocationIndex);
        var moveablePumps = cpuPumps.Where(x => x.LocationIndex == -1).Any()
            ? cpuPumps.Where(x => x.LocationIndex == -1).ToList()
            : cpuPumps.Where(x => !fullPlotIndices.Contains(x.LocationIndex)).ToList();
        if (moveablePumps.Count != 0)
        {
            var allPumps = new List<MiningPump>();
            allPumps.AddRange(cpuPumps);
            allPumps.AddRange(playerPumps);
            var potentialLocations = GetEmptyPlots(plots, allPumps).ToList();
            if (potentialLocations.Count != 0)
            {
                var chosenLoc = potentialLocations.GetRandomItem();
                var chosenPump = moveablePumps.GetRandomItem();
                chosenPump.LocationIndex = chosenLoc.LocationIndex;
                onHighlightMove.Invoke(chosenLoc.GetBounds());
            }
            else
            {
                var chosenPump = moveablePumps.GetRandomItem();
                chosenPump.LocationIndex = -1;
                onHighlightMove.Invoke(chosenPump.GetBoundsOfStartingLocation());
            }
        }
    }

    public override void DetermineOozePlacementAction(List<MiningPlot> plots, List<MiningPump> cpuPumps, List<MiningPump> playerPumps)
    {
        //Grab plots by category
        var playerIndices = playerPumps.Select(x => x.LocationIndex);
        var playerPlots = plots.Where(x => playerIndices.Contains(x.LocationIndex) && !x.AreAllFull()).ToList();

        var allPumps = new List<MiningPump>();
        allPumps.AddRange(cpuPumps);
        allPumps.AddRange(playerPumps);
        var emptyPlots = GetEmptyPlots(plots, allPumps);

        var cpuIndices = cpuPumps.Select(x => x.LocationIndex).ToList();
        var cpuPlots = plots.Where(x => cpuIndices.Contains(x.LocationIndex) && !x.AreAllFull()).ToList();

        //Prefer slime placement based upon what is least harmful to cpu
        List<MiningPlot> chosenPlot;

        if (playerPlots.Count != 0)
        {
            chosenPlot = playerPlots;
        }
        else if (emptyPlots.Count != 0)
        {
            chosenPlot = emptyPlots;
        }
        else
        {
            chosenPlot = cpuPlots;
        }


        if (chosenPlot.Count != 0)
        {
            var plotSelected = chosenPlot.GetRandomItem();
            var chosenIndex = plotSelected.GetNotSlimedLocations().GetRandomItem();
            plotSelected.PlaceOozeAtLocation(chosenIndex);
            onHighlightMove.Invoke(plotSelected.GetBoundsOfLocationIndex(chosenIndex));
            return;
        }
    }

    public override void DetermineOozeRemovalAction(List<MiningPlot> plots, List<MiningPump> cpuPumps, List<MiningPump> playerPumps)
    {
        var cpuIndices = cpuPumps.Select(x => x.LocationIndex).ToList();
        var cpuPlots = plots.Where(x => cpuIndices.Contains(x.LocationIndex) && !x.AreAllFull() && x.GetSlimedLocations().Count > 0).ToList();

        if (cpuPlots.Count != 0)
        {
            var plotSelected = cpuPlots.GetRandomItem();
            var chosenIndex = plotSelected.GetSlimedLocations().GetRandomItem();
            plotSelected.RemoveOozeAtLocation(chosenIndex);
            onHighlightMove.Invoke(plotSelected.GetBoundsOfLocationIndex(chosenIndex));
            return;
        }
    }
}
