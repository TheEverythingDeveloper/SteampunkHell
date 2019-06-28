using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem //A.K.A BUILDER.
{
    public int itemID;
    public string name;
    public int price;
    public Rareness rareness;

    public ShopItem SetItemID(int IDparam)
    {
        itemID = IDparam;
        return this;
    }

    public ShopItem SetName(string nameParam)
    {
        name = nameParam;
        return this;
    }

    public ShopItem SetPrice(int priceParam)
    {
        price = priceParam;
        return this;
    }

    public ShopItem SetRareness(Rareness rarenessParam)
    {
        rareness = rarenessParam;
        return this;
    }
}

public enum Rareness
{
    Common = 0,
    Epic = 1,
    Legendary = 2
}
