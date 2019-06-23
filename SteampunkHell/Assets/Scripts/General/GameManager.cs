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
    public AudioSource music, ambient, openDoor;
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
        ambient.Stop();
        music.Play();
    }

    void AmbientFinishWave(params object[] parameters)
    {
        ambient.Play();
        music.Pause();
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
