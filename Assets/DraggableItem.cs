using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform parentAfterDrag;
    [SerializeField] private InputActionReference pointerPositionForDragging;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

        Vector3 mousePosition = pointerPositionForDragging.action.ReadValue<Vector2>();
        Vector3 pos = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        transform.SetParent(parentAfterDrag);
    }
}
