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
    int _state;

    int _actualEnemiesActive;

    public List<Transform> doors;
    public List<Action> allStages = new List<Action>();
    public int[] stages;
    void Start()
    {
        AddStages();
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
        _state++;
        initialEnemiesState++;
        textState.text = "State " + _state;
        allStages[CheckStage()]();
        Debug.Log("Oleada " + _state + ". Con " + _actualEnemiesActive + " enemigos");
    }
    int CheckStage()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if(_state < stages[i])
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
            NewState();
    }
    void Stage1()
    {
        for (int i = 0; i < initialEnemiesState; i++)
        {
            var numberDoor = Random.Range(0, doors.Count - 1);
            EnemySpawner.Instance.GetEnemySniper(doors[numberDoor]);
            _actualEnemiesActive++;
        }
    }
    void Stage2()
    {
        for (int i = 0; i < initialEnemiesState; i++)
        {
            var numberDoor = Random.Range(0, doors.Count - 1);
            var selectEnemy = Random.Range(1, 4);

            if (selectEnemy == 1)
                EnemySpawner.Instance.GetEnemyExplosive(doors[numberDoor]);
            else
                EnemySpawner.Instance.GetEnemySniper(doors[numberDoor]);

            _actualEnemiesActive++;
        }
    }
    void Stage3()
    {
        for (int i = 0; i < initialEnemiesState; i++)
        {
            var numberDoor = Random.Range(0, doors.Count - 1);
            var selectEnemy = Random.Range(1, 2);

            if (selectEnemy == 1)
                EnemySpawner.Instance.GetEnemyExplosive(doors[numberDoor]);
            else
                EnemySpawner.Instance.GetEnemySniper(doors[numberDoor]);

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
