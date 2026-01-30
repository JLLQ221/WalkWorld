using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MoveObject : MonoBehaviour
{
    private Camera mainCamera;   // Cámara que usaremos para convertir coordenadas
    public float distance = 10f; // Distancia desde la cámara donde se proyectará el mouse
    private InputDetector detector;
    private Image imgChildren;
    private InputAction action;
    private SeedUp objectUp;
    private bool interactEnable = false;

    private void Awake()
    {
        imgChildren = GetComponentInChildren<Image>();
        mainCamera = FindAnyObjectByType<Camera>();
        detector = FindAnyObjectByType<InputDetector>();
        action = InputSystem.actions.FindAction("Interact");
        objectUp = FindAnyObjectByType<SeedUp>();
    }

    public void DestroyAll()
    {
        Destroy(imgChildren);
        Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(ListenerInteract());
    }

    void Update()
    {
        bool usingGamepad = detector.GetUsingGamepad();
        if (Mouse.current.leftButton.isPressed && !usingGamepad)
        {
            // Posición del mouse en pantalla
            Vector2 mousePos = Mouse.current.position.ReadValue();

            // Convertir a coordenadas del mundo (en 2D basta con X,Y)
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));

            // Mantener Z en 0 para 2D
            worldPos.z = 0;

            // Opcional: detectar colisión con otros colliders 2D
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                // Mover el objeto
                transform.position = worldPos;
            }
        }
        else
        {
            if (action.IsPressed() && interactEnable)
            {
                objectUp.Activate();
                Destroy(gameObject);
            }
        }
    }

    IEnumerator ListenerInteract()
    {
        float time = 0;
        float endTime = 1f;
        while (time < endTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        interactEnable = true;
    }
}
