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
    AudioSource _audiosrc;
    public AudioClip newZoneClip;

    private void Awake()
    {
        _audiosrc = GetComponent<AudioSource>();
    }

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

    public IEnumerator OpenDoorCoroutine()
    {
        OpenDoor();
        yield return new WaitForSeconds(5f);
        _audiosrc.clip = newZoneClip;
        _audiosrc.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        textZone.gameObject.SetActive(false);
        other.GetComponentInParent<Model>().inDoor = null;
    }

    public void OpenDoor()
    {
        GameManager.Instance.openDoor.Play();
        _audiosrc.Play();
        for (int i = 0; i < _animationsDoor.Length; i++)
        {
            _animationsDoor[i].SetTrigger("Open");
        }
        zoneDoor.OpenZone();
        GetComponent<BoxCollider>().enabled = false;
        textZone.gameObject.SetActive(false);
    }
}
