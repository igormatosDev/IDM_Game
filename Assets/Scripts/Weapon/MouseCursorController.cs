using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCursorController : MonoBehaviour
{
    public Texture2D defaultCursorTexture;
    public Texture2D cursorOverInventory;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        SetCursor(defaultCursorTexture);
    }

    public void SetCursor(Texture2D cursor)
    {
        print(cursor);
        Vector2 hotspot = Vector2.zero;
        if(cursor == defaultCursorTexture || cursor == null)
        {
            cursor = defaultCursorTexture;
            hotspot = new Vector2(cursor.width / 2f, cursor.height / 2f);
        }
        Cursor.SetCursor(cursor, hotspot, CursorMode.Auto);
    }

    private void OnMouseEnter()
    {
        print("COLLISION ENTERED!");
        SetCursor(cursorOverInventory);
    }

    private void OnMouseDown()
    {
        print("COLLISION ENTERED!");
        SetCursor(cursorOverInventory);
    }
    private void OnMouseExit()
    {
        print("COLLISION EXITED!");
        SetCursor(defaultCursorTexture);
    }

}
