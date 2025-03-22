
using System;
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
    [SerializeField] private float leanAngle = 17.0f;
    [SerializeField] private float rotateCameraAt = 17.0f;
    [SerializeField] private float currentLeanAngle = 0.0f;
    [SerializeField] private float leanSpeed = 1.0f;
    [SerializeField] private float leanTimeElapsed = 0.0f;
    [SerializeField] private float leanTimeDuration = 0.5f;
    [SerializeField] private bool isLeaning = false;
    private float xRotation;
    private float yRotation;

    // alter player's rotation y according to the camera
    [SerializeField] private Transform playerOrientation;

    // for disabling input
    private bool isCameraControlEnabled = true;
    // debug
    public bool showCameraDebugUI = false;

    private void OnEnable()
    {
        PlayerMovement.OnCameraLeanTowards += LeanCameraTowards;
        PlayerMovement.OnResetCamera += ResetRotation;
        PlayerHealth.OnDeathEvent += DisableCameraControl;
        WallRun.OnStopCameraLerp += StopLeaning;
    }

    private void OnDisable()
    {
        PlayerMovement.OnCameraLeanTowards -= LeanCameraTowards;
        PlayerMovement.OnResetCamera -= ResetRotation;
        WallRun.OnStopCameraLerp -= StopLeaning;
        PlayerHealth.OnDeathEvent -= DisableCameraControl;
    }

    private void StopLeaning()
    {
        isLeaning = false;
        
    }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    [ContextMenu("Disable Camera Control")]
    public void DisableCameraControl()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCameraControlEnabled = false;
    }

    [ContextMenu("Enable Camera Control")]
    public void EnableCameraControl()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCameraControlEnabled = true;
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
        transform.rotation = Quaternion.Euler(xRotation, yRotation, transform.rotation.z);
        playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    #endregion
    private void Update()
    {
        if (!isCameraControlEnabled) return;
        SetCameraRotation();
        SetCameraLean();
    }


    // slowly lerps the camera's z axis to the desired value
    private void SetCameraLean()
    {
        leanTimeElapsed += Time.deltaTime;
        float leanCompletedPercentage = leanTimeElapsed / leanTimeDuration;
        if (leanCompletedPercentage == 1)
        {
            isLeaning = false;
        }
        if (isLeaning) currentLeanAngle = Mathf.Lerp(0, rotateCameraAt, leanCompletedPercentage);
        if (!isLeaning) currentLeanAngle = Mathf.Lerp(currentLeanAngle, 0, leanCompletedPercentage);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, currentLeanAngle);
    }

    public void LeanCameraTowards(string direction)
    {
        if (direction == "left")
        {
            rotateCameraAt = leanAngle * -1;
            isLeaning = true;
            ResetTimer();
            
        }

        else if (direction == "right")
        {
            rotateCameraAt = leanAngle;
            isLeaning = true;
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        leanTimeElapsed = 0.0f;
    }

    [ContextMenu("Reset Rotation")]
    public void ResetRotation()
    {
        ResetTimer();
        isLeaning = false;
        rotateCameraAt = 0.0f;
    }
}

