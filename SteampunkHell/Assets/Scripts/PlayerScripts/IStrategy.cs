using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStrategy 
{
    void Enter();

    void Shoot();

    void Move();

    void Jump();
}
