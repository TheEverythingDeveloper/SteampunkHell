using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraController : MonoBehaviour
{
    public Camera _myCam;
    public CinemachineVirtualCamera initialVC;
    private CinemachineBrain _CMBrain;
    private CinemachineVirtualCamera _actualVC;

    private Vector3 _cameraOffset;
    Transform _bodyTransform;
    Transform _camPivot;


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
        _CMBrain = _myCam.transform.parent.gameObject.GetComponent<CinemachineBrain>();
        _camPivot = _CMBrain.gameObject.transform.parent.gameObject.transform;
        _cameraOffset = _camPivot.position - transform.position;
        _actualVC = initialVC;
    }

    public void ChangeZoom(float newFOV)
    {
        _myCam.fieldOfView = newFOV;
        if (_CMBrain.ActiveVirtualCamera == null)
        {
            StartCoroutine(ChangeZoomFirstTime(newFOV));
            return;
        }
        _CMBrain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = newFOV;
    }

    IEnumerator ChangeZoomFirstTime(float newfov)
    {
        yield return new WaitForSeconds(0.2f);
        ChangeZoom(newfov);
    }

    /// <summary>
    /// Cambiar la virtual camera por una nueva
    /// </summary>
    /// <param name="newCamera"></param>
    public void ChangeCamera(CinemachineVirtualCamera newCamera)
    {
        _actualVC.Priority = 10;
        _actualVC = newCamera;
        _actualVC.Priority = 20;
    }

    /// <summary>
    /// Cambiar la virtual camara a la camara original del jugador.
    /// </summary>
    public void ChangeToInitialCamera()
    {
        _actualVC.Priority = 10;
        _actualVC = initialVC;
        _actualVC.Priority = 20;
    }

    void Update()
    {
        if (GameManager.paused)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        else
            Cursor.lockState = CursorLockMode.Locked;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        if (limitInX)
        {
            rotationX = Mathf.Clamp(rotationX, minimumX, maximumY);
        }
        _camPivot.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
    }

    private void LateUpdate()
    {
        _camPivot.position = _bodyTransform.position + _cameraOffset;
    }
}
