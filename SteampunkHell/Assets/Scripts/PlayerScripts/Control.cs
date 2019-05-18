using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : IController
{
    Model _m;

    public Control(Model m, View v)
    {
        _m = m;

        _m.OnGetDmg += v.UpdateHudLife;
        _m.OnDeath += v.AnimDeath;
        _m.OnDeath += v.ParticleDeath;
    }


    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            _m.TakeDmg(Random.Range(30f, 70f));
        }
    }
    
}
