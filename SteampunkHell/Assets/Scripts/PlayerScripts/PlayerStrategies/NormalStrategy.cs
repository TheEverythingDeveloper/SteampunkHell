using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStrategy : PlayerStrategy
{
    public NormalStrategy(PlayerStrategyController owner, float bulletSpeed, float maxDistance, float agressiveness, float damage, float cd) : base(owner, bulletSpeed, maxDistance, agressiveness, damage, cd)
    {
        strategyType = Strategy.Normal;
    }

    public override void Enter()
    {
        base.Enter();
        _owner.GetComponent<CameraController>().ChangeZoom(50);
    }

    public override void Jump()
    {

    }

    public override void Move()
    {

    }

    public override void Shoot(bool start)
    {
        base.Shoot(start);
        _owner.weaponMng.actualWeapon.RealShoot(this);
    }
}
