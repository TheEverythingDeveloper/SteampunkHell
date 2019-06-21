using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    public GameObject allCanvas;

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

    public void Select(int ID)
    {
        Debug.Log("seleccionaste el item " + ID);
        //TODO: Cambiar de seleccion visualmente. Aca va a haber una logica interna para esto agarrando los items cercanos.
    }

    public void ArtificialUpdate()
    {

    }
}
