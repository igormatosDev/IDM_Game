using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;
using static Inventory;
using System.Linq;
using System;

public class UI_Inventory : MonoBehaviour
{

    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private Button itemSlotButton;
    private PlayerController player;
    public GameObject[] itemSlots;
    public MouseCursorController mouseCursorController;

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        RefreshInventoryItems();
    }



    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }


    public void DragItem(ItemListSlot item)
    {
        GameObject itemSlotGameObject = inventory.GetSlotGameObject(item.slot);
        mouseCursorController.DragItem(item, itemSlotGameObject);
    }

    private void SetInventorySlotClickFunctions(ItemListSlot itemSlot)
    {
        GameObject slotGameObject = inventory.GetSlotGameObject(itemSlot.slot);

        slotGameObject.GetComponent<Button_UI>().ClickFunc = () =>
        {
            // Use item
            //inventory.UseItem(itemSlot);

            DragItem(itemSlot);
        };

        slotGameObject.GetComponent<Button_UI>().MouseRightClickFunc = () =>
        {
            // Drop item
            Item duplicateItem = new Item { itemType = itemSlot.item.itemType, amount = itemSlot.item.amount };
            inventory.RemoveItem(itemSlot);


            Vector2 dropDirection = player.GetLookDirection().normalized;
            ItemWorld itemWorld = ItemWorld.DropItem(player.transform.position, duplicateItem, dropDirection);

            itemWorld.isPickable = false;
            StartCoroutine(itemWorld.setPickableTrue(itemWorld, 5));

        };
    }

    private void RefreshInventoryItems()
    {
        List<ItemListSlot> inventoryItemList = inventory.GetItemList();
        foreach (ItemListSlot itemSlot in inventoryItemList)
        {
            // -1  : Nothing in inventory.
            // >=0 : Item in inventory at the specified slot.
            
            // Treating to avoid null values
            if (itemSlot.item == null) continue;
            if (itemSlot.slot == -1)
            {
                itemSlot.slot = inventory.GetFirstClearSlot();
            }
            if (itemSlot.slot == -1) continue; // if it's still -1: inventory is Full, there is no slots left

            print($"OK!!! SLOT: {itemSlot.slot}, ITEM: {itemSlot.item.itemType}");
            SetInventorySlotClickFunctions(itemSlot);
            inventory.SetItemSprite(itemSlot, false);
        };
    }
}


