using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    private static WaveManager _Instance;

    public TextMeshProUGUI textStage;

    public int initialEnemiesStage;
    int _actualStage;

    int _actualEnemiesActive;

    public List<Action> allStages = new List<Action>();
    public int[] stages;

    bool waveActive;

    void Awake()
    {
        _Instance = this;
    }
    void Start()
    {
        EventsManager.SubscribeToEvent(TypeOfEvent.EnemyDead, CheckEnemiesState);
        AddStages();
        textStage.text = "Press enter to start stage " + (_actualStage + 1);
    }

    void AddStages()
    {
        allStages.Add(Stage1);
        allStages.Add(Stage2);
        allStages.Add(Stage3);
        allStages.Add(Stage4);
        allStages.Add(Stage5);
        allStages.Add(Stage6);
    }

    public void NewStage()
    {
        if (waveActive)
            return;

        EventsManager.TriggerEvent(TypeOfEvent.NewWave);
        waveActive = true;
        _actualStage++;
        initialEnemiesStage++;
        textStage.text = "Stage " + _actualStage;
        allStages[CheckStage()]();
        Debug.Log("Oleada " + _actualStage + ". Con " + _actualEnemiesActive + " enemigos");
        GateSystem.Instance.ActivateStage();
    }
    int CheckStage()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if(_actualStage < stages[i])
            {
                return i;
            }
        }
            return 0;
    }
    public void CheckEnemiesState(params object[] parameters)
    {
        _actualEnemiesActive--;
        Debug.Log("Faltan matar: " + _actualEnemiesActive + " enemigos");
        if (_actualEnemiesActive <= 0)
        {
            EventsManager.TriggerEvent(TypeOfEvent.FinishWave);
            textStage.text = "Press enter to start stage " + (_actualStage + 1);
            waveActive = false;
        }
    }

    public void AddToEnemiesActive() { _actualEnemiesActive++; } //Simplemente agregar uno sin entrar a la variable privada

    void Stage1()
    {
        for (int i = 0; i < initialEnemiesStage; i++)
        {
            var numberDoor = Random.Range(0, GateSystem.Instance.doors.Count - 1);
            EnemySpawner.Instance.GetEnemySniper(GateSystem.Instance.doors[numberDoor].spawnEnemy());
        }
    }
    void Stage2()
    {
        for (int i = 0; i < initialEnemiesStage; i++)
        {
            var numberDoor = Random.Range(0, GateSystem.Instance.doors.Count - 1);
            var selectEnemy = Random.Range(1, 4);

            if (selectEnemy == 1)
                EnemySpawner.Instance.GetEnemyExplosive(GateSystem.Instance.doors[numberDoor].spawnEnemy());
            else
                EnemySpawner.Instance.GetEnemySniper(GateSystem.Instance.doors[numberDoor].spawnEnemy());
        }

        Debug.Log("bla");
        EnemySpawner.Instance.GetEnemyZeppellin(
            GateSystem.Instance.zepellinSpawnPoints[Random.Range(0, GateSystem.Instance.zepellinSpawnPoints.Count)]);
    }
    void Stage3()
    {
        for (int i = 0; i < initialEnemiesStage; i++)
        {
            var numberDoor = Random.Range(0, GateSystem.Instance.doors.Count - 1);
            var selectEnemy = Random.Range(1, 2);

            if (selectEnemy == 1)
                EnemySpawner.Instance.GetEnemyExplosive(GateSystem.Instance.doors[numberDoor].spawnEnemy());
            else
                EnemySpawner.Instance.GetEnemySniper(GateSystem.Instance.doors[numberDoor].spawnEnemy());
        }


    }
    void Stage4()
    {

    }
    void Stage5()
    {

    }
    void Stage6()
    {

    }
}
