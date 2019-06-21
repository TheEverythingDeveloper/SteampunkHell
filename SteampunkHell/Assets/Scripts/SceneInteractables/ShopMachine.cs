using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShopMachine : MonoBehaviour
{
    private Model _actualUser;
    public CinemachineVirtualCamera shopCM;

    public void PlayerEnter(Model model)
    {
        Debug.Log("el player entro a comprar");
        _actualUser = model;
        _actualUser.cameraControl.ChangeCamera(shopCM);
        GameManager.Instance.canPause = false;
        
    }

    IEnumerator ExitShopping()
    {
        Debug.Log("el player paro de comprar");
        _actualUser.cameraControl.ChangeToInitialCamera();
        _actualUser = null;
        yield return new WaitForSeconds(1);
        GameManager.Instance.canPause = true;
    }
    
    private void Update()
    {
        if (_actualUser == null) return; //No hacer nada en caso de que no tenga usuario comprando

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(ExitShopping());
        }
    }
}
