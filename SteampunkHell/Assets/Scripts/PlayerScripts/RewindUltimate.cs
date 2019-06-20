using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindUltimate : PlayerStrategy
{
    public RewindUltimate(PlayerStrategyController owner, float bulletSpeed, float maxDistance, float agressiveness, float damage, float cd) : base(owner, bulletSpeed, maxDistance, agressiveness, damage, cd)
    {

    }

    public override void Jump()
    {

    }

    public override void Move()
    {

    }

    public override void Shoot()
    {
        _owner.ShootCallBack(_bulletSpeed);
        Debug.Log("rewind shoot");
        for (int i = 0; i < _owner.actualWeapon.spawnBulletsTransforms.Length; i++)
        {
            BulletSpawner.Instance.GetBulletAt(_owner.actualWeapon.spawnBulletsTransforms[i]);
        }
    }
}
