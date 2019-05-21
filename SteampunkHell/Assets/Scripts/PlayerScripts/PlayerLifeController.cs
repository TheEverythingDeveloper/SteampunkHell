using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeController : MonoBehaviour, IVulnerable
{
    public float life;
    private float _totalLife;
    private MoveController _myMovementBody;
    private Model model;

    private void Awake()
    {
        _totalLife = life;
        _myMovementBody = GetComponent<MoveController>();
        model = GetComponent<Model>();
    }
      
    private void Start()
    {
        UIController.Instance.totalHP = life;
        UIController.Instance.previousHP = life;
    }

    public bool ReceiveDamage(float amount, Vector3 pushForce)
    {
        life -= amount;
        life = Mathf.Clamp(life, 0, _totalLife);
        model.ReceiveDamage(life, pushForce);
        if (life <= 0)
        {
            model.Death();
            return true;
        }
        return false;
    }

    public void PlayerDie()
    {
        Debug.Log("Murio el jugador");
        //TODO: feedback de muerte
        //TODO: hacer algo despues de morir, tipo respawnear.
    }
}
