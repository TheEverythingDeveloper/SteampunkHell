using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeavyEnemy : Enemy
{
    bool attackActive;
    public Transform spawnBullets;
    public int amountBullets;
    int _totalAmountBullets;
    Animator _anim;
    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = VariablesPointer.EnemyHeavyState.speed;
        _anim = GetComponentInChildren<Animator>();
    }
    protected override void Start()
    {
        base.Start();
        _totalAmountBullets = amountBullets;
    }
    private void Update()
    {
        if (!attackActive)
        {
            var _dist = Vector3.Distance(_player.gameObject.transform.position, transform.position);

            if (_dist < 10)
            {
                if (!_agent.isStopped)
                {
                    Debug.Log(_rb.velocity);
                    _agent.isStopped = true;
                    _rb.velocity = Vector3.zero;
                }
                attackActive = true;
                StartCoroutine(AttackRobot());
            }
            else
            {
                if(_agent.isStopped)
                    _agent.isStopped = false;

                Chase();
            }
        }

    }
    IEnumerator AttackRobot()
    {
        var rotSpawner = 180 / _totalAmountBullets;
        _anim.SetBool("AttackActive", true);
        yield return new WaitForSeconds(2);
        for (int i = 0; i < amountBullets; i++)
        {
            spawnBullets.localRotation = Quaternion.Euler(new Vector3(0, i * rotSpawner + 90, 0));
            Shoot();
            yield return new WaitForSeconds(shootCd);
        }
        _anim.SetBool("AttackActive", false);
        attackActive = false;
    }

    void Chase()
    {
        //transform.forward = _player.gameObject.transform.position - transform.position;
        _agent.SetDestination(_player.transform.position);
    }
    protected override void DeathFeedback()
    {
        EnemySpawner.Instance.ReturnEnemy(this);
    }

    protected override void Shoot()
    {
        EnemyBulletSpawner.Instance.GetBulletAt(spawnBullets);
    }

    public static void TurnOn(HeavyEnemy b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(HeavyEnemy b)
    {
        b.gameObject.SetActive(false);
    }

}
