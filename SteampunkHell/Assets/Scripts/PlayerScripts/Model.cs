using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Model : MonoBehaviour
{
    [HideInInspector] public PlayerController controller;
    [HideInInspector] public View view;
    [HideInInspector] public CameraController cameraControl;
    [HideInInspector] public MoveController moveControl;
    [HideInInspector] public PlayerLifeController lifeControl;
    [HideInInspector] public PlayerAdrenalinController adrenalineControl;
    [HideInInspector] public PlayerPointsController pointsControl;
    [HideInInspector] public PlayerShootController shootControl;
    [HideInInspector] public PlayerAudioController audioControl;
    [HideInInspector] public PlayerStrategyController strategyControl;

    private bool ulting;
    public bool isShopping;

    public Door inDoor;

    public event Action OnDeath = delegate { };
    public event Action OnJump = delegate { };
    public event Action OnReload = delegate { };
    public event Action<bool> OnShoot = delegate { };
    public event Action<Ulti, bool> OnUlti = delegate { }; //ulti = tipo de ulti, bool = si se activa o desactiva
    public event Action<Unit> OnKill = delegate { };
    public event Action<float, Vector3> OnReceiveDamage = delegate { }; //float vida actual, vector3 direccion

    public event Action<bool> OnCanShop = delegate { }; //TODO: Feedback puede comprar.
    public event Action OnStartShopping = delegate { }; //TODO: Feedback cuando empieza a comprar
    public event Action OnEndShopping = delegate { }; //TODO: Feedback cuando termina de comprar
    public event Action<int> OnBuyWeapon = delegate { };

    private void Awake()
    {
        controller = new PlayerController(this);
        cameraControl = GetComponent<CameraController>();

        view = GetComponent<View>();
        moveControl = GetComponent<MoveController>();
        lifeControl = GetComponent<PlayerLifeController>();
        adrenalineControl = GetComponent<PlayerAdrenalinController>();
        pointsControl = GetComponent<PlayerPointsController>();
        shootControl = GetComponent<PlayerShootController>();
        audioControl = GetComponent<PlayerAudioController>();
        strategyControl = GetComponent<PlayerStrategyController>();
    }

    private void Start()
    {
        OnDeath += lifeControl.PlayerDie;

        EventsManager.SubscribeToEvent(TypeOfEvent.EnemyDead, KillEnemy);
        OnKill += pointsControl.KillEnemy;
        OnKill += adrenalineControl.KillEnemy;

        OnUlti += UIController.Instance.UltiActivation;
        OnUlti += adrenalineControl.UltiActivation;
        OnUlti += strategyControl.UltiActivation;

        OnReload += strategyControl.Reload;

        OnJump += moveControl.TryJump;

        OnReceiveDamage += UIController.Instance.ReceiveDamageFeedback;
        OnReceiveDamage += moveControl.HitPushForce;

        OnShoot += shootControl.Shoot;
        OnShoot += UIController.Instance.Shoot;

        OnCanShop += moveControl.EnterShop;
        //TODO: OnCanShop += _view.CanShop;

        OnBuyWeapon += strategyControl.ChangeWeapon;
        //TODO: OnBuyWeapon += _view.ChangeWeapon; //cambia la interfaz segun el arma.
        //TODO: OnBuyWeapon += _moveController.ChangeWeapon; //segun el arma nos movemos diferente
    }

    private void Update()
    {
        if (isShopping || GameManager.paused) return;

        controller.ArtificialUpdate();
    }

    private void FixedUpdate()
    {
        if (isShopping || GameManager.paused) return;

        moveControl.ArtificialFixedUpdate();
    }

    public void ReceiveDamage(float x, Vector3 vectorx)
    {
        OnReceiveDamage(x, vectorx);
        audioControl.MakeSound(5); //TODO: Poner aca el numero del sonido de que te pegaron
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
            if (adrenalineControl.CheckUltiActivation()) ulting = true;
            else Debug.Log("No hay suficiente adrenalina");
        }
        OnUlti(ultiID, ulting);
    }

    public void Shoot(bool start)
    {
        OnShoot(start);
    }

    public void Reload()
    {
        OnReload();
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
        audioControl.MakeSound(2);  //TODO: poner aca el numero del sonido de muerte
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