using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public Zone zoneDoor;
    public TextMeshProUGUI textZone;
    public float price;
    Animator[] _animationsDoor;

    private void Start()
    {
        _animationsDoor = GetComponentsInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            textZone.gameObject.SetActive(true);
            textZone.text = "Price: " + price + ". Press 'B' to open it";
            other.GetComponentInParent<Model>().inDoor = this;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        textZone.gameObject.SetActive(false);
        other.GetComponentInParent<Model>().inDoor = null;
    }

    public void OpenDoor()
    {
        GameManager.Instance.openDoor.Play();
        GetComponent<AudioSource>().Play();
        for (int i = 0; i < _animationsDoor.Length; i++)
        {
            _animationsDoor[i].SetTrigger("Open");
        }
        zoneDoor.OpenZone();
        GetComponent<BoxCollider>().enabled = false;
        textZone.gameObject.SetActive(false);
    }
}
