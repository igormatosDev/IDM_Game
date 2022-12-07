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
        inventoryMenuUI.anchoredPosition = new Vector2(0, 0);
        //inventoryMenuUI.transform.position = new Vector2(960, 540);
        inventoryMenuTab.SetActive(true);
    }

    public void DeactivateInventoryMenu()
    {
        inventoryMenuUI.anchoredPosition = new Vector2(0, -312);
        //inventoryMenuUI.transform.position = new Vector2(960, -83);
        inventoryMenuTab.SetActive(false);
    }


}
