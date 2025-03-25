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

    public int LocationIndex { get => locationIndex; set => locationIndex = value; }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator.runtimeAnimatorController = isPlayer ? player : enemy;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsPumping", locationIndex <= 0);
    }
}
