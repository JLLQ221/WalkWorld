using UnityEngine;

public enum EnemySoundType
{
    Attack,
    Hurt,
    Death,
    Step
}


[System.Serializable]
public class InfoSoundEnemy
{
    public AudioClip clip;
    public EnemySoundType type;
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]

public class EnemyInfo : ScriptableObject
{
    [SerializeField] public int life;
    [SerializeField] public float speed;
    [SerializeField] private InfoSoundEnemy[] sounds;

    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip[] hurtSounds;
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField] private AudioClip[] stepsSounds;

    public AudioClip GetSound(EnemySoundType soundType)
    {
        // Primero intenta obtener una variación
        AudioClip variant = GetSoundVariant(soundType);
        if (variant != null)
        {
            return variant;
        }

        // Si no hay variaciones, usa el clip único del array "sounds"
        foreach (var sound in sounds)
        {
            if (sound.type == soundType)
            {
                return sound.clip;
            }
        }

        Debug.LogWarning($"Sound {soundType} not found in EnemyInfo.");
        return null;
    }


    private AudioClip GetSoundVariant(EnemySoundType soundType)
    {
        switch (soundType)
        {
            case EnemySoundType.Attack:
                return GetRandomClip(attackSounds);
            case EnemySoundType.Hurt:
                return GetRandomClip(hurtSounds);
            case EnemySoundType.Death:
                return GetRandomClip(deathSounds);
            case EnemySoundType.Step:
                return GetRandomClip(stepsSounds);
            default:
                return null;
        }
    }
    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
