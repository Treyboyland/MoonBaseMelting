using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent-On", menuName = "Events/No Params", order = -1)]
public class GameEvent : ScriptableObject
{
    List<GameEventListener> listeners = new List<GameEventListener>();

    public void AddListener(GameEventListener listener)
    {
        if (listener != null && !listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void RemoveListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public virtual void Invoke()
    {
        foreach (var listener in listeners)
        {
            listener.Response.Invoke();
        }
    }
}
