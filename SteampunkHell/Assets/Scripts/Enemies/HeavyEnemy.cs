using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : Enemy
{
    bool attackActive;
    public GameObject robotAttack;
    public int countBullets;
    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Start()
    {
        base.Start();

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
        robotAttack.transform.position = Vector3.Lerp(robotAttack.transform.position, new Vector3(0, 1.5f, 0), 2f);
        yield return new WaitForSeconds(3);
        var rotSpawner = 180 / countBullets;
        for (int i = 0; i < countBullets; i++)
        {
            robotAttack.transform.Rotate(Vector3.up, i * rotSpawner);
            EnemyBulletSpawner.Instance.GetBulletAt(robotAttack.transform);
        }
        yield return new WaitForSeconds(2);
        robotAttack.transform.position = Vector3.Lerp(robotAttack.transform.position, Vector3.zero, 2f);
        yield return new WaitForSeconds(3);
        attackActive = false;
    }
    void Chase()
    {
        transform.forward = _player.gameObject.transform.position - transform.position;
        transform.position += transform.forward * VariablesPointer.EnemyExplosiveState.speed * Time.deltaTime;
    }
    protected override void DeathFeedback()
    {

    }

    protected override void Shoot()
    {

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
