using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject objectMove;
    private Vector2 movePosition;
    private Player player;
    private GameObject uiPlayer;
    private GameObject cineticPlayer;
    public Scene1Plants objScript;


    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        uiPlayer = GameObject.Find("CanvaUI").transform.Find("UIPlayer").gameObject;
        cineticPlayer = GameObject.Find("CanvaUI").transform.Find("UICinematic").gameObject;
        player = FindAnyObjectByType<Player>();
    }

    public void SetObjectMove(Vector2 position, GameObject obj)
    {
        movePosition = position;
        objectMove = obj;
    }

    public void ExitCinematic()
    {
        uiPlayer.SetActive(true);
        cineticPlayer.SetActive(false);
        player.SetFreeMoving(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!objScript.changeScene && scene.buildIndex != 0)
        {
            objScript.changeScene = true;
        }
        if (objectMove != null)
        {
            objectMove.transform.position = movePosition;
        }
        movePosition = Vector2.zero;
        objectMove = null;
    }
}
