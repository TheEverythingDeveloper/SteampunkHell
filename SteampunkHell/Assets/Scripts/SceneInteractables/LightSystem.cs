using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSystem : MonoBehaviour
{
    public List<Light> myLights = new List<Light>();
    private AudioSource _audiosrc;

    private void Awake()
    {
        _audiosrc = GetComponent<AudioSource>();
    }

    private void Start()
    {
        EventsManager.SubscribeToEvent(TypeOfEvent.FinishWave, ActivateLights);
        EventsManager.SubscribeToEvent(TypeOfEvent.NewWave, DeactivateLights);
    }

    public void ActivateLights(params object[] parameters)
    {
        _audiosrc.Play();
        foreach (var item in myLights)
        {
            item.enabled = true;
        }
    }

    public void DeactivateLights(params object[] parameters)
    {
        _audiosrc.Stop();
        foreach (var item in myLights)
        {
            item.enabled = false;
        }
    }
}
