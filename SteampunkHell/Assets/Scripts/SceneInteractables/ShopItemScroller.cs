using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ShopItemScroller : MonoBehaviour
{
    public List<ShopItemIcon> allItemIcons = new List<ShopItemIcon>();
    public List<ShopItem> allItems = new List<ShopItem>();
    private ShopItem _selectedItem;
    private int _selectedItemID;

    public event Action<ShopItem> SelectionCallback;

    private bool _directionRight;

    private void Awake()
    {
        allItems.Add(new ShopItem().SetItemID(0).SetName("Pistol").SetPrice(50).SetRareness(Rareness.Common));
        allItems.Add(new ShopItem().SetItemID(1).SetName("Shotgun").SetPrice(100).SetRareness(Rareness.Common));
        allItems.Add(new ShopItem().SetItemID(2).SetName("Sniper").SetPrice(200).SetRareness(Rareness.Epic));
        allItems.Add(new ShopItem().SetItemID(3).SetName("Special Pistol").SetPrice(500).SetRareness(Rareness.Epic));
        allItems.Add(new ShopItem().SetItemID(4).SetName("Newgun").SetPrice(500).SetRareness(Rareness.Legendary));

        _selectedItem = allItems[0];

        if(GetComponentsInChildren<ShopItemIcon>() != null)
        {
            allItemIcons = GetComponentsInChildren<ShopItemIcon>().ToList();
        }
    }
    
    public void Buy(bool canBuy)
    {
        //TODO: Animacion de agrandarse el icono o algo asi.
    }

    public void Select(bool direction)
    {
        foreach (var item in allItemIcons)
        {
            item.Move(direction);
        }

        if (direction)
            SelectRight(direction);
        else
            SelectLeft(direction);

        _directionRight = direction;
        _selectedItem = allItems[_selectedItemID];

        Debug.Log("itemID: "+_selectedItem.itemID);
        SelectionCallback(_selectedItem);
    }

    private void SelectRight(bool dir)
    {
        if (_directionRight == dir)
            _selectedItemID++;
        else
            _selectedItemID += 2;
        if (_selectedItemID == allItems.Count) _selectedItemID = 0;
        else if (_selectedItemID == allItems.Count + 1) _selectedItemID = 1;
    }

    private void SelectLeft(bool dir)
    {
        if (_directionRight == dir)
            _selectedItemID--;
        else
            _selectedItemID -= 2;
        if (_selectedItemID == -1) _selectedItemID = allItems.Count - 1;
        else if (_selectedItemID == -2) _selectedItemID = allItems.Count - 2;
    }
}