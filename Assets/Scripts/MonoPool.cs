using System.Collections.Generic;
using UnityEngine;

public class MonoPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    T objectPrefab;

    List<T> pool = new List<T>();

    T CreateObject()
    {
        var newObj = Instantiate(objectPrefab, transform);
        newObj.gameObject.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }

    public T GetItem()
    {
        foreach (var item in pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                return item;
            }
        }

        return CreateObject();
    }
}
