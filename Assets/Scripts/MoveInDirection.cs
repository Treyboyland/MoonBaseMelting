using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    [SerializeField]
    Vector3 direction;

    [SerializeField]
    float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += direction.normalized * Time.deltaTime * speed;
    }
}
