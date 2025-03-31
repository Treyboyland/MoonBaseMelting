using UnityEngine;

public class MineLocation : CoordinateHaver
{
    [SerializeField]
    int locationIndex;

    [SerializeField]
    bool hasSlime;

    [SerializeField]
    ParticleAccelerator particle;

    public int LocationIndex { get => locationIndex; }

    public bool HasSlime
    {
        get => hasSlime;
        set
        {
            hasSlime = value;
            if (!hasSlime && particle.Particle.isPlaying)
            {
                particle.Particle.Stop();
                particle.Accelerate();
            }
            else if (hasSlime && !particle.Particle.isPlaying)
            {
                particle.Particle.Play();
                particle.Accelerate();
            }
        }
    }
}
