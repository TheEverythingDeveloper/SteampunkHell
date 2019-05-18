using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public static void TurnOn(PlayerBullet b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(PlayerBullet b)
    {
        b.gameObject.SetActive(false);
    }

    protected override void ReturnBullet()
    {
        BulletSpawner.Instance.ReturnBullet(this);
    }
}
