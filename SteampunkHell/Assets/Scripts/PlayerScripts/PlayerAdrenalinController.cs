using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdrenalinController : MonoBehaviour
{
    public float adrenalin;

    [Tooltip("0 = Laser, 1 = Heavy, 2 = Explosive, 3 = Zeppellin, 4 = Boss, 5 = Other")]
    public List<float> adrenalinTable;

    private void Start()
    {
        ChangeAdrenalin(0);
    }

    public void KillEnemy(Unit unitID)
    {
        ChangeAdrenalin(adrenalinTable[(int)unitID]);
    }

    private void ChangeAdrenalin(float amount)
    {
        adrenalin += amount;
        adrenalin = Mathf.Clamp(adrenalin, 0, 100);
        UIController.Instance.ChangeAdrenalinBar(adrenalin);
    }
}
