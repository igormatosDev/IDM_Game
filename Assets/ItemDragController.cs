using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemDragController : MonoBehaviour
{
    [SerializeField] private InputActionReference pointerPositionForDragging;
    
    private void Update()
    {
        Vector3 mousePosition = pointerPositionForDragging.action.ReadValue<Vector2>();
        Vector3 pos = Camera.main.ScreenToWorldPoint(mousePosition);
        pos.z = 650;
        transform.position = pos;
    }
}
