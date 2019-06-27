using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    private AudioSource _audioSrc;
    public List<AudioClip> audios;

    public bool muteVolume;

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

    public void MakeSound(AudioClip newSound)
    {
        _audioSrc.clip = newSound;
        _audioSrc.Stop();
        _audioSrc.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            muteVolume = !muteVolume;
            AudioListener.pause = muteVolume;
            AudioListener.volume = muteVolume ? 0 : 1;
        }
    } 
}
