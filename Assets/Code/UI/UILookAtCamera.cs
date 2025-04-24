using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Attaches to UI elements to constantly look at the player.
/// prevents player to see flipped text 
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = FindFirstObjectByType<Camera>();
    }

    private void Update()
    {
        transform.LookAt(_mainCamera.transform);
        transform.rotation.SetLookRotation(_mainCamera.transform.position);
        
    }
}
