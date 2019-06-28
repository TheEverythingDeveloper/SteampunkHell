using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupergunFire : MonoBehaviour, IAgressive
{
    public Weapon owner;

    public float GetAgressiveness()
    {
        return owner.agressiveness;
    }

    public float GetDamage()
    {
        return owner.damage;
    }

    public void Hit() { /*TODO: Para mas feedback, avisarle al arma de que le pego a algo*/ }
}
