using System.Collections;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField]
    GameObject lossObject;

    [SerializeField]
    GameEvent loadEndScreen;

    [SerializeField]
    float secondsToWait;

    bool startedResolution;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lossObject.gameObject.SetActive(false);
    }

    public void HandleResolution(bool playerWon)
    {
        if (!startedResolution)
        {
            startedResolution = true;
            StartCoroutine(StartEndGame(playerWon));
        }
    }

    IEnumerator StartEndGame(bool playerWon)
    {
        yield return new WaitForSeconds(secondsToWait);
        if (playerWon)
        {
            loadEndScreen.Invoke();
        }
        else
        {
            lossObject.SetActive(true);
        }
    }
}
