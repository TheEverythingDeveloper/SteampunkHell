using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : RotationScript /*Hereda las corrutinas de rotacion smooth*/, IAgressive, IVulnerable
{
    [Tooltip("Cantidad de vida de la unidad")]
    public float life;
    [Tooltip("Daño de las balas que spawnea")]
    public float bulletDamage;
    [Tooltip("Cooldown de disparo")]
    public float shootCd;

    public bool dead;


    [Tooltip("Cuanto va a empujar cuando pega a algo")]
    public float agressiveness;

    protected PlayerLifeController _player;
    protected float _totalLife;
    protected Rigidbody _rb;

    //TODO: Builder de enemigo

    protected virtual void Awake()
    {
        _totalLife = life;
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<PlayerLifeController>();
    }

    protected virtual void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        if (dead) yield break;
        yield return new WaitForSeconds(shootCd + Random.Range(-1f,1f));
        Shoot();
        StartCoroutine(ShootCoroutine());
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
        dead = true;
        DeathFeedback();
        //TODO: Volver al pool o algo asi
    }

    protected void ReceiveDamageFeedback(Vector3 pushForce)
    {
        HitPushForce(pushForce);
        //TODO: Feedback de daño
    }

    public void HitPushForce(Vector3 pushForce)
    {
        _rb.AddForce(pushForce, ForceMode.Impulse);
    }

    protected abstract void Shoot();
    protected abstract void DeathFeedback();

    public float GetDamage() => bulletDamage; //si el jugador lo toca le va a sacar vida y empujar
    public float GetAgressiveness() => agressiveness; //cuanto va a empujar al player cuando lo toca

    public void Hit() { /*TODO: Feedback del enemigo al pegarte*/ }
}
