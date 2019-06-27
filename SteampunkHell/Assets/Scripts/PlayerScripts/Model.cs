using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Model : MonoBehaviour
{
    PlayerController _controller;
    View _view;
    [HideInInspector] public CameraController cameraControl;
    MoveController _moveControl;
    PlayerLifeController _lifeControl;
    PlayerAdrenalinController _adrenalinControl;
    PlayerPointsController _pointsControl;
    PlayerShootController _shootControl;
    PlayerAudioController _audioControl;
    PlayerStrategyController _strategyControl;

    private bool ulting;
    public bool isShopping;

    public Door inDoor;

    public event Action OnDeath = delegate { };
    public event Action OnJump = delegate { };
    public event Action OnShoot = delegate { };
    public event Action OnStopShooting = delegate { };
    public event Action<Ulti, bool> OnUlti = delegate { }; //ulti = tipo de ulti, bool = si se activa o desactiva
    public event Action<Unit> OnKill = delegate { };
    public event Action<float, Vector3> OnReceiveDamage = delegate { }; //float vida actual, vector3 direccion

    public event Action<bool> OnCanShop = delegate { }; //TODO: Feedback puede comprar.
    public event Action OnStartShopping = delegate { }; //TODO: Feedback cuando empieza a comprar
    public event Action OnEndShopping = delegate { }; //TODO: Feedback cuando termina de comprar
    public event Action<int> OnBuyWeapon = delegate { };

    private void Awake()
    {
        _controller = new PlayerController(this);
        cameraControl = GetComponent<CameraController>();

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

        EventsManager.SubscribeToEvent(TypeOfEvent.EnemyDead, KillEnemy);
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

        OnCanShop += _moveControl.EnterShop;
        //TODO: OnCanShop += _view.CanShop;

        OnBuyWeapon += _strategyControl.weaponMng.ChangeWeapon;
        //TODO: OnBuyWeapon += _view.ChangeWeapon; //cambia la interfaz segun el arma.
        //TODO: OnBuyWeapon += _moveController.ChangeWeapon; //segun el arma nos movemos diferente
    }

    private void Update()
    {
        if (isShopping || GameManager.paused) return;

        _controller.ArtificialUpdate();
    }

    private void FixedUpdate()
    {
        if (isShopping || GameManager.paused) return;

        _moveControl.ArtificialFixedUpdate();
    }

    public void ReceiveDamage(float x, Vector3 vectorx)
    {
        OnReceiveDamage(x, vectorx);
        _audioControl.MakeSound(5); //TODO: Poner aca el numero del sonido de que te pegaron
    }

    public void KillEnemy(params object[] parameters)
    {
        OnKill((Unit)parameters[0]);
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
    }

    public void CanShop(bool enter)
    {
        OnCanShop(enter);
    }

    public void OpenDoor()
    {
        if (inDoor == null)
            return;

        inDoor.StartCoroutine(inDoor.OpenDoorCoroutine());
        inDoor = null;
    }

    public void StartStage()
    {
        WaveManager.Instance.NewStage();
    }
    public void StartShopping()
    {
        isShopping = true;
        OnStartShopping();
    }

    public void EndShopping()
    {
        isShopping = false;
        OnEndShopping();
    }

    public void BuyWeapon(int ID)
    {
        OnBuyWeapon(ID);
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