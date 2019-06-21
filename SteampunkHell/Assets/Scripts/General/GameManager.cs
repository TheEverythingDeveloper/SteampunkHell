using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    public bool canPause;

    private void Awake()
    {
        Instance = this;
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
    public void CheckState()
    {

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
