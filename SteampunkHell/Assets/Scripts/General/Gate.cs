using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Transform spawnerEnemy;
    Animator _anim;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        FindObjectOfType<GateSystem>().doors.Add(this);
    }

    public void DoorActive()
    {
        GetComponentInChildren<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
        _anim.SetBool("Open", true);
    }
    public void DoorClosed()
    {
        //GetComponent<AudioSource>().Play();
        _anim.SetBool("Open", false);
    }
    public Transform spawnEnemy()
    {
        FindObjectOfType<GateSystem>().doorsActive.Add(this);
        return spawnerEnemy;
    }
}
