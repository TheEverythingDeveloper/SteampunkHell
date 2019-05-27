using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private void Awake()
    {
        _currentDistance = VariablesPointer.PlayerBulletState.maxDistance;
        _bulletSpeed = VariablesPointer.PlayerBulletState.speed;
        _maxDistance = VariablesPointer.PlayerBulletState.maxDistance;
        _damage = VariablesPointer.PlayerBulletState.damage;
        _agressiveness = VariablesPointer.PlayerBulletState.agressiveness;
    }

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
