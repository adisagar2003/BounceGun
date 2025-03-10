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
    [SerializeField] private float maxWalkSpeed = 7.0f;
    [SerializeField] private float maxSprintSpeed = 12.0f;
    [SerializeField] private float playerMaxSpeed = 7.0f;
    [SerializeField] private float airSpeed = 1.5f;
    private InputMaster inputMaster;

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
    [SerializeField] private float sprintBoost = 5.5f;

    [Header("Input")]
    [SerializeField] private bool isMovePressed = false;
    [SerializeField] private bool isSprintPressed = false;

    [Header("Slope Detection")]
    [SerializeField] private bool isOnSlope = false;
    [SerializeField] private float slopeDetectionHeight = 0.3f;
    [SerializeField] private RaycastHit slopeHit;

    public enum PlayerMovementState
    {
        Idle,
        Walk,
        Sprint,
        Air
    };
    
    public PlayerMovementState currentState { get; private set; } = PlayerMovementState.Idle;

    [Header("Debugging")]
    public bool showDebugUI = false;
    [SerializeField] private string debugString = "";
    #region Events
    // future implementation
    public delegate void SendDebugData(Dictionary<string,string> data);
    public static event SendDebugData OnSendDebugData;
    #endregion
    // Start is called before the first frame update
    void Awake()
    {
        //inputMaster = new InputMaster();
        //inputMaster.Player.Move.performed += context => { GetInput(context); isMovePressed = true; };
        //inputMaster.Player.Move.canceled += context => { GetInput(context); isMovePressed = false; };
        //inputMaster.Player.Jump.performed += context => JumpPressed();
        //inputMaster.Player.Jump.canceled += context => JumpCancelled();
        //inputMaster.Player.Sprint.performed += context => { isSprintPressed = true; };
        //inputMaster.Player.Sprint.canceled += context => { isSprintPressed = false; };
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
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
        CalculateDebugData();
        StateManagement();
        IsOnSlope();
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

    public void GetInput(Vector2 keyboardInput)
    {
        this.keyboardInput = keyboardInput;
    }

    public void SetIsMovePressed(bool isMovePressed)
    {
        this.isMovePressed = isMovePressed;
    }

    private void StateManagement()
    {
        // walk
        if (isMovePressed && isGrounded && !isSprintPressed)
        {
            sprintBoost = 1.0f;   
            playerMaxSpeed = maxWalkSpeed;
            currentState = PlayerMovementState.Walk;
        }
        
        //sprint
        if (isSprintPressed  && isMovePressed && isGrounded)
        {
            sprintBoost = 4.0f;
            playerMaxSpeed = maxSprintSpeed;
            currentState = PlayerMovementState.Sprint;
        }

        // air
        if (!isGrounded)
        {
            sprintBoost = 1.0f;
            playerMaxSpeed = maxWalkSpeed;
            currentState = PlayerMovementState.Air;
        }

        // idle
        if (!isMovePressed && isGrounded && _rb.velocity.sqrMagnitude < 0.1)
        {
            sprintBoost = 1.0f;
            playerMaxSpeed = maxWalkSpeed;
            currentState = PlayerMovementState.Idle;
        }

    }

    # region PlayerMovement
    private void MovePlayer()
    {
        Vector3 directionOfMovement = orientation.forward * keyboardInput.y + orientation.right * keyboardInput.x;
        
        if (isGrounded && !isOnSlope) _rb.AddForce(directionOfMovement * playerSpeed * sprintBoost, ForceMode.Force);
        else if (isOnSlope && isGrounded)
        {
            _rb.AddForce(ProjectMoveDirectionOnSlope(directionOfMovement) * playerSpeed * sprintBoost, ForceMode.Force);
            CheckIfPropellingUpInSlope();
        }
        if (!isGrounded) _rb.AddForce(directionOfMovement * playerSpeed * airSpeed, ForceMode.Force);
    }

    private void CheckIfPropellingUpInSlope()
    {
        if (_rb.velocity.y > 0)
        {
            _rb.AddForce(Vector3.down * 80.0f, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        if (_rb.velocity.magnitude > playerMaxSpeed)
        {
            // limit the speed;
            float maxSpeedX = Mathf.Clamp(_rb.velocity.x, -playerMaxSpeed, playerMaxSpeed);
            float maxSpeedY = Mathf.Clamp(_rb.velocity.y, -playerMaxSpeed, playerMaxSpeed);
            float maxSpeedZ = Mathf.Clamp(_rb.velocity.z, -playerMaxSpeed, playerMaxSpeed);

            _rb.velocity = new Vector3(maxSpeedX, maxSpeedY, maxSpeedZ);
        }
    }

    private bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight + slopeDetectionHeight))
        {
            // find if the player is not on flat ground 
            // find angle
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            isOnSlope = slopeAngle > 0.01f;
            if (slopeAngle > 0.01f)
            {
                _rb.useGravity = false;
            }
            return slopeAngle > 0.01f;
        }
        else
        {
            _rb.useGravity = true;
            isOnSlope = false;
            return false;
        };
    }

    public void SetIsSprintPressed(bool value)
    {
        isSprintPressed = value;
    }

    private Vector3 ProjectMoveDirectionOnSlope(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    #endregion

    #region Player Jump
    public void JumpCancelled()
    {
        jumpInputPressed = false;
    }

    public void JumpPressed()
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
    private void SetIsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundLayer);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }

    #endregion

    #region Debug Data
    private void CalculateDebugData()
    {
        debugString = $"Player  Movement Debug Data\n"
            + $"\nMax Speed: {playerSpeed}" +
            $"\nGround: {isGrounded}" +
            $"\nSpeed:{ _rb.velocity.magnitude}" +
            $"\nCurrent State: {currentState}" + 
            $"\n Is On Slope: {isOnSlope}"
            ;
    }

    private void OnGUI()
    {
        if (!showDebugUI) return;
        GUIStyle debugWindowStyle = new GUIStyle();
        debugWindowStyle.normal.textColor = Color.red;
        debugWindowStyle.fontSize = 32;
        GUI.Label(new Rect(10, 70, 200, 200), debugString, debugWindowStyle);
    }
    #endregion

    
}
