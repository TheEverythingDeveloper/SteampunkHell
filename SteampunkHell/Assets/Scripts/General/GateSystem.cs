using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSystem : MonoBehaviour
{
    public static GateSystem Instance
    {
        get
        {
            return _Instance;
        }
    }

    private static GateSystem _Instance;

    public List<Transform> doors;
    public List<GameObject> doorsActive = new List<GameObject>();
    private void Awake()
    {
        _Instance = this;
    }
    private void Start()
    {
        EventsManager.SubscribeToEvent(TypeOfEvent.NewWave, ActivateStage);
    }

    public void ActivateStage(params object[] parameters)
    {
        //TODO: Activar puertas
        doorsActive = new List<GameObject>();
    }
}
