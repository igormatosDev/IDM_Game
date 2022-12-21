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

    private void Awake()
    {
        itemSlots = GameObject.FindGameObjectsWithTag("ItemSlotContainer");
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

    private void SetInventorySlotClickFunctions(GameObject slotGameObject, ItemListSlot itemSlot)
    {

        slotGameObject.GetComponent<Button_UI>().ClickFunc = () =>
        {
            // Use item
            inventory.UseItem(itemSlot);
        };

        slotGameObject.GetComponent<Button_UI>().MouseRightClickFunc = () =>
        {
            // Drop item
            Item duplicateItem = new Item { itemType = itemSlot.item.itemType, amount = itemSlot.item.amount };
            inventory.RemoveItem(itemSlot);


            Vector2 dropDirection = player.GetLookDirection().normalized;
            ItemWorld itemWorld = ItemWorld.DropItem(player.transform.position, duplicateItem, dropDirection);

            itemWorld.isPickable = false;
            //StartCoroutine(itemWorld.pushItemAway(dropPosition, itemWorld));
            StartCoroutine(itemWorld.setPickableTrue(itemWorld, 5));

        };
    }

    private void RefreshInventoryItems()
    {
        //foreach (Transform child in itemSlotContainer)
        //{
        //    if (child == itemSlotTemplate) continue;
        //    Destroy(child.gameObject);
        //}

        // Instantiating itemSlots and InventoryList
        
        List <ItemListSlot> inventoryItemList = inventory.GetItemList();


        //print(itemSlots);
        //print(itemSlots.Length);
        foreach (ItemListSlot itemSlot in inventoryItemList)
        {
            // -1  : Nothing in inventory.
            // -2  : There was something there, but should be removed and set to -1.
            // >=0 : Item in inventory at the specified slot.

            if (itemSlot.item == null) continue;
            if(itemSlot.slot == -1)
            {
                itemSlot.slot = GetFirstClearSlot(itemSlots);
            }
            
            if(itemSlot.slot == -1) continue; // if it's still 1: inventory is Full, there is no slots left


            GameObject slotGameObject = itemSlots[itemSlot.slot];
            SetInventorySlotClickFunctions(slotGameObject, itemSlot);


            Image image = slotGameObject.transform.Find("itemImage").GetComponent<Image>();
            TextMeshProUGUI uiText = slotGameObject.transform.Find("itemAmount").GetComponent<TextMeshProUGUI>();

            if(itemSlot.slot == -2)
            {
                itemSlot.slot = -1;
                itemSlot.item = null;
                image.sprite = null;
                image.enabled = false;
                uiText.SetText("");
                
            }
            else
            {
                image.enabled = true;
                image.sprite = itemSlot.item.GetSprite();

                if (itemSlot.item.amount > 1)
                {
                    uiText.SetText(itemSlot.item.amount.ToString());
                }
                else
                {
                    uiText.SetText("");
                }
            }
        }

        //print("ITEMSLOTS");
        //print(itemSlots);
        
        //print("-------");

        //print("INVENTORY");
        //print(inventory);
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

