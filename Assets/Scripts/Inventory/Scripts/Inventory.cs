using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{

    public event EventHandler OnItemListChanged;

    private List<ItemListSlot> itemListSlot;
    private Action<ItemListSlot> useItemAction;

    public class ItemListSlot
    {
        public Item item;
        public int slot;
    }
    public Inventory(Action<ItemListSlot> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemListSlot = new List<ItemListSlot>();
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
                itemListSlot.Remove(itemSlot);
            }
        }
        else
        {
            itemListSlot.Remove(itemSlot);
        }
        itemSlot.slot = -2;

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(ItemListSlot item)
    {
        useItemAction(item);
    }

    public List<ItemListSlot> GetItemList()
    {
        return itemListSlot;
    }

}
