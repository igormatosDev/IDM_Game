using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    [SerializeField] InputActionReference pointerPosition;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        if (Helpers.isPaused()) { return; };

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(pointerPosition.action.ReadValue<Vector2>());
        transform.position = (Vector2)mousePos;
    }
}
