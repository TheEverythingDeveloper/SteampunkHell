using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    public Image hudLife;

    public ParticleSystem partSystem;

    Material _mat;

    private void Awake()
    {
        _mat = GetComponentInParent<Renderer>().material;
    }

    public void UpdateHudLife(float value)
    {
        hudLife.fillAmount = value;
    }

    public void AnimDeath()
    {
        _mat.color = Color.red;
    }

    public void ParticleDeath()
    {
        partSystem.Play();
    }
}
