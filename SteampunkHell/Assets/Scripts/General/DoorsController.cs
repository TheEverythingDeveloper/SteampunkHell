using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorsController : MonoBehaviour
{
    public static DoorsController Instance
    {
        get
        {
            return _Instance;
        }
    }
    private static DoorsController _Instance;

    public TextMeshProUGUI textZone;
    public int price;
    public AudioSource newZoneClip;
    Model player;
    private void Awake()
    {
        _Instance = this;
        player = FindObjectOfType<Model>();
    }
    public void DoorActive(Door d)
    {
        textZone.gameObject.SetActive(true);
        textZone.text = "Price: " + price + ". Press 'B' to open it";
        player.inDoor = d;
    }

    public void DoorDeactivated()
    {
        textZone.gameObject.SetActive(false);
        player.inDoor = null;
    }
    public bool CanOpenDoor()
    {
        if(player.pointsControl.points >= price)
        {
            player.pointsControl.ChangePoints(-price);
            newZoneClip.Play();
            return true;
        }
        return false;
    }
}
