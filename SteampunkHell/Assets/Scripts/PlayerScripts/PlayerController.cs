using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    Model _model;
    

    public PlayerController(Model model)
    {
        _model = model;
    }

    public void ArtificialUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _model.Jump();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _model.StartStage();
        }
        if (Input.GetMouseButton(0))
        {
            _model.Shoot();
        }
        if (Input.GetMouseButtonUp(0))
        {
            _model.StopShooting();
        }

        //Seleccion de ultis
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _model.UltiActivation(Ulti.AGRESSIVE);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _model.UltiActivation(Ulti.PRECISION);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _model.UltiActivation(Ulti.REWIND);
        }
    }
}
