using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointsController : MonoBehaviour
{
    public float points;

    [Tooltip("0 = Laser, 1 = Heavy, 2 = Explosive, 3 = Zeppellin, 4 = Boss, 5 = Other")]
    public List<float> pointsTable;
    
    public void KillEnemy(Unit unitID)
    {
        ChangePoints(pointsTable[(int)unitID]);
    }

    private void ChangePoints(float amount)
    {
        points += amount;
        UIController.Instance.NewPoints(amount);
    }
}
