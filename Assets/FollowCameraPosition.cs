using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Attaches to Camera Holder (Orientation) and sets the position of 
/// it to Camera Position inside FPS Controller.
/// </summary>
public class FollowCameraPosition : MonoBehaviour
{
    [SerializeField] private Transform cameraTransformInFPSController;
    private void Update()
    {
        transform.position = cameraTransformInFPSController.position;
    }
}
