using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugController : MonoBehaviour
{
    public GameObject _UIGo;
    public GameObject _EnemiesGo;

    public bool enemies;

    private void Awake()
    {
        _UIGo.SetActive(true);
        _EnemiesGo.SetActive(enemies);
    }
}
