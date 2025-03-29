using UnityEngine;
using UnityEngine.InputSystem;

public class UserControl : MonoBehaviour
{
    [SerializeField]
    GameEventGeneric<Vector2Int> moveEvent;

    [SerializeField]
    GameEvent selectionEvent;

    [SerializeField]
    GameEvent cancelEvent;

    [SerializeField]
    float secondsBetweenMovementEventFires;

    Vector2 movementVector;

    float elapsed = 0;


    void Update()
    {
        elapsed += Time.deltaTime;
        if (movementVector != Vector2.zero && elapsed > secondsBetweenMovementEventFires)
        {
            elapsed = 0;
            FireMovementEvent();
        }
    }

    public void HandleMove(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }

    void FireMovementEvent()
    {
        Vector2Int moveVector = Vector2Int.zero;

        if (Mathf.Abs(movementVector.x) < .5)
        {
            movementVector.x = 0;
        }
        if (Mathf.Abs(movementVector.y) < .5)
        {
            movementVector.y = 0;
        }

        if (movementVector.x != 0)
        {
            moveVector.x = (int)Mathf.Sign(movementVector.x);
        }

        if (movementVector.y != 0)
        {
            moveVector.y = (int)Mathf.Sign(movementVector.y);
        }

        moveEvent.Invoke(moveVector);
    }

    public void HandleSelection(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            selectionEvent.Invoke();
        }
    }

    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            cancelEvent.Invoke();
        }
    }
}
