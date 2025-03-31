using UnityEngine;

public class HighlighterPool : MonoPool<ActionHighlighter>
{
    public void HighlightPosition(Bounds bounds)
    {
        var obj = GetItem();
        obj.SetBounds(bounds);
        obj.gameObject.SetActive(true);
    }
}
