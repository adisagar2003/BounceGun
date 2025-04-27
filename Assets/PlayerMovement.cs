#define DEBUGMODE

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Controls movement to the player 
/// MAJOR DEPENDENT CLASS FOR PLAYER
/// Dependencies:
/// 
///     -- WallRun.cs
///     -- CameraControlPlayer
///     -- Rigidbody on GameObject
/// </summary>
/// 
    
public class PlayerMovement : MonoBehaviour
{
    [Header("Handles Player Movement")]
    [Header("Movement")]
    [SerializeField] private float playerSpeed = 10.0f;
    [SerializeField] private float jumpForce = 100.0f;
    [SerializeField] private float maxWalkSpeed = 7.0f;
    [SerializeField] private float maxSprintSpeed = 12.0f;
    [SerializeField] private float playerMaxSpeed = 7.0f;
    [SerializeField] private float airSpeed = 1.5f;
    [SerializeField] private float playerStopIntensity = 2.0f;
    private InputMaster inputMaster;

    private Rigidbody _rb;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform FPController;
    private Vector2 keyboardInput;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;

    // returns current position
    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }

    [SerializeField] private float groundDrag;
    [SerializeField] private bool isReadyToJump = true;
    [SerializeField] private bool jumpInputPressed = false;
    [SerializeField] private float jumpCooldown = 0.5f;
    [SerializeField] private float wallJumpCooldown = 0.5f;
    [SerializeField] private float sprintBoost = 5.5f;

    [Header("Input")]
    [SerializeField] private bool isMovePressed = false;
    [SerializeField] private bool isSprintPressed = false;

    [Header("Slope Detection")]
    [SerializeField] private bool isOnSlope = false;
    [SerializeField] private float slopeDetectionHeight = 0.3f;
    [SerializeField] private RaycastHit slopeHit;

    [Header("Input Disable")]
    [SerializeField] private bool isMovementEnabled = true;
    [SerializeField] private float disableTimeElapsed = 0.0f;
    // used for disabling input for some time
    // for example wall slide needs input disabled for a certain time after jumping
    [SerializeField] private float disableTimeDuration = 2.0f;


    [Header("Audio Stuff")]
    [SerializeField] private AudioSource walkSound;
    [SerializeField] private AudioSource sprintSound;
    [SerializeField] private AudioSource jumpSound;

    #region Events
    public delegate void CameraLeanTowards(string direction);
    public static event CameraLeanTowards  OnCameraLeanTowards;

    public delegate void ResetCamera();
    public static event ResetCamera OnResetCamera;
    #endregion

    // dependencies
    private WallRun _playerWallRun;
    private CameraControlPlayer _cameraControl;
    public enum PlayerMovementState
    {
        Idle,
        Walk,
        Sprint,
        WallRun,
        Air
    };
    
    public PlayerMovementState currentState = PlayerMovementState.Idle;


    [Header("Debugging")]
    public bool showDebugUI = false;
    [SerializeField] private string debugString = "";

    private void Awake()
    {
    }

    public Vector3 GetPositionOfPlayer()
    {
        return FPController.position;
    }

    public void AddImpulsiveForceTowards(Vector3 direction)
    {
        _rb.AddForce(direction, ForceMode.Impulse);
    }


    // gets real value of gravity in game. 
    public float GetGravityYValue()
    {
        return Physics.gravity.y;
    }
    #region Events
    [Obsolete]
    public delegate void SendDebugData(Dictionary<string,string> data);
    [Obsolete]
    public static event SendDebugData OnSendDebugData;
    #endregion
  
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraControl = GetComponentInChildren<CameraControlPlayer>();
        _playerWallRun = GetComponent<WallRun>();
    }

    private void OnEnable()
    {
        PlayerHealth.OnDeathEvent += DisableAllInputs;
    }


    private void OnDisable()
    {
        PlayerHealth.OnDeathEvent -= DisableAllInputs;

    }
    private void DisableAllInputs()
    {
        isMovementEnabled = false;
    }

    private void EnableAllInputs()
    {
        isMovementEnabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isMovementEnabled) return;
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

        // Loop Walking Sound if Walking
        if (currentState == PlayerMovementState.Walk)
        {
            if (walkSound.isPlaying) return;
            walkSound.Play();
        }
        else if (currentState == PlayerMovementState.Sprint)
        {
            if (sprintSound.isPlaying) return;
            sprintSound.Play();
        }
        else if (currentState == PlayerMovementState.WallRun)
        {
            if (walkSound.isPlaying) return;
            walkSound.Play();
        }
        else if (currentState == PlayerMovementState.Idle || currentState == PlayerMovementState.Air)
        {
            walkSound.Stop();
            sprintSound.Stop();
        }

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
        if (!isGrounded && !_playerWallRun.hasEnteredWallRunState)
        {
            _rb.useGravity = true;
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

        //wallrun 
        if (currentState == PlayerMovementState.WallRun)
        {
            // add constant downward force to simulate slide
            _rb.AddForce(Vector3.down * _playerWallRun.wallGravity, ForceMode.Force);
        }

        // wall run 
        // future implementation

    }

    public void AddLinearForceTowards(Vector3 value)
    {
        _rb.AddForce(value, ForceMode.Force);
    }

    #region PlayerMovement
    private void MovePlayer()
    {
        Vector3 directionOfMovement = orientation.forward * keyboardInput.y + orientation.right * keyboardInput.x;

        StopMovementIfNoInput(directionOfMovement);

        // Handle WallRun Movement
        if (currentState == PlayerMovementState.WallRun)
        {
            // disable x movement
            directionOfMovement = orientation.forward * keyboardInput.y;
            _rb.AddForce(ProjectMoveDirectionOnSlope(directionOfMovement, _playerWallRun.GetWallHitPointNormal()) * playerSpeed * sprintBoost * _playerWallRun.wallRunSpeedBoost, ForceMode.Force);
        }

        else
        {
            // normal movement
            if (isGrounded && !isOnSlope) _rb.AddForce(directionOfMovement * playerSpeed * sprintBoost, ForceMode.Force);

            // slop movement
            else if (isOnSlope && isGrounded)
            {
                _rb.AddForce(ProjectMoveDirectionOnSlope(directionOfMovement, slopeHit.normal) * playerSpeed * sprintBoost, ForceMode.Force);
                CheckIfPropellingUpInSlope();
            }

            // air movement
            if (!isGrounded) _rb.AddForce(directionOfMovement * playerSpeed * airSpeed, ForceMode.Force);
        }
    }

    private void StopMovementIfNoInput(Vector3 directionOfMovement)
    {
        // stop the player without sliding if no movement
        if (directionOfMovement == Vector3.zero && isGrounded && !isOnSlope && currentState != PlayerMovementState.WallRun)
        {
            _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, Time.deltaTime * playerStopIntensity);
        }
    }

    private void CheckIfPropellingUpInSlope()
    {
        if (_rb.velocity.y > 0)
        {
            _rb.AddForce(Vector3.down * 80.0f, ForceMode.Force);
        }
    }

    private void CheckIfPropellingInWalls(Vector3 normalDirection)
    {
        if (_rb.velocity.y > 0)
        {
            _rb.AddForce(normalDirection * -1 * 80.0f * Time.deltaTime, ForceMode.Force);
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
            isOnSlope = false;
            return false;
        };
    }

    public void SetIsSprintPressed(bool value)
    {
        isSprintPressed = value;
    }

    private Vector3 ProjectMoveDirectionOnSlope(Vector3 moveDirection, Vector3 normalHit)
    {
        return Vector3.ProjectOnPlane(moveDirection, normalHit).normalized;
    }

    private Vector3 ProjectMoveDirectionOnPlane(Vector3 moveDirection)
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public void StopMovement()
    {
        _rb.velocity = Vector3.zero;
    }

    public void SetGrappleVelocity(Vector3 value)
    {
        _rb.velocity = value;
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
        if (isGrounded && isReadyToJump && jumpInputPressed && currentState != PlayerMovementState.WallRun)
        {

            isReadyToJump = false;
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Call jump after delay if jump is pressed constantly
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Walljump: Direction will be normal towards plane...
        else if (currentState == PlayerMovementState.WallRun && isReadyToJump && jumpInputPressed)
        {
        #if DEBUGMODE
        #endif

            isReadyToJump = false;
            _rb.useGravity = true;
            DisableAllInputs();
            StartCoroutine(EnableInputAgain());
            _rb.AddForce(_playerWallRun.GetWallHitPointNormal() * _playerWallRun.wallJumpForceHorizontal, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private IEnumerator EnableInputAgain()
    {
        yield return new WaitForSeconds(wallJumpCooldown);
        EnableAllInputs();
    }

    // ground check 
    private void SetIsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundLayer);
         
        Debug.DrawRay(transform.position, Vector3.down, Color.red, playerHeight);
    }

    private void ResetJump()
    {
        isReadyToJump = true;
    }

    #endregion

    #region Wall Run
    public void EnterWallRunState(string directionToLeanTo)
    {
        if (_playerWallRun.hasEnteredWallRunState) return;
        _playerWallRun.hasEnteredWallRunState = true;
        _rb.useGravity = false;
        currentState = PlayerMovementState.WallRun;
        OnCameraLeanTowards?.Invoke(directionToLeanTo);
    }

    public void ExitWallState()
    {
        // wall has been exited
        if (!_playerWallRun.hasEnteredWallRunState) return;
        _playerWallRun.hasEnteredWallRunState = false;
        _rb.useGravity = true;
        currentState = PlayerMovementState.Air;
        OnResetCamera?.Invoke();
    }

    #endregion

    #region Debug Data
    private void CalculateDebugData()
    {
        debugString = $"Player  Movement Debug Data\n"
            + $"\nMax Speed: {playerSpeed}" +
            $"\nIs On Ground: {isGrounded}" +
            $"\nSpeed:{ _rb.velocity.magnitude}" +
            $"\nCurrent State: {currentState}" +
            $"\n Is On Slope: {isOnSlope}" +
            $"\n Jump" +
            $"\n Is Ready To Jump: {isReadyToJump}" +
            $"\n Jump Input Pressed: {jumpInputPressed}" +
            $"\n Jump Cooldown: {jumpCooldown}" +
            $"\n Has Entered Wall Run State: {_playerWallRun.hasEnteredWallRunState}"
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
