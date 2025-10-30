using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    public bool usingGamepad = false;
    void Update()
    {
        // Detectar si el último dispositivo usado fue un mando
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            if (!usingGamepad)
            {
                Debug.Log("Cambiado a Mando");
                usingGamepad = true;
            }
        }
        // Detectar si fue el teclado
        else if (Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame)
        {
            if (usingGamepad)
            {
                Debug.Log("Cambiado a Teclado");
                usingGamepad = false;
            }
        }
    }
}
