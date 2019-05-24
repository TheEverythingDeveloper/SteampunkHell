using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour, IAgressive
{

    [Tooltip("aca van a ir todas las layers que destruyan a la bala al pegar")]
    public LayerMask wallLayerMask;

    protected float _currentDistance;

    protected virtual void Reset()
    {
        _currentDistance = 0;
    }

    protected virtual void Update()
    {
        transform.position += transform.forward * VariablesPointer.bulletEnemyState.speed * Time.deltaTime;
        _currentDistance += VariablesPointer.bulletEnemyState.speed * Time.deltaTime;
        if (_currentDistance > VariablesPointer.bulletEnemyState.maxDistance)
        {
            ReturnBullet();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(((1 << collision.gameObject.layer) & wallLayerMask) != 0)
        {
            ReturnBullet();
        }
    }

    protected abstract void ReturnBullet();

    public float GetDamage() => VariablesPointer.bulletEnemyState.damage;

    public float GetAgressiveness() => VariablesPointer.bulletEnemyState.agressiveness;

    public void Hit() => ReturnBullet();
}
