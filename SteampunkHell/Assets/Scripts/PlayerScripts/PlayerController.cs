using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IVulnerable
{
    public float life;
    private float _totalLife;
    private MoveController _myMovementBody;
    [HideInInspector] public CameraController myCamera;
    private AudioSource _audioSrc;
    public List<AudioClip> audios;

    public float shootCD;
    float _totalShootCD;

    private void Awake()
    {
        _totalLife = life;
        _myMovementBody = GetComponent<MoveController>();
        myCamera = GetComponent<CameraController>();
        _audioSrc = GetComponent<AudioSource>();
        _totalShootCD = shootCD;
    }

    public void MakeSound(int id)
    {
        //TODO: Sin sonidos
        /*_audioSrc.clip = audios[id];
        _audioSrc.Stop();
        _audioSrc.Play();*/
    }

    private void Start()
    {
        UIController.Instance.totalHP = life;
        UIController.Instance.previousHP = life;
    }

    private void Update()
    {
        if(shootCD > 0)
        {
            shootCD -= Time.deltaTime;
        }
        else if(Input.GetMouseButton(0))
        {
            BulletSpawner.Instance.GetBulletAt(myCamera._myCam.transform);
            shootCD = _totalShootCD;
        }
    }

    public bool ReceiveDamage(float amount, Vector3 pushForce)
    {
        life -= amount;
        life = Mathf.Clamp(life, 0, _totalLife);
        ReceiveDamageFeedback(pushForce);
        if (life <= 0)
        {
            PlayerDie();
            return true;
        }
        return false;
    }

    private void PlayerDie()
    {
        Debug.Log("Murio el jugador");
        //TODO: feedback de muerte
        //TODO: hacer algo despues de morir, tipo respawnear.
    }

    private void ReceiveDamageFeedback(Vector3 pushForce)
    {
        _myMovementBody.HitPushForce(pushForce);
        UIController.Instance.ChangeHP(life);
        Debug.Log("Recibiste Daño");
    }
}
