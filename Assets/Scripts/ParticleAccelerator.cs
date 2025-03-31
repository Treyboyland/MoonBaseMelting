using System.Collections;
using UnityEngine;

public class ParticleAccelerator : MonoBehaviour
{
    [SerializeField]
    ParticleSystem particle;

    [SerializeField]
    float secondsToAccelerate;

    [SerializeField]
    float acceleratedSpeed;

    public ParticleSystem Particle { get => particle; }

    float initialSpeed;

    void Awake()
    {
        initialSpeed = particle.main.simulationSpeed;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Accelerate()
    {
        StopAllCoroutines();
        StartCoroutine(Speedup());
    }

    void SetSpeed(float speed)
    {
        var main = particle.main;
        main.simulationSpeed = speed;
    }

    IEnumerator Speedup()
    {
        SetSpeed(acceleratedSpeed);
        float elapsed = 0;

        while (elapsed < secondsToAccelerate)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetSpeed(initialSpeed);
    }
}
