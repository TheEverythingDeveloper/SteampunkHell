using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    PlayerController _myPlayerController;

    private void Awake()
    {
        _myPlayerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGo = other.gameObject;
        if(otherGo.layer == Layers.ENEMY)
        {
            _myPlayerController.PlayerReceiveDamage(otherGo.GetComponent<IAgressive>().GetDamage(),
                (transform.position - otherGo.transform.position) * otherGo.GetComponent<IAgressive>().GetAgressiveness()
                + Vector3.up * (otherGo.GetComponent<IAgressive>().GetAgressiveness() * 1.5f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
