using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPointsController : MonoBehaviour
{
    public int points;

    [Tooltip("0 = Laser, 1 = Heavy, 2 = Explosive, 3 = Zeppellin, 4 = Boss, 5 = Other")]
    public List<int> pointsTable;
    
    public void KillEnemy(Unit unitID)
    {
        ChangePoints(pointsTable[(int)unitID]);
    }

    private void ChangePoints(int amount)
    {
        points += amount;
        UIController.Instance.NewPoints(amount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            points += 100;
        }
    }
}
