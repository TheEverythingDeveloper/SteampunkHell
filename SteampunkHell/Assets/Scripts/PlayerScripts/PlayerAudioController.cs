using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    private AudioSource _audioSrc;
    public List<AudioClip> audios;

    public void Awake()
    {
        _audioSrc = GetComponent<AudioSource>();
    }
    public void MakeSound(int id)
    {
        if (id >= audios.Count) return;
        _audioSrc.clip = audios[id];
        _audioSrc.Stop();
        _audioSrc.Play();
    }
}
