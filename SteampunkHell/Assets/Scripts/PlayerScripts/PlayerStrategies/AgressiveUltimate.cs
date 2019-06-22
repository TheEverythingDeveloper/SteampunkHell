using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressiveUltimate : PlayerStrategy
{
    public AgressiveUltimate(PlayerStrategyController owner, float bulletSpeed, float maxDistance, float agressiveness, float damage, float cd) : base(owner, bulletSpeed, maxDistance, agressiveness, damage, cd)
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
        Debug.Log("agressive shoot");
        for (int i = 0; i < _owner.weaponMng.actualWeapon.spawnBulletsTransforms.Length; i++)
        {
            Transform originalTransform = _owner.weaponMng.actualWeapon.spawnBulletsTransforms[i];
            BulletSpawner.Instance.GetBulletAt(originalTransform);
            originalTransform.Rotate(Vector3.up, 20);
            BulletSpawner.Instance.GetBulletAt(originalTransform);
            originalTransform.Rotate(Vector3.up, -40);
            BulletSpawner.Instance.GetBulletAt(originalTransform);
            originalTransform.Rotate(Vector3.up, 20);
        }
    }
}
