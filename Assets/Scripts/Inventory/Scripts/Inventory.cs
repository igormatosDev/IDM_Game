using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{

    public event EventHandler OnItemListChanged;

    private List<itemListSlot> itemList;
    private Action<Item> useItemAction;

    public class itemListSlot
    {
        public Item item;
        public int slot;
    }
    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<itemListSlot>();
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (itemListSlot itemListSlot in itemList)
            {
                Item inventoryItem = itemListSlot.item;
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(new itemListSlot { item=item, slot=-1});
            }
        }
        else
        {
            itemList.Add(new itemListSlot { item = item, slot = -1 });
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        //if (item.IsStackable())
        //{
        //    Item itemInInventory = null;
        //    foreach (Item inventoryItem in itemList)
        //    {
        //        if (inventoryItem.itemType == item.itemType)
        //        {
        //            inventoryItem.amount -= item.amount;
        //            itemInInventory = inventoryItem;
        //        }
        //    }
        //    if (itemInInventory != null && itemInInventory.amount <= 0)
        //    {
        //        itemList.Remove(itemInInventory);
        //    }
        //}
        //else
        //{
        //    itemList.Remove(item);
        //}
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public List<itemListSlot> GetItemList()
    {
        return itemList;
    }

}
