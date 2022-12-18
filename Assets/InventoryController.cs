using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour
{
    private NewControls newControls;
    private InputAction menu;

    [SerializeField] private RectTransform inventoryMenuUI;
    [SerializeField] private GameObject inventoryMenuTab;
    [SerializeField] private bool isOpened;
    [SerializeField] private MouseCursorController mouseCursorController;

    void Awake()
    {
        // INVENTORY IN BOTTOM OF SCREEN (ANCHORED) -312PX Y
        newControls = new NewControls();
    }

    private void OnEnable()
    {
        try
        {
            menu = newControls.Menus.Inventory;
        }
        catch
        {
            Awake();
            menu = newControls.Menus.Inventory;
        }
        menu.Enable();
        menu.performed += Inventory;
    }
    private void OnDisable()
    {
        menu.Disable();
    }

    void Inventory(InputAction.CallbackContext context)
    {
        if (Helpers.isPaused()) { return; };

        isOpened = !isOpened;

        if (isOpened)
        {
            ActivateInventoryMenu();
        }
        else
        {
            DeactivateInventoryMenu();

        }
    }

    void ActivateInventoryMenu()
    {
        mouseCursorController.SetCursor(mouseCursorController.cursorOverInventory);
        inventoryMenuUI.anchoredPosition = new Vector2(0, 0);
        inventoryMenuTab.SetActive(true);
    }

    public void DeactivateInventoryMenu()
    {
        print("DEACTIVATING INV");
        mouseCursorController.SetCursor(mouseCursorController.defaultCursorTexture);
        inventoryMenuUI.anchoredPosition = new Vector2(0, -312);
        inventoryMenuTab.SetActive(false);
    }


}
