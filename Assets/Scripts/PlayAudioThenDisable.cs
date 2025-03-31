using System.Collections;
using UnityEngine;

public class PlayAudioThenDisable : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    public AudioSource Source { get => source; }


    void OnEnable()
    {
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(WaitThenDisable());
        }
    }

    IEnumerator WaitThenDisable()
    {
        source.Play();

        while (source.isPlaying)
        {
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
