using DG.Tweening;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioMaganer;
    private AudioSource audioMaganer2;
    public AudioDates infoAudio;
    [SerializeField] private bool controlAudio = true;

    private void Awake()
    {
        if (!controlAudio) { return; }
        audioMaganer = GetComponent<AudioSource>();
        audioMaganer2 = GameObject.Find("SoundManager-2").GetComponent<AudioSource>();
    }

    public void PlaySoundAmbient(int sound)
    {
        if (!controlAudio) { return; }
        int index = sound;
        float volumenMax = 0.35f;
        float timeIn = 1.5f;
        AudioClip newClip = infoAudio.getAudioAmbient(index);

        // Caso 1: el primer AudioSource está libre
        if (audioMaganer.clip == null || !audioMaganer.isPlaying)
        {
            // Si el segundo está sonando, lo apagamos con fade
            if (audioMaganer2.isPlaying)
            {
                audioMaganer2.DOFade(0f, timeIn).OnComplete(() => audioMaganer2.Stop());
            }
            audioMaganer.clip = newClip;
            audioMaganer.loop = true;
            audioMaganer.volume = 0f;
            audioMaganer.Play();
            audioMaganer.DOFade(volumenMax, timeIn); // fade in
        }
        else
        {
            // Caso 2: el primero ya está ocupado → usamos audioMaganer2
            if (audioMaganer.isPlaying)
            {
                audioMaganer.DOFade(0f, timeIn).OnComplete(() => audioMaganer.Stop());
            }

            audioMaganer2.clip = newClip;
            audioMaganer2.loop = true;
            audioMaganer2.volume = 0f;
            audioMaganer2.Play();
            audioMaganer2.DOFade(volumenMax, timeIn); // fade in
        }
    }

    public void StopSound()
    {
        if (!controlAudio) { return; }
        // Baja el volumen a 0 en 2 segundos
        audioMaganer.DOFade(0f, 0f).OnComplete(() => audioMaganer.Stop());
        audioMaganer2.DOFade(0f, 0f).OnComplete(() => audioMaganer.Stop());
    }
}
