using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))] [RequireComponent(typeof(PlayerShootController))]
public class PlayerStrategyController : MonoBehaviour
{
    [SerializeField]private PlayerStrategy actualStrategy;

    private PlayerStrategy _normalStrategy;
    private PlayerStrategy _agressiveUltimateStrategy;
    private PlayerStrategy _precisionUltimateStrategy;
    private PlayerStrategy _rewindUltimateStrategy;

    public Weapon actualWeapon;

    private List<PlayerStrategy> _allStrategies = new List<PlayerStrategy>();

    [HideInInspector] public PlayerShootController shootControl;
    [HideInInspector] public MoveController moveControl;

    private void Awake()
    {
        shootControl = GetComponent<PlayerShootController>();
        moveControl = GetComponent<MoveController>();

        _normalStrategy = new NormalStrategy(this, 20, 70, 2, 5, 0.35f);
        _agressiveUltimateStrategy = new AgressiveUltimate(this, 30, 50, 5, 8, 0.2f);
        _precisionUltimateStrategy = new PrecisionUltimate(this, 80, 70, 10, 15, 0.5f);
        _rewindUltimateStrategy = new RewindUltimate(this, 20, 70, 3, 5, 0.2f);

        _allStrategies.Add(_normalStrategy);
        _allStrategies.Add(_agressiveUltimateStrategy);
        _allStrategies.Add(_precisionUltimateStrategy);
        _allStrategies.Add(_rewindUltimateStrategy);
        ChangeStrategy(Strategy.Normal);
    }

    public void UltiActivation(Ulti ultiID, bool on) //ULTI TO STRATEGY CONVERSOR
    {
        if (on)
        {
            switch (ultiID)
            {
                case Ulti.AGRESSIVE:
                    ChangeStrategy(Strategy.AgressiveUlti);
                    break;
                case Ulti.PRECISION:
                    ChangeStrategy(Strategy.PrecisionUlti);
                    break;
                case Ulti.REWIND:
                    ChangeStrategy(Strategy.RewindUlti);
                    break;
            }
        }
        else
        {
            ChangeStrategy(Strategy.Normal);
        }
    }

    public void ChangeStrategy(Strategy strategyID)
    {
        actualStrategy = _allStrategies[(int)strategyID];
        actualStrategy.Enter();
    }

    public bool CanShoot()
    {
        return actualWeapon.CanShoot();
    }

    public void Reload()
    {
        actualWeapon.Reload();
    }

    public void Shoot()
    {
        actualStrategy.Shoot();
    }

    public void ShootCallBack(float speed)
    {
        actualWeapon.Shoot(speed);
    }

    public void Jump()
    {
        actualStrategy.Jump();
    }

    public void Move()
    {
        actualStrategy.Move();
    }
}

public enum Strategy
{
    Normal,
    AgressiveUlti,
    PrecisionUlti,
    RewindUlti
}
