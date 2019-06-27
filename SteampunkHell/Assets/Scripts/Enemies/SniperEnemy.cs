using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SniperEnemy : Enemy
{
    public float distanceAttack;
    [Tooltip("Esta va a ser la distancia a la que va a apuntar del jugador. Si esta en 0 va a apuntar a su frente")]
    float _failOffset; 
    [Tooltip("Tiempo de espera entre cada vez que dispara y apunta de nuevo")]
    public float aimCD;

    float _totalAimCD;
    LineRenderer _sniperLine;
    Vector3 laserPosition; //el laser que se acerca al jugador

    protected override void Awake()
    {
        base.Awake();
        _sniperLine = GetComponent<LineRenderer>();
        _totalAimCD = aimCD;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = VariablesPointer.EnemySniperState.movementSpeed;
    }

    protected override void Reset()
    {
        base.Reset();
        _sniperLine.enabled = true;
        aimCD = _totalAimCD;

    }

    protected override void Start()
    {
        base.Start();
        _failOffset = Random.Range(VariablesPointer.EnemySniperState.failOffsetRange.x, VariablesPointer.EnemySniperState.failOffsetRange.y);
    }

    private void Update()
    {
        if (dead) return;

        var _dist = Vector3.Distance(_player.gameObject.transform.position, transform.position);

        Aim();

        if (_dist > distanceAttack)
        {
            if (_agent.isStopped)
                _agent.isStopped = false;

            _agent.SetDestination(_player.transform.position);
        }
        else
        {
            if (!_agent.isStopped)
            {
                _agent.isStopped = true;
                _rb.velocity = Vector3.zero;
            }
        }

    }

    private void Aim()
    {

        if (aimCD > 0)
        {
            aimCD -= Time.deltaTime;
        }
        else
        {
            _sniperLine.SetPosition(0, transform.position);
            float distance = Vector3.Distance(transform.position, _player.transform.position);
            float endLaserDistance = Vector3.Distance(_sniperLine.GetPosition(1), _player.transform.position);
            //Rotar al enemigo para mirar cada vez mas cerca al jugador
            laserPosition = Vector3.MoveTowards(laserPosition, _player.transform.position - Vector3.up * 0.5f
                , (VariablesPointer.EnemySniperState.aimSpeed + VariablesPointer.EnemySniperState.aimSpeedMultiplier / distance * (endLaserDistance > _failOffset ? 1 : 0.2f)) * Time.deltaTime * 100);
            transform.LookAt(laserPosition);
        }

        //Raycast para dibujar el laser
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
        {
            _sniperLine.SetPosition(1, hit.point);
        }
    }

    protected override void DeathFeedback()
    {
        _sniperLine.enabled = false;
    }

    protected override void Shoot()
    {
        aimCD = _totalAimCD;
        EnemyBulletSpawner.Instance.GetBulletAt(transform);
    }

    public static void TurnOn(SniperEnemy b)
    {
        b.gameObject.SetActive(true);
        b.Reset();
    }

    public static void TurnOff(SniperEnemy b)
    {
        b.gameObject.SetActive(false);
    }

    protected override void ReturnEnemy()
    {
        EnemySpawner.Instance.ReturnEnemy(this);
    }
}
