using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    Model _model;

    IVulnerable _owner;
    public LayerMask IAgressiveLayers;
    public LayerMask interactableLayers;

    [SerializeField] private InteractableMachine _actualInteractableMachine;

    private void Awake()
    {
        _owner = GetComponent<IVulnerable>();
        _model = GetComponent<Model>();
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
        if (((1 << otherGo.layer) & interactableLayers) != 0)
        {
            _model.CanShop(true);
            _actualInteractableMachine = otherGo.GetComponent<InteractableMachine>();
            _actualInteractableMachine.PlayerOnTrigger(_model,true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherGo = other.gameObject;
        if (((1 << otherGo.layer) & interactableLayers) != 0)
        { 
            _actualInteractableMachine.PlayerOnTrigger(_model,false);
            _actualInteractableMachine = null;
            _model.CanShop(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(_actualInteractableMachine != null)
            {
                _actualInteractableMachine.PlayerEnter(_model);
                _model.StartShopping();
            }
        }
    }
}
