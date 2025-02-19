﻿using System.Collections;
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

    private void OnCollisionEnter(Collision collision)
    {
        if(((1 << collision.gameObject.layer) & wallLayerMask) != 0)
        {
            ReturnBullet();
        }
    }

    protected abstract void ReturnBullet();
    public abstract float GetDamage();
    public abstract float GetAgressiveness();

    public void Hit()
    {
        ReturnBullet();
    }
}
