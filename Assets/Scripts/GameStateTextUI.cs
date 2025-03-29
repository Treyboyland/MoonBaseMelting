using TMPro;
using UnityEngine;

public class GameStateTextUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text textBox;

    public void UpdateText(GameStateSO gameStateSO)
    {
        textBox.text = $"{gameStateSO.StateName}: {gameStateSO.Description}";
    }
}
