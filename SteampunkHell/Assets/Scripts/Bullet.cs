using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour, IAgressive
{
    public float speed;
    public float maxDistance;
    [Tooltip("Cuanta vida va a sacar cuando pega")]
    public float damage;
    [Tooltip("Cuanto va a empujar cuando pega a algo")]
    public float agressiveness;

    protected float _currentDistance;

    protected virtual void Reset()
    {
        _currentDistance = 0;
    }

    protected virtual void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        _currentDistance += speed * Time.deltaTime;
        if (_currentDistance > maxDistance)
        {
            ReturnBullet();
        }
    }

    protected abstract void ReturnBullet();

    public float GetDamage() => damage;

    public float GetAgressiveness() => agressiveness;
}
