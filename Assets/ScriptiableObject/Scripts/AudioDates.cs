using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "Scriptable Objects/Audio")]
public class AudioDates : ScriptableObject
{
    [SerializeField] private AudioClip[] audioAmbient;
    [SerializeField] private AudioClip[] audioEffec;

    public AudioClip getAudioAmbient(int index)
    {
        return audioAmbient[index];
    }
    public AudioClip getEffect(int index)
    {
        return audioEffec[index];
    }
}
