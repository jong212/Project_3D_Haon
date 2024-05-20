using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour
{
    private CinemachineFreeLook m_MainCamera;

    void Start()
    {
        AttachCamera();
    }

    private void AttachCamera()
    {
        m_MainCamera = FindObjectOfType<CinemachineFreeLook>();
        Assert.IsNotNull(m_MainCamera, "CameraController.AttachCamera: Couldn't find gameplay freelook camera");

        if (m_MainCamera)
        {

            m_MainCamera.Follow = transform;
        }
    }



}
