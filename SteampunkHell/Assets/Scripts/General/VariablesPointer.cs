using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesPointer 
{
    public static readonly GlobalVariables PlayerBulletState = new GlobalVariables
    {
        bulletSpeed = 80,
        maxDistance = 70,
        damage = 10,
        agressiveness = 3
    };

    public static readonly GlobalVariables EnemyBulletState = new GlobalVariables
    {
        bulletSpeed = 20,
        maxDistance = 70,
        damage = 10,
        agressiveness = 3
    };

    public static readonly GlobalVariables EnemySniperState = new GlobalVariables
    {
        movementSpeed = 10,
        bulletSpeed = 5,
        agressiveness = 3,
        failOffsetRange = new Vector2(0,7),
        aimSpeed = 0.015f,
        aimSpeedMultiplier = 2.4f
    };

    public static readonly GlobalVariables EnemyExplosiveState = new GlobalVariables
    {
        damage = 10,
        speed = 3,
        maxDistance = 5,
        timeExploit = 2
    };
}
