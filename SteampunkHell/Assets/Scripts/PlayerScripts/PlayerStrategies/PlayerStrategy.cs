using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStrategy
{
    protected PlayerStrategyController _owner;
    public Strategy strategyType;
    protected float _bulletSpeed;
    protected float _maxDistance;
    protected float _agressiveness;
    protected float _damage;
    protected float _cd;

    public PlayerStrategy(PlayerStrategyController owner, float bulletSpeed, float maxDistance, float agressiveness, float damage, float cd)
    {
        _owner = owner;
        _bulletSpeed = bulletSpeed;
        _maxDistance = maxDistance;
        _agressiveness = agressiveness;
        _damage = damage;
        _cd = cd;
    }

    public virtual void Enter()
    {
        VariablesPointer.PlayerBulletState.speed = _bulletSpeed;
        VariablesPointer.PlayerBulletState.maxDistance = _maxDistance;
        VariablesPointer.PlayerBulletState.agressiveness = _agressiveness;
        VariablesPointer.PlayerBulletState.damage = _damage;
        _owner.ChangeShootCD(_cd);
    }

    public virtual void Shoot(bool start)
    {
        _owner.ShootCallBack(start, _bulletSpeed);
    }

    public virtual void Jump()
    {

    }

    public virtual void Move()
    {

    }
}
