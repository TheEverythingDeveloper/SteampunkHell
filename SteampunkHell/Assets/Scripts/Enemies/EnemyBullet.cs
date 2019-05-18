using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    public static void TurnOn(EnemyBullet b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(EnemyBullet b)
    {
        b.gameObject.SetActive(false);
    }

    protected override void ReturnBullet()
    {
        EnemyBulletSpawner.Instance.ReturnBullet(this);
    }
}
