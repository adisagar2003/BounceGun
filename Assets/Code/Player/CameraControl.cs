
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Takes care for looking around using Mouse Delta and Joystick(Future imple)..
/// </summary>
public class CameraControlPlayer : MonoBehaviour
{
    private Vector2 mouseDelta;
    private bool isTakingInput = false;

    [SerializeField] [Range(0,100)] private float sensX = 4.0f;
    [SerializeField] [Range(0, 100)] private float sensY = 2.0f;
    private float xRotation;
    private float yRotation;
    
    // alter player's rotation y according to the camera
    [SerializeField] private Transform playerTransform;

    // debug
    public bool showCameraDebugUI = false;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    #region Debug
    private void OnGUI()
    {
        if (!showCameraDebugUI) return;
        #if UNITY_EDITOR
        GUI.Label(new Rect(10, 70, 200, 200), $"xRotation: " +
            $"{xRotation} \n yRotation: " +
            $"{yRotation}\n Player Speed: " +
            $"{mouseDelta} , Is Taking Input: " +
            $"{isTakingInput}!");

        #endif
    }

    #endregion

    #region Mouse Input
    public void SetMouseLook(Vector2 value)
    {
        if (mouseDelta != Vector2.zero) isTakingInput = true;
        mouseDelta = value;

    #if DebugGame
        Debug.Log("MouseDelta" + mouseDelta);
    #endif
    }

    // Check for taking input: For input system.
    public void SetIsTakingInput(bool value)
    {
        isTakingInput = value;
    }
    #endregion


    #region Camera Control
    private void SetCameraRotation()
    {
        if (!isTakingInput) return; // do not rotate if mouse is not moving
        float lookY = mouseDelta.y * Time.deltaTime * sensY * -1;
        float lookX = mouseDelta.x * Time.deltaTime * sensX;
        yRotation += lookX;
        xRotation += lookY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        playerTransform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    #endregion
    private void Update()
    {
        SetCameraRotation();
    }
}

