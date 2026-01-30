using UnityEngine;

public class AudioController : BasicActor
{
    private SoundManager soundManager;
    public AudioDates infoAudio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected override void Awake()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        base.Awake();
        entity.AddAction<float>("playSoundAmbient", PlaySoundAmbient);
        entity.AddAction("stopSound", StopSound);
    }

    private void PlaySoundAmbient(float sound)
    {
        int index = ((int)sound - 1);
        soundManager.PlaySoundAmbient(index);
    }

    private void StopSound()
    {
        // Baja el volumen a 0 en 2 segundos
        soundManager.StopSound();
    }

    public override void NormalMoving()
    {
        base.NormalMoving();
        Destroy(gameObject);
    }
}
