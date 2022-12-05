using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    [SerializeField] InputActionReference pointerPosition;

    void Update()
    {
        if (Helpers.isPaused()) { return; };

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(pointerPosition.action.ReadValue<Vector2>());
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        transform.position = (Vector2)mousePos;
    }
}
