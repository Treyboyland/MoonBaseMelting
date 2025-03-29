using UnityEngine;

[CreateAssetMenu(fileName = "GameStateSO", menuName = "Scriptable Objects/GameStateSO")]
public class GameStateSO : ScriptableObject
{
    [SerializeField]
    string stateName;

    [TextArea]
    [SerializeField]
    string description;

    public string StateName { get => stateName; }
    public string Description { get => description; }
}
