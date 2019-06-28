using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MedSystemView : MonoBehaviour
{
    public GameObject allCanvas;

    public GameObject background;
    public Color[] backgroundAColors = new Color[3]; //Color base del fondo del shop machine
    public Color[] backgroundBColors = new Color[3]; //Color Extra del fondo del shop machine
    public float timeTransitionColors; //tiempo que cambia de color

    public TextMeshProUGUI priceText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI enterToShopText;

    public void CanHeal(bool can)
    {
        allCanvas.SetActive(can);
        enterToShopText.gameObject.SetActive(can);
    }

    public void Using(bool startUsing)
    {
        enterToShopText.gameObject.SetActive(!startUsing);
        Debug.Log(startUsing ? "se empezo a usar" : "se termino de usar");
        //TODO: Shader que pasa de ser una textura que no se ve que es, a algo que se ve con detalle (al reves en caso de false)
    }

    public void ArtificialUpdate()
    {

    }
}
