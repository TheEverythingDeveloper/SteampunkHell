using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SniperEnemy : Enemy
{
    public float distanceAttack;
    public Transform head, spawnBullets;
    [Tooltip("Esta va a ser la distancia a la que va a apuntar del jugador. Si esta en 0 va a apuntar a su frente")]
    float _failOffset; 
    [Tooltip("Tiempo de espera entre cada vez que dispara y apunta de nuevo")]
    public float aimCD;

    float _totalAimCD;
    LineRenderer _sniperLine;
    Vector3 laserPosition; //el laser que se acerca al jugador
    Vector3 positionPlayer;
    Animator _anim;
    protected override void Awake()
    {
        base.Awake();
        _sniperLine = GetComponent<LineRenderer>();
        _totalAimCD = aimCD;
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = VariablesPointer.EnemySniperState.movementSpeed;
        _anim = GetComponent<Animator>();
    }

    protected override void Reset()
    {
        base.Reset();
        _sniperLine.enabled = true;
        aimCD = _totalAimCD;
        StartCoroutine(ShootCoroutine());

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

            _anim.SetBool("Walk", true);

            _agent.SetDestination(_player.transform.position);
        }
        else
        {
            if (!_agent.isStopped)
            {
                _agent.isStopped = true;
                _rb.velocity = Vector3.zero;
                _anim.SetBool("Walk", false);
            }
        }

    }

    private void Aim()
    {
        positionPlayer = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
        transform.LookAt(positionPlayer);

        _sniperLine.SetPosition(0, head.transform.position);
        if (aimCD > 0)
        {
            aimCD -= Time.deltaTime;
        }
        else
        {
            float distance = Vector3.Distance(transform.position, _player.transform.position);
            float endLaserDistance = Vector3.Distance(_sniperLine.GetPosition(1), _player.transform.position);
            //Rotar al enemigo para mirar cada vez mas cerca al jugador
            laserPosition = Vector3.MoveTowards(laserPosition, _player.transform.position - Vector3.up * 0.5f
                , (VariablesPointer.EnemySniperState.aimSpeed + VariablesPointer.EnemySniperState.aimSpeedMultiplier / distance * (endLaserDistance > _failOffset ? 1 : 0.2f)) * Time.deltaTime * 100);

            head.forward = _player.gameObject.transform.position - transform.position;
        }
        //Raycast para dibujar el laser
        RaycastHit hit;
        if (Physics.Raycast(head.position, head.forward, out hit, 1000f))
        {
            _sniperLine.SetPosition(1, hit.point);
        }
        head.LookAt(laserPosition);
        spawnBullets.LookAt(laserPosition);

    }

    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootCd + Random.Range(-1f, 1f));
            Shoot();
            yield return new WaitForEndOfFrame();
        }
    }

    protected override void DeathFeedback()
    {
        _sniperLine.enabled = false;
        _anim.SetBool("Dead", true);
        StartCoroutine(DeadCoroutine());
    }
    IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(5);
        EnemySpawner.Instance.ReturnEnemy(this);
    }
    protected override void Shoot()
    {
        aimCD = _totalAimCD;
        EnemyBulletSpawner.Instance.GetBulletAt(spawnBullets);
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

}
