using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextLink : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    TMP_Text textBox;

    public void OnPointerClick(PointerEventData eventData)
    {

        var link = TMP_TextUtilities.FindIntersectingLink(textBox, eventData.position, null);
        if (link == -1)
        {
            return;
        }
        var linkInfo = textBox.textInfo.linkInfo[link];
        string linkID = linkInfo.GetLinkID();

        Application.OpenURL(linkID);
    }
}
