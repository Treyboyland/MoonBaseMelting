using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBrain-Random", menuName = "Enemy Brains/Random")]
public class EnemyBrainRandom : EnemyBrain
{
    public override void DetermineMoveAction(List<MiningPlot> plots, List<MiningPump> cpuPumps, List<MiningPump> playerPumps)
    {
        var moveablePumps = cpuPumps.Where(x => x.LocationIndex == -1).ToList();
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
            return;
        }
    }

    public override void DetermineOozeRemovalAction(List<MiningPlot> plots, List<MiningPump> cpuPumps, List<MiningPump> playerPumps)
    {
        var cpuIndices = cpuPumps.Select(x => x.LocationIndex).ToList();
        var cpuPlots = plots.Where(x => cpuIndices.Contains(x.LocationIndex) && !x.AreAllFull()).ToList();


        if (cpuPlots.Count != 0)
        {
            var plotSelected = cpuPlots.GetRandomItem();
            var chosenIndex = plotSelected.GetSlimedLocations().GetRandomItem();
            plotSelected.RemoveOozeAtLocation(chosenIndex);
            return;
        }
    }
}
