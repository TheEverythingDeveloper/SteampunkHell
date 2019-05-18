using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public static BulletSpawner Instance
    {
        get
        {
            return _Instance;
        }
    }

    private static BulletSpawner _Instance;

    private void Start()
    {
        _Instance = this;
        pool = new ObjectPool<PlayerBullet>(BulletFactory, PlayerBullet.TurnOn, PlayerBullet.TurnOff, 20, true);
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

    public PlayerBullet BulletFactory()
    {
        return Instantiate(bulletPrefab);
    }

    public PlayerBullet bulletPrefab;

    private ObjectPool<PlayerBullet> pool;

    public void ReturnBullet(PlayerBullet b)
    {
        pool.ReturnObject(b);
    }
}
