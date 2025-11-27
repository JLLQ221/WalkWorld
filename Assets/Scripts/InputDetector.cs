using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    public bool usingGamepad = false;
    private Gamepad gamepad;

    void Update()
    {
        // Detectar si el último dispositivo usado fue un mando
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
        {
            gamepad = Gamepad.current;
            usingGamepad = true;
        }
        else if (Keyboard.current != null && Keyboard.current.wasUpdatedThisFrame)
        {
            usingGamepad = false;
            gamepad = null;
        }
    }

    public void VibrationController(float intensity, float time)
    {
        if (gamepad != null)
        {
            // Activa vibración: lowFrequency, highFrequency
            gamepad.SetMotorSpeeds(intensity, intensity);

            // Detiene la vibración después de 0.5 segundos
            Invoke(nameof(StopVibration), time);
        }
    }
    public void StopVibration()
    {
        gamepad.SetMotorSpeeds(0f, 0f);
    }
}
