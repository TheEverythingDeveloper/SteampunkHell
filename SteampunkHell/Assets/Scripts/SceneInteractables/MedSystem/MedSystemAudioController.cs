using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedSystemAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSrc;
    [SerializeField] private AudioSource _mainAudioSrc;
    public AudioClip healAudio;
    public AudioClip startUsingAudio;
    public AudioClip stopUsingAudio;
    public AudioClip CanUseAudio;

    public void CanHealAudio(bool can)
    {
        //If(can) => reproducir sonido de prenderse la pc, else => reproducir sonido de apagarse.
    }

    public void Using(bool startUsing)
    {
        //If(startUsing) => reproducir tal sonido, else => parar de reproducirlo.
        if (startUsing)
        {
            StartCoroutine(StartHealing());
        }
        else
        {
            _audioSrc.Stop();
            _audioSrc.clip = stopUsingAudio;
            _audioSrc.Play();
        }
    }

    private IEnumerator StartHealing()
    {
        _audioSrc.Stop();
        _audioSrc.clip = startUsingAudio;
        _audioSrc.Play();
        yield return new WaitForSeconds(1f);
        _mainAudioSrc.Play();
    }
}
