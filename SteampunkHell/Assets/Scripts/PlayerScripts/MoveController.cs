using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveController : MonoBehaviour
{
    [SerializeField] private float _groundedSpeed = 0.1f;
    [SerializeField] private float _inAirSpeed = 0.1f;
    [SerializeField] private float _jumpSpeed = 6;
    [Range(1, 3)]
    [SerializeField] private float _gravityMultiplier = 2;
    private bool _isGrounded;
    public bool canMove;
    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        IsGrounded();
    }

    public void HitPushForce(float amount, Vector3 pushForce)
    {
        _rb.AddForce(pushForce, ForceMode.Impulse);
    }

    public void TryJump()
    {
        if (!_isGrounded) return;

        GetComponent<PlayerAudioController>().MakeSound(0);
        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.AddForce(transform.up * _jumpSpeed, ForceMode.Impulse);
    }

    private void IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 2f))
        {
            _isGrounded = true;
            Debug.DrawRay(transform.position, -transform.up * hit.distance, Color.green);
        }
        else
        {
            _isGrounded = false;
            Debug.DrawRay(transform.position, -transform.up * hit.distance, Color.red);
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.paused) return;

        var newForward = GetComponent<CameraController>()._myCam.transform.forward;
        transform.forward = new Vector3(newForward.x, 0, newForward.z);

        Movement();
    }

    private void Movement()
    {
        if (!canMove) return;

        if (_isGrounded)
        {
            transform.position += transform.right * Input.GetAxis("Horizontal") * _groundedSpeed;
            transform.position += transform.forward * Input.GetAxis("Vertical") * _groundedSpeed;
        }
        else //air movement, slowly
        {
            transform.position += transform.right * Input.GetAxis("Horizontal") * _inAirSpeed;
            transform.position += transform.forward * Input.GetAxis("Vertical") * _inAirSpeed;
            _rb.AddForce((Physics.gravity * _gravityMultiplier) - Physics.gravity);
        }
    }
}
