using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    private void Awake()
    {
        Scene scene = SceneManager.GetSceneByName("SeedPlanten");
        if (!scene.isLoaded)
        {
            SceneManager.LoadScene("SeedPlanten", LoadSceneMode.Additive);
        }
    }
}
