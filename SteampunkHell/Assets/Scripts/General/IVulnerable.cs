using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVulnerable
{
    bool ReceiveDamage(float amount, Vector3 pushForce);
}
