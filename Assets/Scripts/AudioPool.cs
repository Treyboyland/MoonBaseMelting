using UnityEngine;

public class AudioPool : MonoPool<PlayAudioThenDisable>
{
    public void PlaySound(AudioData data)
    {
        var item = GetItem();
        data.SetAudioData(item.Source);
        item.gameObject.SetActive(true);
    }
}
