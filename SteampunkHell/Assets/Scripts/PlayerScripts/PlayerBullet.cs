using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private void Awake()
    {
        _currentDistance = VariablesPointer.PlayerBulletState.maxDistance;
    }

    protected void Update()
    {
        transform.position += transform.forward * VariablesPointer.PlayerBulletState.bulletSpeed * Time.deltaTime;
        _currentDistance += VariablesPointer.PlayerBulletState.bulletSpeed * Time.deltaTime;
        if (_currentDistance > VariablesPointer.PlayerBulletState.maxDistance)
        {
            ReturnBullet();
        }
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

    public override float GetDamage()
    {
        return VariablesPointer.PlayerBulletState.damage;
    }

    public override float GetAgressiveness()
    {
        return VariablesPointer.PlayerBulletState.agressiveness;
    }
}
