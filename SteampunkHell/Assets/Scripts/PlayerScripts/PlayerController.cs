using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Model _model;

    private void Awake()
    {
        _model = GetComponent<Model>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _model.Jump();
        }
        if (Input.GetMouseButton(0))
        {
            _model.Shoot();
        }
        if (Input.GetMouseButtonUp(0))
        {
            _model.StopShooting();
        }

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
