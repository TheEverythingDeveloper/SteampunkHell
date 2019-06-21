using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

using Random = UnityEngine.Random;

public class StatesManager : MonoBehaviour
{
    public TextMeshProUGUI textState;

    public int initialEnemiesState;
    int _actualStage;

    int _actualEnemiesActive;

    public List<Action> allStages = new List<Action>();
    public int[] stages;

    bool waveActive;
    void Start()
    {
        AddStages();
        textState.text = "Press enter to start stage " + (_actualStage + 1);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter) && !waveActive)
            NewState();
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

    public void NewState()
    {
        waveActive = true;
        _actualStage++;
        initialEnemiesState++;
        textState.text = "Stage " + _actualStage;
        GateSystem.Instance.doorsActive = new List<GameObject>();
        allStages[CheckStage()]();
        Debug.Log("Oleada " + _actualStage + ". Con " + _actualEnemiesActive + " enemigos");
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
    public void CheckEnemiesState()
    {
        _actualEnemiesActive--;
        Debug.Log("Faltan matar: " + _actualEnemiesActive + " enemigos");
        if (_actualEnemiesActive <= 0)
        {
            textState.text = "Press enter to start stage " + (_actualStage + 1);
            waveActive = false;
        }
    }
    void Stage1()
    {
        for (int i = 0; i < initialEnemiesState; i++)
        {
            var numberDoor = Random.Range(0, GateSystem.Instance.doors.Count - 1);
            GateSystem.Instance.doorsActive.Add(GateSystem.Instance.doors[numberDoor].gameObject);
            EnemySpawner.Instance.GetEnemySniper(GateSystem.Instance.doors[numberDoor]);
            _actualEnemiesActive++;
        }
    }
    void Stage2()
    {
        for (int i = 0; i < initialEnemiesState; i++)
        {
            var numberDoor = Random.Range(0, GateSystem.Instance.doors.Count - 1);
            GateSystem.Instance.doorsActive.Add(GateSystem.Instance.doors[numberDoor].gameObject);
            var selectEnemy = Random.Range(1, 4);

            if (selectEnemy == 1)
                EnemySpawner.Instance.GetEnemyExplosive(GateSystem.Instance.doors[numberDoor]);
            else
                EnemySpawner.Instance.GetEnemySniper(GateSystem.Instance.doors[numberDoor]);

            _actualEnemiesActive++;
        }
    }
    void Stage3()
    {
        for (int i = 0; i < initialEnemiesState; i++)
        {
            var numberDoor = Random.Range(0, GateSystem.Instance.doors.Count - 1);
            GateSystem.Instance.doorsActive.Add(GateSystem.Instance.doors[numberDoor].gameObject);
            var selectEnemy = Random.Range(1, 2);

            if (selectEnemy == 1)
                EnemySpawner.Instance.GetEnemyExplosive(GateSystem.Instance.doors[numberDoor]);
            else
                EnemySpawner.Instance.GetEnemySniper(GateSystem.Instance.doors[numberDoor]);

            _actualEnemiesActive++;
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
