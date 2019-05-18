using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEnemy : Enemy
{
    [Tooltip("Esta va a ser la distancia a la que va a apuntar del jugador. Si esta en 0 va a apuntar a su frente")]
    public float failOffset;
    [Tooltip("Esta va a ser el rango posible de distancia a la que va a apuntar del jugador. Si esta en 0 va a apuntar a su frente")]
    public Vector2 failOffsetRange;
    [Tooltip("Esta va a ser la velocidad general en la que va a apuntar")]
    public float aimSpeed;
    [Tooltip("Multiplicador del aim. Mientras mas cerca se encuentre del player, mas rapido apunta")]
    public float aimSpeedMultiplier;

    EnemyBulletSpawner _bulletSpawner;
    LineRenderer _sniperLine;
    Vector3 laserPosition; //el laser que se acerca al jugador

    protected override void Awake()
    {
        base.Awake();
        _sniperLine = GetComponent<LineRenderer>();
        _bulletSpawner = GetComponentInParent<EnemyBulletSpawner>();
    }

    protected override void Start()
    {
        base.Start();
        failOffset = Random.Range(failOffsetRange.x, failOffsetRange.y);
    }

    private void Update()
    {
        Aim();
    }

    private void Aim()
    {
        _sniperLine.SetPosition(0, transform.position);
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        float endLaserDistance = Vector3.Distance(_sniperLine.GetPosition(1), _player.transform.position);
        //Rotar al enemigo para mirar cada vez mas cerca al jugador
        laserPosition = Vector3.MoveTowards(laserPosition, _player.transform.position - Vector3.up * 0.5f
            , aimSpeed + aimSpeedMultiplier / distance * (endLaserDistance > failOffset ? 1 : 0.2f));
        transform.LookAt(laserPosition);

        //Raycast para dibujar el laser
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
        {
            _sniperLine.SetPosition(1, hit.point);
        }
    }

    protected override void DeathFeedback()
    {

    }

    protected override void Shoot()
    {
        _bulletSpawner.GetBulletAt(transform);
    }
}
