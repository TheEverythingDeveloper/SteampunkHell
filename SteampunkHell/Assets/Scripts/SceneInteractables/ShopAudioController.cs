using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAudioController : MonoBehaviour
{
    private AudioSource _audioSrc;
    public AudioClip buyAudio;
    public AudioClip newWeaponAudio;
    public AudioClip failAudio;
    public AudioClip selectAudio;
    public AudioClip startUsingAudio;
    public AudioClip stopUsingAudio;
    public AudioClip CanUseAudio;

    private void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
    }
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
        StartCoroutine(TryBought(canBuy));
    }

    IEnumerator TryBought(bool canBuy)
    {
        _audioSrc.Stop();
        yield return new WaitForSeconds(0.1f);
        if (canBuy)
        {
            _audioSrc.clip = buyAudio;
            _audioSrc.Play();
            yield return new WaitForSeconds(0.4f);
            _audioSrc.clip = newWeaponAudio;
            _audioSrc.Play();
        }
        else
        {
            _audioSrc.clip = failAudio;
            _audioSrc.Play();
        }
    }

    public void Select(bool direction)
    {
        _audioSrc.clip = selectAudio;
        _audioSrc.Play();
        //TODO: En caso de que nos hayamos pasado del ID y que no hayan mas, reproducir error.
    }
}
