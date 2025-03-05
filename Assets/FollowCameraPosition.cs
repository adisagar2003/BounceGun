using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 Attaches to Camera Holder and sets the position of 
it to Camera Position inside FPS Controller.
 */

public class FollowCameraPosition : MonoBehaviour
{
    [SerializeField] private Transform cameraTransformInFPSController;
    private void Update()
    {
        transform.position = cameraTransformInFPSController.position;
    }
}
