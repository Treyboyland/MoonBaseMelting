using UnityEngine;

public class SelectorController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer selectorObject;

    [SerializeField]
    Vector2 padding;

    public bool CanSelect
    {
        get
        {
            return selectorObject.gameObject.activeInHierarchy;
        }
        set
        {
            selectorObject.gameObject.SetActive(value);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetPosition(Vector2 pos)
    {
        selectorObject.transform.position = pos;
    }

    public void SetScale(Vector2 scale)
    {
        selectorObject.size = scale + padding;
    }
}
