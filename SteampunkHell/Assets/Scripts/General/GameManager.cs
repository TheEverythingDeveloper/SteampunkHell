using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static bool paused;
    [Tooltip("Esto es para si queremos saltearnos todo lo de la camara, etc, y poner al jugador donde queramos")]
    public bool movilityTest;

    public bool canPause;
    public AudioSource music, ambient;
    private void Awake()
    {
        Instance = this;
        EventsManager.SubscribeToEvent(TypeOfEvent.NewWave, MusicWave);
        EventsManager.SubscribeToEvent(TypeOfEvent.FinishWave, AmbientFinishWave);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(DelayPause());
        }
    }

    IEnumerator DelayPause()
    {
        yield return new WaitForSeconds(0.1f);
        if (canPause)
            Paused();
    }

    void MusicWave(params object[] parameters)
    {
        StartCoroutine(ChangeWaveMusicTransition(ambient, music, 0.3f));
    }

    void AmbientFinishWave(params object[] parameters)
    {
        StartCoroutine(ChangeWaveMusicTransition(music, ambient, 0.3f));
    }

    IEnumerator ChangeWaveMusicTransition(AudioSource actualMusic, AudioSource newMusic, float transitionSpeed)
    {
        newMusic.volume = 0;
        newMusic.Play();
        while(actualMusic.volume > 0 || newMusic.volume < 0.9f)
        {
            actualMusic.volume -= Time.deltaTime * transitionSpeed;
            newMusic.volume += Time.deltaTime * transitionSpeed;
            actualMusic.volume = Mathf.Clamp01(actualMusic.volume);
            newMusic.volume = Mathf.Clamp01(newMusic.volume);
            yield return new WaitForEndOfFrame();
        }
        actualMusic.Stop();
    }

    private void Paused()
    {
        if (paused)
        {
            paused = false;
            Debug.Log("Unpaused.");
        }
        else
        {
            paused = true;
            Debug.Log("Game Paused.");
        }
    }
}
