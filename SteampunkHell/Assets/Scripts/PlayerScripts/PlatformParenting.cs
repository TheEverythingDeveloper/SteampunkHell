using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformParenting : MonoBehaviour, IPlatformParenting
{
    [Tooltip("El gameobject que esta siendo hijo de este por la platform parenting interface")]
    public GameObject parentingTarget = null;
    public LayerMask _myLayerMask;
    [HideInInspector]
    public bool debug;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _myLayerMask)
        {
            if (debug)
                Debug.Log("Empezo un Platform Parent con "+other.name);
            parentingTarget = other.gameObject;
            parentingTarget.transform.parent.gameObject.transform.parent = gameObject.transform;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject == parentingTarget)
        {
            if (debug)
                Debug.Log("Platform Parenting... con "+other.name);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == parentingTarget)
        {
            if (debug)
                Debug.Log("Termino un Platform Parent con "+other.name);
            parentingTarget.transform.parent.gameObject.transform.parent = null;
        }
    }
}
