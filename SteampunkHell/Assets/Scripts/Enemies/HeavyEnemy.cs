using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : Enemy
{
    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Start()
    {
        base.Start();

    }

    protected override void DeathFeedback()
    {

    }

    protected override void Shoot()
    {

    }

    public static void TurnOn(HeavyEnemy b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(HeavyEnemy b)
    {
        b.gameObject.SetActive(false);
    }

    protected override void ReturnEnemy()
    {
        EnemySpawner.Instance.ReturnEnemy(this);
    }
}
