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
    private ShopView _view;
    private ShopAudioController _audioControl;

    private ShopItem _selectedItem;
    public int actualSelection;

    public event Action<bool> OnTrigger = delegate { }; //true = enter, false = exit
    public event Action<bool> OnUsing = delegate { };
    public event Action<bool> OnBuy = delegate { }; // true = lo compro, false = no tenia suficiente plata
    public event Action<int> OnSelectItem = delegate { };

    private void Awake()
    {
        _shopMng = transform.parent.GetComponent<ShopsManager>();
        _view = GetComponent<ShopView>();
        _controller = new ShopControl(this);

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

        OnSelectItem += _view.Select;
        OnSelectItem += _audioControl.Select;
    }

    public void TryBuy()
    {
        //TODO: if(selectedItem.price > plataActual) OnBuy(false); else => OnBuy(true);
        OnBuy(true);
    }

    public void SelectItem(int ID)
    {
        if (ID < 0 || ID > _shopMng.shopItems.Count) return;

        OnSelectItem(ID);
    }

    public void PlayerOnTrigger(bool on)
    {
        OnTrigger(on);
    }

    public void PlayerEnter(Model model)
    {
        _actualUser = model;
        _actualUser.cameraControl.ChangeCamera(shopCM);
        GameManager.Instance.canPause = false;
        OnUsing(true);
    }

    public IEnumerator ExitShopping()
    {
        _actualUser.cameraControl.ChangeToInitialCamera();
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
