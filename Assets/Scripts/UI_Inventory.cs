using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlot;
    private Transform item;
    public int index = 0;

    private void Start()
    {
        itemSlot = transform.Find("itemSlot");
        item = itemSlot.Find("itemSprite");
    }
    private void Update()
    {
        RefreshInventoryItems();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            index++;
        }
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    public void RefreshInventoryItems()
    {
        float itemSize = 0f;
        int i = index % inventory.GetItemList().Count;
        RectTransform itemRectTransform = item.GetComponent<RectTransform>();
        itemRectTransform.gameObject.SetActive(true);
        itemRectTransform.anchoredPosition = new Vector2(itemSize, itemSize);
        Image image = item.GetComponent<Image>();
        image.sprite = inventory.GetItemList()[i].GetSprite();
        Text amount = itemSlot.Find("amount").GetComponent<Text>();
        amount.text = "" + inventory.GetItemList()[i].amount;
    }
}
