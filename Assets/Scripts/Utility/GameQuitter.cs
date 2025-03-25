using UnityEngine;

public class GameQuitter : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
#elif !UNITY_WEBGL
        Application.Quit();
#endif
    }
}
