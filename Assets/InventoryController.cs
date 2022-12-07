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
    private GameObject aim;

    void Awake()
    {
        // INVENTORY IN BOTTOM OF SCREEN (ANCHORED) -312PX Y
        newControls = new NewControls();
        aim = GameObject.Find("Aim");
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        aim.SetActive(false);

        inventoryMenuUI.anchoredPosition = new Vector2(0, 0);
        inventoryMenuTab.SetActive(true);
    }

    public void DeactivateInventoryMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        aim.SetActive(true);
        inventoryMenuUI.anchoredPosition = new Vector2(0, -312);
        inventoryMenuTab.SetActive(false);
    }


}
