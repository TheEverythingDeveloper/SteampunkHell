using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void Awake()
    {
        _currentDistance = VariablesPointer.bulletEnemyState.maxDistance;
        _bulletSpeed = VariablesPointer.bulletEnemyState.speed;
        _maxDistance = VariablesPointer.bulletEnemyState.maxDistance;
        _damage = VariablesPointer.bulletEnemyState.damage;
        _agressiveness = VariablesPointer.bulletEnemyState.agressiveness;
    }
    public static void TurnOn(EnemyBullet b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(EnemyBullet b)
    {
        b.gameObject.SetActive(false);
    }

    protected override void ReturnBullet()
    {
        EnemyBulletSpawner.Instance.ReturnBullet(this);
    }
}
