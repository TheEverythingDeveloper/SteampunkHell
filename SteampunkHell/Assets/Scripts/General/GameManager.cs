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

    public TextMeshProUGUI textState;
    public List<GameObject> enemiesActive;
    public int initialEnemiesState;
    public int state;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < enemiesActive.Count; i++)
        {
            enemiesActive[i].SetActive(false);
        }
        NewState();
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused();
        }
    }
    public void CheckState()
    {

    }
    public void NewState()
    {
        state++;
        initialEnemiesState++;
        textState.text = "State " + state;
        for (int i = 0; i < initialEnemiesState; i++)
        {
            enemiesActive[i].SetActive(true);
        }
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
