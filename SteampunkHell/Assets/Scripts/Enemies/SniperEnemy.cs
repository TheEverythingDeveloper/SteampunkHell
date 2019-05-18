using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperEnemy : Enemy
{
    [Tooltip("Esta va a ser la distancia a la que va a apuntar del jugador. Si esta en 0 va a apuntar a su frente")]
    public float failOffset;
    [Tooltip("Esta va a ser la velocidad general en la que va a apuntar")]
    public float aimSpeed;
    [Tooltip("Multiplicador del aim. Mientras mas cerca se encuentre del player, mas rapido apunta")]
    public float aimSpeedMultiplier;

    LineRenderer _sniperLine;

    protected override void Awake()
    {
        base.Awake();
        _sniperLine = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Aim();
    }

    private void Aim()
    {
        _sniperLine.SetPosition(0, transform.position);
        Vector3 sniperLineFinalPos = _sniperLine.GetPosition(1);
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance > failOffset)
        {
            //Conseguir la nueva posicion del final del laser
            Vector3 laserNewPosition = Vector3.MoveTowards(sniperLineFinalPos, (_player.transform.position - transform.position).normalized * 0.1f,
                 aimSpeed + aimSpeedMultiplier / distance);

            //Setear la posicion del final del laser[1]
            _sniperLine.SetPosition(1, laserNewPosition);
        }
        transform.LookAt(sniperLineFinalPos);
    }

    IEnumerator ShootCoroutine()
    {
        Shoot();
        yield return new WaitForSeconds(shootCd);
    }

    protected override void DeathFeedback()
    {

    }

    protected override void Shoot()
    {

    }
}
