using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopControl
{
    ShopMachine _model;

    public ShopControl(ShopMachine model)
    {
        _model = model;
    }

    public void ArtificialUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _model.StartCoroutine(_model.ExitShopping());
        }
        else if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            _model.TryBuy();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            _model.SelectItem(false);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            _model.SelectItem(true);
        }
    }
}
