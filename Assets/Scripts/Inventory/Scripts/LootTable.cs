using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    private System.Random random = new System.Random();


    [Serializable]
    public class Drop
    {
        public Item.ItemType itemType;
        public int probability;
        public int minAmount = 1;
        public int maxAmount = 1;
    }
    
    public List<Drop> lootTable;

    public List<Item> getLoot()
    {
        List<Item> items = new List<Item>();
        
        for(int i = 0; i < lootTable.Count; i++)
        {
            Drop drop = lootTable[i];

            if (random.Next(1, 101) < drop.probability)
            {
                items.Add(new Item { 
                    itemType = drop.itemType, 
                    amount=random.Next(drop.minAmount, drop.maxAmount+1)
                });
            }
        }
        return items;
    }




}
