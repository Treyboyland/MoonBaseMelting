using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    string sceneName;

    [SerializeField]
    LoadSceneMode loadSceneMode;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName, loadSceneMode);
    }
}
