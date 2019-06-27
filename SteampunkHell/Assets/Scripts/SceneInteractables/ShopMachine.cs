using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

/// <summary>
/// Es el Model del MVC de la tienda
/// </summary>
public class ShopMachine : MonoBehaviour
{
    private Model _actualUser;
    public CinemachineVirtualCamera shopCM;

    private ShopsManager _shopMng; //Manager de todos los shops.
    private ShopControl _controller;
    private ShopItemScroller _itemScroller;
    private ShopView _view;
    private ShopAudioController _audioControl;

    public int actualSelection;

    public event Action<bool> OnTrigger = delegate { }; //true = enter, false = exit
    public event Action<bool> OnUsing = delegate { };
    public event Action<bool> OnBuy = delegate { }; // true = lo compro, false = no tenia suficiente plata
    public event Action<bool> OnSelectItem = delegate { };

    private void Awake()
    {
        _shopMng = transform.parent.GetComponent<ShopsManager>();
        _view = GetComponent<ShopView>();
        _controller = new ShopControl(this);

        _itemScroller = GetComponentInChildren<ShopItemScroller>();
        _itemScroller.shopModel = this;
        _itemScroller.shopMng = _shopMng;
        _itemScroller.UpdateIcons();
        _audioControl = GetComponent<ShopAudioController>();
    }

    private void Start()
    {
        OnTrigger += _view.CanShop;
        OnTrigger += _audioControl.CanShopAudio;

        OnUsing += _view.Using;
        OnUsing += _audioControl.Using;

        OnBuy += _view.Buy;
        OnBuy += _audioControl.Buy;
        OnBuy += _itemScroller.Buy;

        OnSelectItem += _audioControl.Select;
        OnSelectItem += _itemScroller.Select;
        _itemScroller.SelectionCallback += _view.Select;
    }

    public void TryBuy()
    {
        bool canBuy = _itemScroller.CanBuy();

        OnBuy(canBuy);
        if (canBuy)
        {
            StartCoroutine(ExitShopping());
        }
    }

    public void ChangeWeapon(int weaponID)
    {
        _actualUser.BuyWeapon(weaponID);
    }

    /// <summary>
    /// Si el bool es true, entonces va a mover la seleccion para la derecha, sino para la izquierda.
    /// </summary>
    /// <param name="ID"></param>
    public void SelectItem(bool direction)
    {
        OnSelectItem(direction);
    }

    public void PlayerOnTrigger(bool on)
    {
        OnTrigger(on);
    }

    public void PlayerEnter(Model model, PlayerPointsController pointController)
    {
        _actualUser = model;
        _itemScroller.userCurrency = pointController;
        _actualUser.cameraControl.ChangeCamera(shopCM);
        GameManager.Instance.canPause = false;
        OnUsing(true);
        SelectItem(true);
    }

    public IEnumerator ExitShopping()
    {
        yield return new WaitForSeconds(1f);
        _actualUser.cameraControl.ChangeToInitialCamera();
        _actualUser.EndShopping();
        _actualUser = null;
        yield return new WaitForSeconds(1);
        GameManager.Instance.canPause = true;
        OnUsing(false);
    }
    
    private void Update()
    {
        if (_actualUser == null) return; //No hacer nada en caso de que no tenga usuario comprando

        _view.ArtificialUpdate();
        _controller.ArtificialUpdate();
    }
}
