using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Animator _anim;
    public int reloadAmount;
    protected int _totalReload;
    public float reloadSpeed;
    protected bool _reloading;
    public PlayerAudioController audioController;
    public Transform[] spawnBulletsTransforms;

    public virtual bool CanShoot()
    {
        return reloadAmount > 0;
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _totalReload = reloadAmount;
    }
    /// <summary>
    /// Se activa al disparar. Animacion y lo que sea de disparar aca.
    /// </summary>
    public virtual void Shoot(float speed)
    {
        audioController.MakeSound(4); 
        reloadAmount--;
        _anim.speed = speed * 0.1f;
        _anim.SetTrigger("Shoot");
    }

    /// <summary>
    /// En el hijo vacia si es que no recarga, override si es que recarga
    /// </summary>
    public virtual void Reload() { }

    /// <summary>
    /// Mostrar el arma. No tiene ningun efecto en gameplay. Simplemente animacion.
    /// </summary>
    public virtual void ShowWeapon()
    {
        _anim.SetTrigger("Show");
    }
}
