using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                attackActive = true;
                StartCoroutine(AttackRobot());
            }
            else
                Chase();
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
        transform.forward = _player.gameObject.transform.position - transform.position;
        transform.position += transform.forward * VariablesPointer.EnemyHeavyState.speed * Time.deltaTime;
    }
    protected override void DeathFeedback()
    {

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

    protected override void ReturnEnemy()
    {
        EnemySpawner.Instance.ReturnEnemy(this);
    }
}
