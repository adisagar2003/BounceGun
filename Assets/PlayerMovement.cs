using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float playerSpeed = 10.0f;
    [SerializeField] private float jumpForce = 100.0f;
    [SerializeField] private float maxSpeed = 7.0f;
    [SerializeField] private float airSpeed = 1.5f;
    private InputMaster inputMaster;
    private string debugString = "";

    private Rigidbody _rb;
    [SerializeField] private Transform orientation;
    private Vector2 keyboardInput;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundDrag;
    [SerializeField] private bool isReadyToJump = true;
    [SerializeField] private bool jumpInputPressed = false;
    [SerializeField] private float jumpCooldown = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Move.performed += context => GetInput(context);
        inputMaster.Player.Move.canceled += context => GetInput(context);
        inputMaster.Player.Jump.performed += context => JumpPressed();
        inputMaster.Player.Jump.canceled += context => JumpCancelled();
    }

    private void JumpCancelled()
    {
        jumpInputPressed = false;
    }

    private void JumpPressed()
    {
        jumpInputPressed = true;
        PlayerJump();
    }

    private void PlayerJump()
    {
        // when to jump
        if (isGrounded && isReadyToJump && jumpInputPressed)
        {
            isReadyToJump = false;
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputMaster.Player.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Player.Disable();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerJump();
        MovePlayer();
    }

    private void Update()
    {
        SetIsGrounded();
        SpeedControl();
        HandleDrag();
    }

    private void SetIsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundLayer);
    }

    private void HandleDrag()
    {
        if (isGrounded)
        {
            _rb.drag = groundDrag;
        }
        else
        {
            _rb.drag = 0;
        }
    }

    private void GetInput(InputAction.CallbackContext ctx)
    {
        keyboardInput = ctx.ReadValue<Vector2>();
    }

    # region PlayerMovement
    private void MovePlayer()
    {
        Vector3 directionOfMovement = orientation.forward * keyboardInput.y + orientation.right * keyboardInput.x;
        if (isGrounded) _rb.AddForce(directionOfMovement * playerSpeed, ForceMode.Force);
        if (!isGrounded) _rb.AddForce(directionOfMovement * playerSpeed * airSpeed, ForceMode.Force);
    }

    private void SpeedControl()
    {
        if (_rb.velocity.magnitude > maxSpeed)
        {
            // limit the speed;
            float maxSpeedX = Mathf.Clamp(_rb.velocity.x, -maxSpeed, maxSpeed);
            float maxSpeedY = Mathf.Clamp(_rb.velocity.y, -maxSpeed, maxSpeed);
            float maxSpeedZ = Mathf.Clamp(_rb.velocity.z, -maxSpeed, maxSpeed);

            _rb.velocity = new Vector3(maxSpeedX, maxSpeedY, maxSpeedZ);
        }
    }

    #endregion

    #region Debug Data
    #endregion
}
