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
    private InputMaster inputMaster;
    private string debugString = "";

    private Rigidbody _rb;
    [SerializeField] private Transform orientation;
    private Vector2 keyboardInput;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool grounded;
    [SerializeField] private float groundDrag;
    
    // Start is called before the first frame update
    void Awake()
    {
        inputMaster = new InputMaster();
        inputMaster.Player.Move.performed += context => GetInput(context);
        inputMaster.Player.Move.canceled += context => GetInput(context);
        inputMaster.Player.Jump.performed += context => PlayerJump(context);
    }

    private void PlayerJump(InputAction.CallbackContext context)
    {
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
        MovePlayer();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 1.0f, groundLayer);
        SpeedControl();
        HandleDrag();
    }

    private void HandleDrag()
    {
        if (grounded)
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
        _rb.AddForce(directionOfMovement * playerSpeed, ForceMode.Force);
        Debug.Log(_rb.velocity);
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
