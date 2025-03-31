using System.Collections;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    [SerializeField]
    float secondsToWait;

    float elapsed = 0;

    void OnEnable()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(WaitThenDisable());
        }
    }

    IEnumerator WaitThenDisable()
    {
        elapsed = 0;
        while (elapsed < secondsToWait)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    public void ResetTime()
    {
        elapsed = 0;
    }
}
