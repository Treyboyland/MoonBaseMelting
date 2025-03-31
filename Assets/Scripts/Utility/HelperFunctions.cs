using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    public static T GetRandomItem<T>(this List<T> items)
    {
        return items[UnityEngine.Random.Range(0, items.Count)];
    }

    /// <summary>
    /// Shuffles the list of items in place
    /// </summary>
    /// <param name="items"></param>
    /// <typeparam name="T"></typeparam>
    public static void Shuffle<T>(this List<T> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            int chosenIndex = UnityEngine.Random.Range(i, items.Count);
            var temp = items[chosenIndex];
            items[chosenIndex] = items[i];
            items[i] = temp;
        }
    }

    public static float Randomize(this Vector2 vector)
    {
        return UnityEngine.Random.Range(vector.x, vector.y);
    }
}