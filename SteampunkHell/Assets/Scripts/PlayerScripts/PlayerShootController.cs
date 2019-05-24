using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    public float shootCD;
    float _totalShootCD;
    public float bulletSpeed;
    public float damage;
    public float agressiveness;
    public float maxDistance;
    [HideInInspector] public CameraController myCamera;

    private void Awake()
    {
        _totalShootCD = shootCD;
        myCamera = GetComponent<CameraController>();
    }

    private void Update()
    {
        if (shootCD > 0)
        {
            shootCD -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (shootCD > 0) return;

        var spawnedBullet = BulletSpawner.Instance.GetBulletAt(myCamera._myCam.transform);
        VariablesPointer.bulletEnemyState.speed = bulletSpeed;
        VariablesPointer.bulletEnemyState.maxDistance = maxDistance;
        VariablesPointer.bulletEnemyState.agressiveness = agressiveness;
        VariablesPointer.bulletEnemyState.damage = damage;
        shootCD = _totalShootCD;
    }
}
