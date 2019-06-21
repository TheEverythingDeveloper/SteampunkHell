using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAudioController : MonoBehaviour
{
    //TODO: Funciones de audio de todo tipo. Por ejemplo, comprar armas hace un ruido, mover para la derecha o izq, etc.
    //esto se tiene que conectar obviamente con los eventos de shopmachine.

    public void CanShopAudio(bool can)
    {
        //If(can) => reproducir sonido de prenderse la pc, else => reproducir sonido de apagarse.
    }

    public void Using(bool startUsing)
    {
        //If(startUsing) => reproducir tal sonido, else => parar de reproducirlo.
    }

    public void Buy(bool canBuy)
    {
        //en caso de que canBuy = true;
        //TODO: Reproducir sonido de festejo, comprado, monedas gastandose, etc.
        //en caso de que canBuy = false;
        //TODO: Reproducir sonido de error.
    }

    public void Select(int ID)
    {
        //TODO: Reproducir sonido de seleccion. En caso de que nos hayamos pasado del ID y que no hayan mas, reproducir error.
    }
}
