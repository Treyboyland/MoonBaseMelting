using System.Linq;
using UnityEngine;

public class PumpMover : MonoBehaviour
{
    [SerializeField]
    GameManager manager;


    public void MovePump(PumpAndDestinationIndex pumpData)
    {
        var destLoc = manager.MiningPlots.Where(x => x.LocationIndex == pumpData.DestinationIndex).First();
        pumpData.Pump.MoveToLocation(destLoc.GetBoundsOfCoordinate(Vector2Int.zero).center);
    }

}
