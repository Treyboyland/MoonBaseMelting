using System.Collections.Generic;
using UnityEngine;

public class GameEventGeneric<T> : GameEvent
{
    public T Value;

    List<GameEventListenerGeneric<T>> listeners = new List<GameEventListenerGeneric<T>>();

    public void AddListener(GameEventListenerGeneric<T> listener)
    {
        if (listener != null && !listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void RemoveListener(GameEventListenerGeneric<T> listener)
    {
        listeners.Remove(listener);
    }

    public override void Invoke()
    {
        foreach (var listener in listeners)
        {
            listener.Response.Invoke(Value);
        }
    }
}
