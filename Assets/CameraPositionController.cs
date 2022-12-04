using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPositionController : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    [SerializeField] private float threshold;
    [SerializeField] private Camera cam;
    [SerializeField] InputActionReference pointerPosition;



    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(pointerPosition.action.ReadValue<Vector2>());
        Vector3 targetPos = (playerPos.position + mousePos) / 2f;
        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + playerPos.position.x, threshold + playerPos.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + playerPos.position.y, threshold + playerPos.position.y);
        transform.position = targetPos;
    }
}
