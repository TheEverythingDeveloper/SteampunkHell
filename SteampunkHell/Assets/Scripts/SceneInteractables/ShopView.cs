using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopView : MonoBehaviour
{
    private ShopItemScroller _itemScroller;
    private ShopItem _actualShopItem;


    public GameObject allCanvas;

    public GameObject background;
    public Color[] backgroundAColors = new Color[3]; //Color base del fondo del shop machine
    public Color[] backgroundBColors = new Color[3]; //Color Extra del fondo del shop machine
    public float timeTransitionColors; //tiempo que cambia de color

    public TextMeshProUGUI priceText;
    public TextMeshProUGUI nameText;

    private void Awake()
    {
        _itemScroller = GetComponentInChildren<ShopItemScroller>();
    }

    public void CanShop(bool can)
    {
        allCanvas.SetActive(can);
    }

    public void Using(bool startUsing)
    {
        Debug.Log(startUsing ? "se empezo a usar" : "se termino de usar");
        //TODO: Shader que pasa de ser una textura que no se ve que es, a algo que se ve con detalle (al reves en caso de false)
    }

    public void Buy(bool canBuy)
    {
        Debug.Log(canBuy ? "compraste un item" : "no pudiste comprarlo, te falta plata");
        //en caso de que canBuy = true;
        //TODO: Animacion, particulas, etc, de haber comprado.
        //en caso de que canBuy = false;
        //TODO: Feedback en pantalla cuando tratamos pero no podemos. Se pone rojo o algo asi.
    }

    public void Select(ShopItem selectedShopItem)
    {
        if (_actualShopItem == selectedShopItem) return;
        _actualShopItem = selectedShopItem;

        int rarenessID = (int)selectedShopItem.rareness;
        StartCoroutine(ChangeBackgroundColor(backgroundAColors[rarenessID], backgroundBColors[rarenessID]));

        nameText.text = selectedShopItem.name;
        priceText.text = "$1000 / $" + selectedShopItem.price;
    }

    IEnumerator ChangeBackgroundColor(Color colorA, Color colorB)
    {
        float x = timeTransitionColors;
        Material backgroundMat = background.GetComponent<Image>().material;
        Color oldColorA = backgroundMat.GetColor("_ColorA");
        Color oldColorB = backgroundMat.GetColor("_ColorB");
        while(x > 0)
        {
            x -= Time.deltaTime;
            Color newColorA = Color.Lerp(colorA, oldColorA, x/timeTransitionColors);
            Color newColorB = Color.Lerp(colorB, oldColorB, x/timeTransitionColors);
            backgroundMat.SetColor("_ColorA", newColorA);
            backgroundMat.SetColor("_ColorB", newColorB);
            yield return new WaitForEndOfFrame();
        }
    }

    public void ArtificialUpdate()
    {

    }
}
