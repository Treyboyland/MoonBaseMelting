using UnityEngine;

public class ActionHighlighter : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer selectorObject;

    [SerializeField]
    Vector2 padding;

    public void SetPosition(Vector2 pos)
    {
        selectorObject.transform.position = pos;
    }

    public void SetScale(Vector2 scale)
    {
        selectorObject.size = scale + padding;
    }

    public void SetBounds(Bounds bounds)
    {
        SetPosition(bounds.center);
        SetScale(bounds.size);
    }
}
