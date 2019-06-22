using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase va a saber todo sobre el estado de lo que tendria que tener un shop. 
/// Basicamente si ya compraste un arma, en el otro shop no te va a aparecer para comprar.
/// Ademas, va a tener cosas como desbloqueos de armas, etc, que sirven para todos los shops y no uno en especifico.
/// </summary>
public class ShopsManager : MonoBehaviour
{
    public List<ShopItem> shopItems = new List<ShopItem>();

    private void Awake()
    {
        //TODO: Hacer todo esto directamente unido con Localization Manager
        /*shopItems.Add(new ShopItem { itemID = 0, name = "Grenade", price = 50 });
        shopItems.Add(new ShopItem { itemID = 1, name = "Pistol", price = 150 });
        shopItems.Add(new ShopItem { itemID = 2, name = "Shotgun", price = 300 });
        shopItems.Add(new ShopItem { itemID = 3, name = "SpecialPistol", price = 500 });*/
    }
    //TODO: Aca van funciones para saber la cantidad de armas en venta, etc.
}
