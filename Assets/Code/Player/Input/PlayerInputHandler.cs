#define DebugMode

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Handles Player Input.
/// 
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{

    [SerializeField] private InputMaster _inputMaster;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CameraControlPlayer _cameraControl;
    [SerializeField] private PlayerGrapple _playerGrapple;
    [SerializeField] private PlayerGun _playerGun;
    [SerializeField] private IThrowable _throwable;

    private void Awake()
    {
        SetAllInputs();
    }

    private void SetAllInputs()
    {
        _inputMaster = new InputMaster();
        _playerMovement = transform.GetComponentInChildren<PlayerMovement>();
        _cameraControl = transform.GetComponentInChildren<CameraControlPlayer>();
        _playerGrapple = transform.GetComponentInChildren<PlayerGrapple>();
        _playerGun = transform.GetComponentInChildren<PlayerGun>();
        _throwable = transform.GetComponentInChildren<IThrowable>();
        _inputMaster.Player.Move.performed += context => OnMovePerformed(context);
        _inputMaster.Player.Move.canceled += context => OnMoveCanceled();
        _inputMaster.Player.Jump.performed += context => OnJumpPressed();
        _inputMaster.Player.Jump.canceled += context => OnJumpCancelled();
        _inputMaster.Player.Look.performed += context => OnMouseLook(context);
        _inputMaster.Player.Look.canceled += context => OnMouseLookCanceled();
        _inputMaster.Player.Fire.performed += context => OnFirePressed();
        _inputMaster.Player.Fire.canceled += context => OnFireCancelled();
        _inputMaster.Player.Sprint.performed += context => OnSprintPerformed();
        _inputMaster.Player.Sprint.canceled += context => OnSprintCanceled();
        _inputMaster.Player.Grapple.performed += context => OnGrapplePerformed();
        _inputMaster.Player.Grapple.canceled += context => OnGrappleCanceled();
    }

    private void Start()
    {
    }

    private void OnGrapplePerformed()
    {
        // future:-> Migrate to GrappleCommand.cs
        _playerGrapple.SetIsGrappleKeyPressed(true);
    }

    private void OnGrappleCanceled()
    {
        // future:-> Migrate to GrappleCommand.cs
        _playerGrapple.SetIsGrappleKeyPressed(false);
    }

    private void OnEnable()
    {
        SetAllInputs();
        _inputMaster.Enable();
    }

    private void OnDisable()
    {

        _inputMaster?.Disable();
    }

    private void OnSprintCanceled()
    {
        SprintCommand sprintCommand = new SprintCommand(_playerMovement);
        sprintCommand.Canceled();
    }

    private void OnSprintPerformed()
    {
        SprintCommand sprintCommand = new SprintCommand(_playerMovement);
        sprintCommand.Execute();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        MoveCommand moveCommand = new MoveCommand(_playerMovement, ctx.ReadValue<Vector2>());
        moveCommand.Execute();
    }

    private void OnMoveCanceled()
    {
        MoveCommand moveCommand = new MoveCommand(_playerMovement, Vector2.zero);
        moveCommand.Canceled();
    }

    private void OnJumpPressed()
    {
        JumpCommand jumpCommand = new JumpCommand(_playerMovement);
        jumpCommand.Execute();
    }

    private void OnJumpCancelled()
    {
        JumpCommand jumpCommand = new JumpCommand(_playerMovement);
        jumpCommand.Canceled();
    }
    private void OnMouseLook(InputAction.CallbackContext ctx)
    {

        MouseLookCommand mouseLookCommand = new MouseLookCommand(_cameraControl, ctx.ReadValue<Vector2>());
        mouseLookCommand.Execute();
    }

    private void OnMouseLookCanceled()
    {
        MouseLookCommand mouseLookCommand = new MouseLookCommand(_cameraControl, Vector2.zero);
        mouseLookCommand.Canceled();
    }
    private void OnFireCancelled()
    {
        FireCommand fireCommand = new FireCommand(_throwable, _playerGun);
        fireCommand.Canceled();
    }

    private void OnFirePressed()
    {
        FireCommand fireCommand = new FireCommand(_throwable,_playerGun);
        fireCommand.Execute();
    }
}
