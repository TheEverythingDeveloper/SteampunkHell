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
    
    public void GetBulletAt(Transform targetTransform)
    {
        var b = pool.GetObject();
        b.transform.position = targetTransform.position;
        b.transform.rotation = targetTransform.rotation;
    }

    public EnemyBullet bulletPrefab;
    public EnemyBullet BulletFactory()
    {
        return Instantiate(bulletPrefab);
    }


    private ObjectPool<EnemyBullet> pool;

    public void ReturnBullet(EnemyBullet b)
    {
        pool.ReturnObject(b);
    }
}
