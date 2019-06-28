using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindUltimate : PlayerStrategy
{
    public RewindUltimate(PlayerStrategyController owner, float bulletSpeed, float maxDistance, float agressiveness, float damage, float cd) : base(owner, bulletSpeed, maxDistance, agressiveness, damage, cd)
    {
        strategyType = Strategy.RewindUlti;
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
