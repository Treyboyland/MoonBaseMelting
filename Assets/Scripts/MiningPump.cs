using System.Collections;
using UnityEngine;

public class MiningPump : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    RuntimeAnimatorController player;

    [SerializeField]
    RuntimeAnimatorController enemy;

    [SerializeField]
    int locationIndex;

    [SerializeField]
    bool isPlayer;

    [SerializeField]
    float secondsToMove;

    [SerializeField]
    GameEventGeneric<PumpAndDestinationIndex> onMoveToIndex;

    public int LocationIndex
    {
        get => locationIndex;
        set
        {
            locationIndex = value;
            onMoveToIndex.Invoke(new PumpAndDestinationIndex() { Pump = this, DestinationIndex = locationIndex });
        }
    }
    public bool IsPlayer { get => isPlayer; }


    Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator.runtimeAnimatorController = isPlayer ? player : enemy;
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsPumping", locationIndex > 0);
    }

    public void MoveToLocation(Vector3 newPos)
    {
        StopAllCoroutines();
        StartCoroutine(MovementOverTime(newPos));
    }

    public void MoveToStartingLocation()
    {
        MoveToLocation(startingPos);
    }

    IEnumerator MovementOverTime(Vector3 newPos)
    {
        float elapsed = 0;
        Vector3 startPos = transform.position;
        while (elapsed < secondsToMove)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, newPos, elapsed / secondsToMove);
            yield return null;
        }
        transform.position = newPos;
    }
}
