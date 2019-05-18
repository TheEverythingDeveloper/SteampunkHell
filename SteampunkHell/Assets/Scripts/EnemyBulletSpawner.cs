using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletSpawner : MonoBehaviour
{
    public static EnemyBulletSpawner Instance
    {
        get
        {
            return _Instance;
        }
    }

    private static EnemyBulletSpawner _Instance;

    private void Start()
    {
        _Instance = this;
        pool = new ObjectPool<EnemyBullet>(BulletFactory, EnemyBullet.TurnOn, EnemyBullet.TurnOff, 50, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var b = pool.GetObject();
            b.transform.position = Vector3.zero;
            b.transform.forward = Vector3.forward;
        }
    }

    public void GetBulletAt(Transform targetTransform)
    {
        var b = pool.GetObject();
        b.transform.position = targetTransform.position;
        b.transform.localRotation = targetTransform.localRotation;
    }

    public EnemyBullet BulletFactory()
    {
        return Instantiate(bulletPrefab);
    }

    public EnemyBullet bulletPrefab;

    private ObjectPool<EnemyBullet> pool;

    public void ReturnBullet(EnemyBullet b)
    {
        pool.ReturnObject(b);
    }
}
