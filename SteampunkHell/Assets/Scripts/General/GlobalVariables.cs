using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables 
{
    //Variables bullet
    public float speed;
    public float maxDistance;
    public float damage;     //Cuanta vida va a sacar cuando pega
    public float agressiveness;     //Cuanto va a empujar cuando pega a algo

    //Variables global Enemy
    public float movementSpeed; //Velocidad de la unidad
    public float bulletSpeed; //Velocidad de las balas que spawnea

    //Variables Enemy Sniper
    public Vector2 failOffsetRange; //Esta va a ser el rango posible de distancia a la que va a apuntar del jugador. Si esta en 0 va a apuntar a su frente
    public float aimSpeed; //Esta va a ser la velocidad general en la que va a apuntar
    public float aimSpeedMultiplier; // Multiplicador del aim. Mientras mas cerca se encuentre del player, mas rapido apunta

    //Variables Enemy Explosive
    public float timeExploit;
}
