using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public List<Gate> gatesZone;

    public void OpenZone()
    {
        for (int i = 0; i < gatesZone.Count; i++)
        {
            GateSystem.Instance.doors.Add(gatesZone[i]);
        }
    }
}
