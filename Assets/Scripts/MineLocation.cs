using UnityEngine;

public class MineLocation : CoordinateHaver
{
    [SerializeField]
    int locationIndex;

    [SerializeField]
    bool hasSlime;

    [SerializeField]
    ParticleSystem particle;

    public int LocationIndex { get => locationIndex; }

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
