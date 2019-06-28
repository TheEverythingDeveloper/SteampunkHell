using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supergun : Weapon //Es tipo un lanzallamas. Mientras mantenes el click se va vaciando.
{
    public GameObject fireableArea;
    private ParticleSystem _fireParticles;
    private AudioSource _fireAudiosrc;

    public bool isActive;

    public override bool CanShoot()
    {
        return true;
    }

    protected override void Awake()
    {
        base.Awake();
        _fireParticles = fireableArea.GetComponent<ParticleSystem>();
        _fireAudiosrc = fireableArea.GetComponent<AudioSource>();
        fireableArea.GetComponent<SupergunFire>().owner = this;
    }

    public override void Shoot(bool start, float speed)
    {
        Debug.Log("se llamo con " + (start ? "apretado" : "soltado"));
        if (!isActive || !start)
        {
            Fire(start);
        }
        isActive = start;
    }

    public void Fire(bool fireUp)
    {
        fireableArea.SetActive(fireUp);
        if (!_fireAudiosrc.isPlaying)
        {
            if (fireUp)
            {
                _fireAudiosrc.Play();
                _fireParticles.Play();
            }
            else
            {
                _fireAudiosrc.Stop();
                _fireParticles.Stop();
            }
        }
    }
}
