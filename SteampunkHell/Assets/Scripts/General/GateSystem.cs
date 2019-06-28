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

    public List<Gate> doors;
    public List<Gate> doorsActive;// = new List<Gate>();
    public List<Texture2D> texturesDoor;
    public List<Transform> zepellinSpawnPoints = new List<Transform>();

    private void Awake()
    {
        _Instance = this;
    }
    private void Start()
    {
        //EventsManager.SubscribeToEvent(TypeOfEvent.NewWave, ActivateStage);
        EventsManager.SubscribeToEvent(TypeOfEvent.FinishWave, ClosedDoors);
    }
    public void ClosedDoors(params object[] parameters)
    {
        for (int i = 0; i < doorsActive.Count; i++)
        {
            doorsActive[i].DoorClosed();
        }
            doorsActive = new List<Gate>();
    }
    public void ActivateStage(params object[] parameters)
    {
        //TODO: Activar puertas
        for (int i = 0; i < doorsActive.Count; i++)
        {
            doorsActive[i].DoorActive();
        }

    }
}
