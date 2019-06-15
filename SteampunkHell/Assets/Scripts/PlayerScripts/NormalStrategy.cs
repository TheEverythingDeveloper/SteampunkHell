using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalStrategy : PlayerStrategy
{
    public NormalStrategy(PlayerStrategyController owner, float bulletSpeed, float maxDistance, float agressiveness, float damage, float cd) : base(owner, bulletSpeed, maxDistance, agressiveness, damage, cd)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _owner.GetComponent<CameraController>().ChangeZoom(60);
    }

    public override void Jump()
    {

    }

    public override void Move()
    {

    }

    public override void Shoot()
    {
        base.Shoot();
        Debug.Log("normal shoot");
        BulletSpawner.Instance.GetBulletAt(_owner.GetComponent<CameraController>()._myCam.transform);
    }
}
