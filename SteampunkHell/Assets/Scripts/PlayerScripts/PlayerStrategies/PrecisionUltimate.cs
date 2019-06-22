using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionUltimate : PlayerStrategy
{
    public PrecisionUltimate(PlayerStrategyController owner, float bulletSpeed, float maxDistance, float agressiveness, float damage, float cd) : base(owner, bulletSpeed, maxDistance, agressiveness, damage, cd)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _owner.GetComponent<CameraController>().ChangeZoom(40);
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
        Debug.Log("precision shoot");
        for (int i = 0; i < _owner.weaponMng.actualWeapon.spawnBulletsTransforms.Length; i++)
        {
            BulletSpawner.Instance.GetBulletAt(_owner.weaponMng.actualWeapon.spawnBulletsTransforms[i]);
        }
    }
}
