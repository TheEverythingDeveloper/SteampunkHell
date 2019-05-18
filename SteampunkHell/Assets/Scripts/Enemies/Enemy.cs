using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : RotationScript //Hereda las corrutinas de rotacion smooth
{
    [Tooltip("Cantidad de vida de la unidad")]
    public float life;
    [Tooltip("Velocidad de la unidad")]
    public float movementSpeed;
    [Tooltip("Daño de las balas que spawnea")]
    public float bulletDamage;
    [Tooltip("Velocidad de las balas que spawnea")]
    public float bulletSpeed;
    [Tooltip("Cooldown de disparo")]
    public float shootCd;

    protected PlayerController _player;
    protected float _totalLife;
    protected Rigidbody _rb;

    //TODO: Builder de enemigo

    protected virtual void Awake()
    {
        _totalLife = life;
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerController>();
    }

    public bool ReceiveDamage(float amount, Vector3 pushForce)
    {
        life -= amount;
        life = Mathf.Clamp(life, 0, _totalLife);
        ReceiveDamageFeedback(pushForce);
        if (life <= 0)
        {
            Death();
            return true;
        }
        return false;
    }

    protected void Death()
    {
        DeathFeedback();
        //TODO: Volver al pool o algo asi
    }

    protected void ReceiveDamageFeedback(Vector3 pushForce)
    {
        HitPushForce(pushForce);
        UIController.Instance.ChangeHP(life);
        //TODO: Feedback de daño
    }

    public void HitPushForce(Vector3 pushForce)
    {
        _rb.AddForce(pushForce, ForceMode.Impulse);
    }

    protected abstract void Shoot();
    protected abstract void DeathFeedback();
}
