using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void Awake()
    {
        _currentDistance = VariablesPointer.EnemyBulletState.maxDistance;
    }

    protected void Update()
    {
        transform.position += transform.forward * VariablesPointer.EnemyBulletState.bulletSpeed * Time.deltaTime;
        _currentDistance += VariablesPointer.EnemyBulletState.bulletSpeed * Time.deltaTime;
        if (_currentDistance > VariablesPointer.EnemyBulletState.maxDistance)
        {
            ReturnBullet();
        }
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

    public override float GetDamage()
    {
        return VariablesPointer.EnemyBulletState.damage;
    }

    public override float GetAgressiveness()
    {
        return VariablesPointer.EnemyBulletState.agressiveness;
    }
}
