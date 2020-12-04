using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.Repel, amount = 0, cost = 10 });
        AddItem(new Item { itemType = Item.ItemType.Attract, amount = 0, cost = 10 });
        AddItem(new Item { itemType = Item.ItemType.Trap, amount = 0, cost = 20 });
        AddItem(new Item { itemType = Item.ItemType.Battery, amount = 0, cost = 5 });
    }

    public void AddItem(Item item)
    {
        bool alreadyCollected = false;
        foreach(Item x in itemList)
        {
            if (x.itemType == item.itemType)
            {
                x.amount++;
                alreadyCollected = true;
            }
        }
        if (!alreadyCollected)
        {
            itemList.Add(item);
        }
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
