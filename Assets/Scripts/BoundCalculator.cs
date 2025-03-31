using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoundCalculator : MonoBehaviour
{
    [SerializeField]
    List<Renderer> renderers;

    public Bounds GetBounds()
    {
        var bounds = renderers.Select(x => x.GetComponent<Renderer>().bounds);
        Vector3 max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        Vector3 min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

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

        return new Bounds(center / renderers.Count, size);
    }
}
