using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Model : MonoBehaviour
{
    PlayerController _controller;
    View _view;
    MoveController _moveControl;
    PlayerLifeController _lifeControl;
    PlayerShootController _shootControl;
    PlayerAudioController _audioControl;

    public event Action OnDeath = delegate { };
    public event Action OnJump = delegate { };
    public event Action OnShoot = delegate { };
    public event Action<float, Vector3> OnReceiveDamage = delegate { }; //float vida actual, vector3 direccion

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _view = GetComponent<View>();
        _moveControl = GetComponent<MoveController>();
        _lifeControl = GetComponent<PlayerLifeController>();
        _shootControl = GetComponent<PlayerShootController>();
        _audioControl = GetComponent<PlayerAudioController>();
    }

    private void Start()
    {
        OnDeath += _lifeControl.PlayerDie;

        OnJump += _moveControl.TryJump;

        OnReceiveDamage += UIController.Instance.ReceiveDamageFeedback;
        OnReceiveDamage += _moveControl.HitPushForce;

        OnShoot += _shootControl.Shoot;
    }

    public void ReceiveDamage(float x, Vector3 vectorx)
    {
        OnReceiveDamage(x, vectorx);
        _audioControl.MakeSound(5); //Poner aca el numero del sonido de que te pegaron
    }

    public void Shoot()
    {
        OnShoot();
        _audioControl.MakeSound(4); //poner aca el numero del sonido de shoot
    }

    public void Jump()
    {
        OnJump();
        _audioControl.MakeSound(3); //poner aca el numero del sonido de salto
    }

    public void Death() 
    {
        OnDeath();
        _audioControl.MakeSound(2);  //poner aca el numero del sonido de muerte
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
