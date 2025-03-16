/*
 This script allows to retrieve the vector
 middle of the screen for crosshair. Major dependency
for guns
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtils : MonoBehaviour
{
    private Camera _mainCamera;
    public delegate void SendRayData(Vector3 directionFromCamera);
    public static event SendRayData OnSendRayData;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        OnSendRayData?.Invoke(SendCameraPosition());
    }

    // Send camera Transform
    public Vector3 SendCameraPosition()
    {
        return _mainCamera.transform.position;
    }
}
