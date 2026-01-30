using UnityEngine;

public enum PersistentType { Player, Canvas, Object, GameManager, PastObject, Camera, AudioManager, AudioManager2, WordSpace }

public class DontDestroy : MonoBehaviour
{
    public PersistentType type;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        DontDestroy[] objs = FindObjectsByType<DontDestroy>(FindObjectsSortMode.None);

        int coutType = 0;
        foreach (var obj in objs)
        {
            if (obj.type == this.type)
            {
                coutType++;
            }
        }

        if (coutType <= 1) return;

        foreach (var obj in objs)
        {
            if (obj.type == this.type && obj == this)
            {
                // En vez de destruir al otro, destruye este duplicado
                Destroy(gameObject);
                return;
            }
        }
    }
}
