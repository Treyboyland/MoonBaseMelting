using UnityEngine;

public class CoordinateHaver : MonoBehaviour
{
    [SerializeField]
    protected Vector2Int coordinates;

    public Vector2Int Coordinates { get => coordinates; set => coordinates = value; }

}
