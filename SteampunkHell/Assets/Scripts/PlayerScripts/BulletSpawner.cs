using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner Instance
    {
        get { return _Instance; }
    }
    private static BulletSpawner _Instance;

    private void Start()
    {
        _Instance = this;
        pool = new ObjectPool<PlayerBullet>(BulletFactory, PlayerBullet.TurnOn, PlayerBullet.TurnOff, 20, true);
    }
    public PlayerBullet GetBulletAt(Transform targetTransform)
    {
        var b = pool.GetObject();
        b.transform.position = targetTransform.position;
        b.transform.localRotation = targetTransform.localRotation;
        return b;
    }

    public PlayerBullet bulletPrefab;
    public PlayerBullet BulletFactory()
    {
        return Instantiate(bulletPrefab);
    }

    private ObjectPool<PlayerBullet> pool;

    public void ReturnBullet(PlayerBullet b)
    {
        pool.ReturnObject(b);
    }
}
