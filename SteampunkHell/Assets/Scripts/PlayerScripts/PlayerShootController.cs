using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    public float shootCD;
    float _totalShootCD;
    [HideInInspector] public CameraController myCamera;
    private PlayerStrategyController _strategy;

    private void Awake()
    {
        _strategy = GetComponent<PlayerStrategyController>();
        _totalShootCD = shootCD;
        myCamera = GetComponent<CameraController>();
    }

    public void ChangeShootCD(float newCD)
    {
        _totalShootCD = newCD;
        shootCD = newCD;
    }

    private void Update()
    {
        if (shootCD > 0)
        {
            shootCD -= Time.deltaTime;
            shootCD = Mathf.Clamp(shootCD, 0, 1000);
        }
    }

    public void Shoot()
    {
        if (shootCD > 0) return;

        if (!_strategy.CanShoot())
        {
            _strategy.Reload();
        }
        else
        {
            _strategy.Shoot();
            shootCD = _totalShootCD;
        }
    }
}
