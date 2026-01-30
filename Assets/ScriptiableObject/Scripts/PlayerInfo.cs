using UnityEngine;

public enum PlayerSoundType
{
    Dash,
    Attack,
    Hurt,
    Death,
    Steps
}

[System.Serializable]
public class InfoSoundPlayer
{
    public AudioClip clip;
    public PlayerSoundType type;
}

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Scriptable Objects/PlayerInfo")]
public class PlayerInfo : ScriptableObject
{
    public int life = 12;

    [SerializeField] private InfoSoundPlayer[] sounds;
    [SerializeField] private AudioClip[] attackSounds;
    [SerializeField] private AudioClip[] hurtSounds;
    [SerializeField] private AudioClip[] deathSounds;
    [SerializeField] private AudioClip[] stepsSounds;

    public AudioClip GetSound(PlayerSoundType soundType)
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

    private AudioClip GetSoundVariant(PlayerSoundType soundType)
    {
        switch (soundType)
        {
            case PlayerSoundType.Attack:
                return GetRandomClip(attackSounds);
            case PlayerSoundType.Hurt:
                return GetRandomClip(hurtSounds);
            case PlayerSoundType.Death:
                return GetRandomClip(deathSounds);
            case PlayerSoundType.Steps:
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
