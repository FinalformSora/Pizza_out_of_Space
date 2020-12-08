using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item
{
    public enum ItemType
    {
        Attract,
        Repel,
        Battery,
        Trap,
        Pizza,
        Pills
    }

    public ItemType itemType;
    public int amount;
    public int cost;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Attract: return ItemAssets.Instance.AttractSprite;
            case ItemType.Repel: return ItemAssets.Instance.RepelSprite;
            case ItemType.Battery: return ItemAssets.Instance.BatterySprite;
            case ItemType.Trap: return ItemAssets.Instance.TrapSprite;
            case ItemType.Pizza: return ItemAssets.Instance.PizzaSprite;
            case ItemType.Pills: return ItemAssets.Instance.PillsSprite;
        }
    }
}
