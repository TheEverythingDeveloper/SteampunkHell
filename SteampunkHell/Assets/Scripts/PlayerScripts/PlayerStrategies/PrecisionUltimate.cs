using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionUltimate : PlayerStrategy
{
    public PrecisionUltimate(PlayerStrategyController owner, float bulletSpeed, float maxDistance, float agressiveness, float damage, float cd) : base(owner, bulletSpeed, maxDistance, agressiveness, damage, cd)
    {
        strategyType = Strategy.PrecisionUlti;
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

    public override void Shoot(bool start)
    {
        _owner.ShootCallBack(start, _bulletSpeed);
        _owner.weaponMng.actualWeapon.RealShoot(this);
    }
}
