using UnityEngine;

public class MineLocation : MonoBehaviour
{
    [SerializeField]
    Vector2Int coordinates;

    [SerializeField]
    int locationIndex;

    [SerializeField]
    bool hasSlime;

    [SerializeField]
    ParticleSystem particle;

    public int LocationIndex { get => locationIndex; }
    public Vector2Int Coordinates { get => coordinates; }
    public bool HasSlime
    {
        get => hasSlime;
        set
        {
            hasSlime = value;
            if (!hasSlime && particle.isPlaying)
            {
                particle.Stop();
            }
            else if (hasSlime && !particle.isPlaying)
            {
                particle.Play();
            }
        }
    }
}
