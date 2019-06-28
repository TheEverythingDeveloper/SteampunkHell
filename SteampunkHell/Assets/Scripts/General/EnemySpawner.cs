using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner Instance
    {
        get
        {
            return _Instance;
        }
    }

    private static EnemySpawner _Instance;

    private void Start()
    {
        _Instance = this;
        poolEnemyExplosive = new ObjectPool<EnemyExplosive>(EnemyExplosiveFactory, EnemyExplosive.TurnOn, EnemyExplosive.TurnOff, 5, true);
        poolSniperEnemy = new ObjectPool<SniperEnemy>(EnemySniperFactory, SniperEnemy.TurnOn, SniperEnemy.TurnOff, 5, true);
        poolEnemyHeavy = new ObjectPool<HeavyEnemy>(EnemyHeavyFactory, HeavyEnemy.TurnOn, HeavyEnemy.TurnOff, 5, true);
        poolZeppellinEnemy = new ObjectPool<ZeppellinEnemy>(EnemyZeppellinFactory, ZeppellinEnemy.TurnOn, ZeppellinEnemy.TurnOff, 5, true);
    }

    public void GetEnemyExplosive(Transform targetTransform)
    {
        var b = poolEnemyExplosive.GetObject();
        b.transform.position = targetTransform.position;
        b.transform.localRotation = targetTransform.localRotation;
    }
    public void GetEnemySniper(Transform targetTransform)
    {
        var b = poolSniperEnemy.GetObject();
        b.transform.position = targetTransform.position;
        b.transform.localRotation = targetTransform.localRotation;
    }
    public void GetEnemyHeavy(Transform targetTransform)
    {
        var b = poolEnemyHeavy.GetObject();
        b.transform.position = targetTransform.position;
        b.transform.localRotation = targetTransform.localRotation;
    }
    public void GetEnemyZeppellin(Transform targetTransform)
    {
        var b = poolZeppellinEnemy.GetObject();
        b.transform.position = targetTransform.position;
        b.transform.localRotation = targetTransform.localRotation;
    }

    public SniperEnemy EnemySniperFactory()
    {
        return Instantiate(enemySniperPrefab);
    }
    public EnemyExplosive EnemyExplosiveFactory()
    {
        return Instantiate(enemyExplosivePrefab);
    }
    public HeavyEnemy EnemyHeavyFactory()
    {
        return Instantiate(enemyHeavyPrefab);
    }
    public ZeppellinEnemy EnemyZeppellinFactory()
    {
        return Instantiate(enemyZeppellinPrefab);
    }

    public SniperEnemy enemySniperPrefab;
    public EnemyExplosive enemyExplosivePrefab;
    public HeavyEnemy enemyHeavyPrefab;
    public ZeppellinEnemy enemyZeppellinPrefab;

    private ObjectPool<SniperEnemy> poolSniperEnemy;
    private ObjectPool<EnemyExplosive> poolEnemyExplosive;
    private ObjectPool<HeavyEnemy> poolEnemyHeavy;
    private ObjectPool<ZeppellinEnemy> poolZeppellinEnemy;

    public void ReturnEnemy(SniperEnemy b)
    {
        poolSniperEnemy.ReturnObject(b);
    }
    public void ReturnEnemy(EnemyExplosive b)
    {
        poolEnemyExplosive.ReturnObject(b);
    }
    public void ReturnEnemy(HeavyEnemy b)
    {
        poolEnemyHeavy.ReturnObject(b);
    }
    public void ReturnEnemy(ZeppellinEnemy b)
    {
        poolZeppellinEnemy.ReturnObject(b);
    }
}
