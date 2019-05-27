using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    Model _model;

    IVulnerable _owner;
    public LayerMask IAgressiveLayers;

    private void Awake()
    {
        _owner = GetComponent<IVulnerable>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGo = other.gameObject;
        if(((1 << otherGo.layer) & IAgressiveLayers) != 0)
        {
            otherGo.GetComponent<IAgressive>().Hit();
            _owner.ReceiveDamage(otherGo.GetComponent<IAgressive>().GetDamage(),
                (transform.position - otherGo.transform.position) * otherGo.GetComponent<IAgressive>().GetAgressiveness()
                + Vector3.up * (otherGo.GetComponent<IAgressive>().GetAgressiveness()) * 0.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
