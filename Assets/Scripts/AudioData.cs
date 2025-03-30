using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Scriptable Objects/AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField]
    List<AudioClip> clips;

    [SerializeField]
    Vector2 volumeRandomizer;

    [SerializeField]
    Vector2 pitchRandomizer;

    public void SetAudioData(AudioSource source)
    {
        source.clip = clips.GetRandomItem();
        source.pitch = pitchRandomizer.Randomize();
        source.volume = volumeRandomizer.Randomize();
    }
}
