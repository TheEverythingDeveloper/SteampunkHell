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
    PlayerAdrenalinController _adrenalinControl;
    PlayerPointsController _pointsControl;
    PlayerShootController _shootControl;
    PlayerAudioController _audioControl;
    PlayerStrategyController _strategyControl;

    private bool ulting;

    public event Action OnDeath = delegate { };
    public event Action OnJump = delegate { };
    public event Action OnShoot = delegate { };
    public event Action OnStopShooting = delegate { };
    public event Action<Ulti, bool> OnUlti = delegate { }; //ulti = tipo de ulti, bool = si se activa o desactiva
    public event Action<Unit> OnKill = delegate { };
    public event Action<float, Vector3> OnReceiveDamage = delegate { }; //float vida actual, vector3 direccion

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _view = GetComponent<View>();
        _moveControl = GetComponent<MoveController>();
        _lifeControl = GetComponent<PlayerLifeController>();
        _adrenalinControl = GetComponent<PlayerAdrenalinController>();
        _pointsControl = GetComponent<PlayerPointsController>();
        _shootControl = GetComponent<PlayerShootController>();
        _audioControl = GetComponent<PlayerAudioController>();
        _strategyControl = GetComponent<PlayerStrategyController>();
    }

    private void Start()
    {
        OnDeath += _lifeControl.PlayerDie;

        OnKill += _pointsControl.KillEnemy;
        OnKill += _adrenalinControl.KillEnemy;

        OnUlti += UIController.Instance.UltiActivation;
        OnUlti += _adrenalinControl.UltiActivation;
        OnUlti += _strategyControl.UltiActivation;

        OnJump += _moveControl.TryJump;

        OnReceiveDamage += UIController.Instance.ReceiveDamageFeedback;
        OnReceiveDamage += _moveControl.HitPushForce;

        OnShoot += _shootControl.Shoot;
        OnShoot += UIController.Instance.StartShooting;

        OnStopShooting += UIController.Instance.StopShooting;
    }

    public void ReceiveDamage(float x, Vector3 vectorx)
    {
        OnReceiveDamage(x, vectorx);
        _audioControl.MakeSound(5); //TODO: Poner aca el numero del sonido de que te pegaron
    }

    public void KillEnemy(Unit unitID)
    {
        OnKill(unitID);
    }

    public void UltiActivation(Ulti ultiID)
    {
        if (ulting)
        {
            ulting = false;
        }
        else
        {
            if (_adrenalinControl.CheckUltiActivation()) ulting = true;
            else Debug.Log("No hay suficiente adrenalina");
        }
        OnUlti(ultiID, ulting);
    }

    public void Shoot()
    {
        OnShoot();
    }

    public void StopShooting()
    {
        OnStopShooting();
    }

    public void Jump()
    {
        OnJump();
        _audioControl.MakeSound(3); //TODO: poner aca el numero del sonido de salto
    }

    public void Death() 
    {
        OnDeath();
        _audioControl.MakeSound(2);  //TODO: poner aca el numero del sonido de muerte
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}

public enum Ulti
{
    AGRESSIVE,
    PRECISION,
    REWIND
}

public enum Unit
{
    LASER = 0,
    HEAVY = 1,
    EXPLOSIVE = 2,
    ZEPPELLIN = 3,
    BOSS = 4,
    Other = 5
}