using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosive : Enemy
{
    float _dist;
    bool _exploitActive;

    protected override void Reset()
    {
        WaveManager.Instance.AddToEnemiesActive();
        life = _totalLife;
        dead = false;
    }

    private void Update()
    {
        if (dead || _exploitActive) return;

        _dist = Vector3.Distance(_player.gameObject.transform.position, transform.position);

        if (_dist < 2)
            Shoot();
        else
            Chase();

    }
    IEnumerator Exploit()
    {
        _exploitActive = true;
        yield return new WaitForSeconds(VariablesPointer.EnemyExplosiveState.timeExploit);
        if(Vector3.Distance(_player.gameObject.transform.position, transform.position) < VariablesPointer.EnemyExplosiveState.maxDistance)
        {
            _player.ReceiveDamage(VariablesPointer.EnemyExplosiveState.damage,
                (_player.gameObject.transform.position - transform.position) * agressiveness
                + Vector3.up * (agressiveness * 1.5f));
        }
        ReturnEnemy();
        FindObjectOfType<WaveManager>().CheckEnemiesState();
        DeathFeedback();
    }
    void Chase()
    {
        transform.forward = _player.gameObject.transform.position - transform.position;
        transform.position += transform.forward * VariablesPointer.EnemyExplosiveState.speed * Time.deltaTime;
    }
    protected override void DeathFeedback()
    {
       // gameObject.SetActive(false);
    }

    protected override void Shoot()
    {
        StartCoroutine(Exploit());
    }

    public static void TurnOn(EnemyExplosive b)
    {
        b.gameObject.SetActive(true);
        b.Reset();
    }

    public static void TurnOff(EnemyExplosive b)
    {
        b.gameObject.SetActive(false);
    }

    protected override void ReturnEnemy()
    {
        EnemySpawner.Instance.ReturnEnemy(this);
    }
}
