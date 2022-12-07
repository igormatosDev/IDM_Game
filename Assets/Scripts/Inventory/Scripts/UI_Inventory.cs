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

    private void Awake()
    {
        //itemSlotContainer = transform.Find("itemSlotContainer");
        //itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
    }

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

    private void RefreshInventoryItems()
    {
        //foreach (Transform child in itemSlotContainer)
        //{
        //    if (child == itemSlotTemplate) continue;
        //    Destroy(child.gameObject);
        //}

        print(inventory.GetItemList());
        // Instantiating itemSlots and InventoryList
        GameObject[] itemSlots = GameObject.FindGameObjectsWithTag("ItemSlotContainer");
        List <itemListSlot> inventoryItemList = inventory.GetItemList();


        print(itemSlots);
        print(itemSlots.Length);
        foreach (itemListSlot itemSlot in inventoryItemList)
        {
            Item item = itemSlot.item;
            if (item == null) continue;

            if(itemSlot.slot == -1)
            {
                itemSlot.slot = GetFirstClearSlot(itemSlots);
            }
            
            if(itemSlot.slot == -1) continue; // inventory is Full, there is no slots left

            GameObject slotGameObject = itemSlots[itemSlot.slot];

            slotGameObject.GetComponent<Button_UI>().ClickFunc = () =>
            {
                // Use item
                inventory.UseItem(itemSlot);
            };

            slotGameObject.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                // Drop item
                Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
                inventory.RemoveItem(itemSlot);

                Vector2 dropDirection = player.GetLookDirection().normalized;
                ItemWorld itemWorld = ItemWorld.DropItem(player.transform.position, duplicateItem, dropDirection);

                itemWorld.isPickable = false;
                //StartCoroutine(itemWorld.pushItemAway(dropPosition, itemWorld));
                StartCoroutine(itemWorld.setPickableTrue(itemWorld, 5));

            };

            Image image = slotGameObject.transform.Find("itemImage").GetComponent<Image>();
            image.enabled = true;
            image.sprite = item.GetSprite();

            TextMeshProUGUI uiText = slotGameObject.transform.Find("itemAmount").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                uiText.SetText("");
            }
        }
    }

    private int GetFirstClearSlot(GameObject[] itemSlotContainers)
    {
        for(int i=0; i<itemSlotContainers.Length; i++)
        {
            Transform itemImage = itemSlotContainers[i].transform.GetChild(0);
            //Transform itemAmount = itemSlotContainers[i].transform.GetChild(1);
            if (!itemImage.GetComponent<Image>().enabled)
            {
                print($"SLOT: {i}");
                return i;
            }
        }

        return -1;
    }
}
