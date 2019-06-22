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

    private void Awake()
    {
        _Instance = this;
    }
    private void Start()
    {
        //EventsManager.SubscribeToEvent(TypeOfEvent.NewWave, ActivateStage);
       /* for (int i = 0; i < doors.Count; i++)
        {
            var t = Random.Range(0, texturesDoor.Count);
            doors[i].gameObject.GetComponent<Renderer>().material.SetTexture("_Texture", texturesDoor[t]);
        }*/
    }

    public void ActivateStage(params object[] parameters)
    {
        //TODO: Activar puertas
        for (int i = 0; i < doors.Count; i++)
        {
            doors[i].DoorClosed();
        }
        for (int i = 0; i < doorsActive.Count; i++)
        {
            doorsActive[i].DoorActive();
        }
        doorsActive = new List<Gate>();
    }
}
