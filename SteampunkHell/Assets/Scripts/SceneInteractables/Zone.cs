using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public List<Gate> gatesZone;
    public List<Transform> zeppelinSpawnPoints;

    public void OpenZone()
    {
        for (int i = 0; i < gatesZone.Count; i++)
        {
            GateSystem.Instance.doors.Add(gatesZone[i]);
        }
        for (int i = 0; i < zeppelinSpawnPoints.Count; i++)
        {
            GateSystem.Instance.zepellinSpawnPoints.Add(zeppelinSpawnPoints[i]);
        }
    }
}
