using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour, IAgressive
{

    [Tooltip("aca van a ir todas las layers que destruyan a la bala al pegar")]
    public LayerMask wallLayerMask;

    protected float _currentDistance;

    protected float _bulletSpeed;
    protected float _maxDistance;
    protected float _damage;
    protected float _agressiveness;
    
    protected virtual void Reset()
    {
        _currentDistance = 0;
    }

    protected virtual void Update()
    {
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime;
        _currentDistance += _bulletSpeed * Time.deltaTime;
        if (_currentDistance > _maxDistance)
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

    public float GetDamage() => _damage;

    public float GetAgressiveness() => _agressiveness;

    public void Hit() => ReturnBullet();
}
