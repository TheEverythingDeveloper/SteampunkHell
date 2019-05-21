using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Model _model;

    private void Awake()
    {
        _model = GetComponent<Model>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _model.Jump();
        }
        if (Input.GetMouseButton(0))
        {
            _model.Shoot();
        }
    }
}
