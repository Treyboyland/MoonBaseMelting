using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerGeneric<T> : MonoBehaviour
{
    [SerializeField]
    GameEventGeneric<T> gameEvent;

    public UnityEvent<T> Response;

    void OnEnable()
    {
        gameEvent.AddListener(this);
    }

    void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }
}
