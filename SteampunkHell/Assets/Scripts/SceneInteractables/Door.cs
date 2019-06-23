using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public Zone zoneDoor;
    public TextMeshProUGUI textZone;
    public float price;

    private void OnTriggerEnter(Collider other)
    {
        textZone.gameObject.SetActive(true);
        textZone.text = "Price: " + price;
    }
    private void OnTriggerExit(Collider other)
    {
        textZone.gameObject.SetActive(false);
    }
}
