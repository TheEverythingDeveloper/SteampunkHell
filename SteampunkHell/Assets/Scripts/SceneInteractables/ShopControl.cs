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
            _model.SelectItem(_model.actualSelection - 1);
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            _model.SelectItem(_model.actualSelection + 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _model.SelectItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _model.SelectItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _model.SelectItem(2);
        }
    }
}
