using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Animator _anim;
    public float damage;
    public float agressiveness;
    public int reloadAmount;
    protected int _totalReload;
    public float reloadSpeed;
    protected bool _reloading;
    public float shootCD;
    public PlayerAudioController audioController;
    public Transform[] spawnBulletsTransforms;
    public AudioClip reloadClip;
    public AudioClip shootClip;

    public bool canShoot = true; //Todas las armas que no disparen balas comunes, van a tener esto en false.

    public virtual bool CanShoot()
    {
        return reloadAmount > 0;
    }

    protected virtual void Awake()
    {
        _anim = GetComponent<Animator>();
        _totalReload = reloadAmount;
    }
    /// <summary>
    /// Se activa al disparar. Animacion y lo que sea de disparar aca.
    /// </summary>
    public virtual void Shoot(bool start, float speed)
    {
        if (start)
        {
            audioController.MakeSound(shootClip);
            reloadAmount--;
            _anim.speed = speed * 0.1f;
            _anim.SetTrigger("Shoot");
        }
    }

    public void RealShoot(PlayerStrategy actualUsedStrategy)
    {
        if (!canShoot) return;

        switch (actualUsedStrategy.strategyType)
        {
            case Strategy.Normal:
                break;
            case Strategy.AgressiveUlti:
                for (int i = 0; i < spawnBulletsTransforms.Length; i++)
                {
                    Transform originalTransform = spawnBulletsTransforms[i];
                    BulletSpawner.Instance.GetBulletAt(originalTransform);
                    originalTransform.Rotate(Vector3.up, 10);
                    BulletSpawner.Instance.GetBulletAt(originalTransform);
                    originalTransform.Rotate(Vector3.up, -20);
                    BulletSpawner.Instance.GetBulletAt(originalTransform);
                    originalTransform.Rotate(Vector3.up, 10);
                }
                return; //return para que no vaya al otro for que sigue
            case Strategy.PrecisionUlti:
                //TODO: Pensar y hacer que cambie el tiro
                break;
            case Strategy.RewindUlti:
                //TODO: Hacer que las balas vayan mas lentas, etc.
                break;
        }
        for (int i = 0; i < spawnBulletsTransforms.Length; i++)
        {
            BulletSpawner.Instance.GetBulletAt(spawnBulletsTransforms[i]);
        }
    }

    /// <summary>
    /// En el hijo vacia si es que no recarga, override si es que recarga
    /// </summary>
    public virtual void Reload()
    {
        if (_reloading) return;

        _reloading = true;
        audioController.MakeSound(reloadClip);
        _anim.speed = reloadSpeed;
        StartCoroutine(ReloadTimer(reloadSpeed));
    }

    /// <summary>
    /// Mostrar el arma. No tiene ningun efecto en gameplay. Simplemente animacion.
    /// </summary>
    public virtual void ShowWeapon()
    {
        _anim.SetTrigger("Show");
    }

    protected virtual IEnumerator ReloadTimer(float reloadSpeed)
    {
        _anim.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadSpeed);
        reloadAmount = _totalReload;
        _reloading = false;
    }
}
