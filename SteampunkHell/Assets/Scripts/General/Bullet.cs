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

    [Tooltip("aca van a ir todas las layers que destruyan a la bala al pegar")]
    public LayerMask wallLayerMask;

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

    private void OnCollisionEnter(Collision collision)
    {
        if(((1 << collision.gameObject.layer) & wallLayerMask) != 0)
        {
            ReturnBullet();
        }
    }

    protected abstract void ReturnBullet();

    public float GetDamage() => damage;

    public float GetAgressiveness() => agressiveness;

    public void Hit() => ReturnBullet();
}
