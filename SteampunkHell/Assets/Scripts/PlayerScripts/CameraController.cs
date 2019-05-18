using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera _myCam;
    private Vector3 _cameraOffset;
    Transform _bodyTransform;

    public bool limitInX;

    public float sensitivityX = 8F;
    public float sensitivityY = 8F;

    public float minimumY = -60F;
    public float maximumY = 60F;
    public float minimumX = -90F;
    public float maximumX = 90F;

    float rotationY = 0F;
    float rotationX = 0f;

    bool muteVolume;

    private void Awake()
    {
        _bodyTransform = GetComponentInChildren<Rigidbody>().gameObject.transform;
        _cameraOffset = _myCam.gameObject.transform.position - transform.position;
    }

    void Update()
    {
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        if (limitInX)
        {
            rotationX = Mathf.Clamp(rotationX, minimumX, maximumY);
        }
        _myCam.transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

        if (Input.GetKeyDown(KeyCode.M))
        {
            muteVolume = !muteVolume;
            AudioListener.pause = muteVolume;
            AudioListener.volume = muteVolume ? 0 : 1;
        }
    }

    private void LateUpdate()
    {
        _myCam.transform.position = _bodyTransform.position + _cameraOffset;
    }
}
