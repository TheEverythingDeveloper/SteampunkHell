using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgressiveUltimate : PlayerStrategy
{
    public AgressiveUltimate(PlayerStrategyController owner, float bulletSpeed, float maxDistance, float agressiveness, float damage, float cd) : base(owner, bulletSpeed, maxDistance, agressiveness, damage, cd)
    {
        strategyType = Strategy.AgressiveUlti;
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
