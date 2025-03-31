using TMPro;
using UnityEngine;

public class InvalidMoveTextUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text textBox;

    [SerializeField]
    DisableAfterTime textParent;

    public void SetText(string text)
    {
        textBox.text = text;
        if (textParent.gameObject.activeInHierarchy)
        {
            textParent.ResetTime();
        }
        else
        {
            textParent.gameObject.SetActive(true);
        }
    }

    public void DisableNotification()
    {
        textParent.gameObject.SetActive(false);
    }
}
