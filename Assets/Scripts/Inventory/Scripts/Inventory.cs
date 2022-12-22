using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory
{

    public event EventHandler OnItemListChanged;

    private List<ItemListSlot> itemListSlot;
    private Action<ItemListSlot> useItemAction;
    private GameObject[] itemSlotsContainerGameObject;

    public class ItemListSlot
    {
        public Item item;
        public int slot;
    }
    public Inventory(Action<ItemListSlot> useItemAction, GameObject[] itemSlotsContainerGameObject)
    {
        this.useItemAction = useItemAction;
        this.itemSlotsContainerGameObject = itemSlotsContainerGameObject;
        itemListSlot = new List<ItemListSlot>();
    }
    public void UseItem(ItemListSlot item)
    {
        useItemAction(item);
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (ItemListSlot ItemListSlot in itemListSlot)
            {
                Item inventoryItem = ItemListSlot.item;
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemListSlot.Add(new ItemListSlot { item=item, slot=-1});
            }
        }
        else
        {
            itemListSlot.Add(new ItemListSlot { item = item, slot = -1 });
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(ItemListSlot itemSlot)
    {
        Item item = itemSlot.item;
        if (item == null)
        {
            return;
        }
        bool completelyRemove = false;
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (ItemListSlot inventoryItemSlot in itemListSlot)
            {
                Item inventoryItem = inventoryItemSlot.item;
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                completelyRemove = true;
            }
        }
        else
        {
            completelyRemove = true;
        }

        if (completelyRemove)
        {
            SetItemSprite(itemSlot, true);
            itemListSlot.Remove(itemSlot);
            itemSlot.item = null;
            itemSlot.slot = -1;
        }
        
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetItemSprite(ItemListSlot itemSlot, bool clear=false)
    {
        // if clear == true -> Resets the GameObject to it's original state
        // if clear == false -> Sets the Image and UITEXT inside the GameObject (itemSlotContainer in UI)
        GameObject slotGameObject = GetSlotGameObject(itemSlot.slot);
        Image image = slotGameObject.transform.Find("itemImage").GetComponent<Image>();
        TextMeshProUGUI uiText = slotGameObject.transform.Find("itemAmount").GetComponent<TextMeshProUGUI>();

        if (clear)
        {
            image.sprite = null;
            image.enabled = false;
            uiText.SetText("");
        }
        else
        {
            image.sprite = itemSlot.item.GetSprite();
            image.enabled = true;
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


    public List<ItemListSlot> GetItemList()
    {
        return itemListSlot;
    }

    public GameObject GetSlotGameObject(int slot)
    {
        return this.itemSlotsContainerGameObject[slot];
    }

    public int GetFirstClearSlot()
    {
        for (int i = 0; i < this.itemSlotsContainerGameObject.Length; i++)
        {
            Transform itemImage = this.itemSlotsContainerGameObject[i].transform.GetChild(0);
            //Transform itemAmount = itemSlotContainers[i].transform.GetChild(1);
            if (!itemImage.GetComponent<Image>().enabled)
            {
                return i;
            }
        }

        return -1;
    }
}

