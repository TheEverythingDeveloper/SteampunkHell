using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Model : MonoBehaviour
{
    IController myController;

    public float maxHp;
    public float _hp;
    public float speed;

    public event Action<float> OnGetDmg = delegate { };
    public event Action OnDeath = delegate { };

    // Start is called before the first frame update
    void Awake()
    {
        _hp = maxHp;
        myController = new Control(this, GetComponentInChildren<View>());
        OnDeath += Death;
    }

    // Update is called once per frame
    void Update()
    {
        myController.OnUpdate();
    }

    public void TakeDmg(float dmg)
    {
        _hp -= dmg;

        OnGetDmg(_hp / maxHp);


        if (_hp < 0)
        {
            OnDeath();
        }
    }

    void Death()
    {
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
