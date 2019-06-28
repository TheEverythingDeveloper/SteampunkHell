using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Zone zoneDoor;

    Animator _animDoor;
    AudioSource _audiosrc;
    private void Start()
    {
        _audiosrc = GetComponent<AudioSource>();
        _animDoor = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            DoorsController.Instance.DoorActive(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DoorsController.Instance.DoorDeactivated();
    }

    public void OpenDoor()
    {
        if (!DoorsController.Instance.CanOpenDoor())
            return;

        DoorsController.Instance.DoorDeactivated();
        _audiosrc.Play();
        _animDoor.SetTrigger("Open");
        zoneDoor.OpenZone();
        GetComponent<BoxCollider>().enabled = false;
    }
}
