using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wall Run functionality
/// </summary>
public class WallRun : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    [Header("Wall Check")]
    public bool isOnWall = false;
    public bool isOnRightWall = false;
    public bool isOnLeftWall = false;
    [SerializeField] private float raycastMaxDistance = 50.0f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private RaycastHit wallHitPoint;

    [Header("WallRun")]
    [SerializeField] public bool hasEnteredWallRunState = false;
    [SerializeField] public float wallJumpForceHorizontal = 14.0f;
    [SerializeField] public float wallJumpForceVertical = 20.0f;
    [SerializeField] public float wallGravity = 4.0f;
    [SerializeField] public float wallRunSpeedBoost = 10.0f;

    #region Events
    public delegate void StopCameraLerp();
    public static event StopCameraLerp OnStopCameraLerp;
    #endregion
    // check for walls left and right
    // raycast transform.left, transform.right

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        WallCheck();

        // Debug 
        Debug.DrawRay(transform.position, Vector3.forward * raycastMaxDistance, Color.yellow); ;
        Debug.DrawRay(transform.position, Vector3.forward * -1 * raycastMaxDistance, Color.red);
    }


    // checks if the player is hitting left or right walls
    private void WallCheck()
    {
        RaycastHit rightWall;
        if (Physics.Raycast(transform.position, Vector3.forward, out rightWall, raycastMaxDistance, wallLayer))
        {
            isOnRightWall = true;
            isOnWall = true;
            _playerMovement.currentState = PlayerMovement.PlayerMovementState.WallRun;
            wallHitPoint = rightWall;
            _playerMovement.EnterWallRunState("right");
        }

        else if (!isOnLeftWall)
        {
            isOnRightWall = false;
            isOnWall = false;
            _playerMovement.ExitWallState();
        };

        RaycastHit leftWall;
        if (Physics.Raycast(transform.position, transform.forward * -1, out leftWall, raycastMaxDistance, wallLayer))
        {
            isOnWall = true;
            isOnLeftWall = true;
            wallHitPoint = leftWall;
            _playerMovement.EnterWallRunState("left");
        }
        else if (!isOnRightWall)
        {
            isOnLeftWall = false;
            isOnWall = false;
            _playerMovement.ExitWallState();

        };

    }


    // For wall detection and calculating velocity depending on the orientation
    public Vector3 GetWallHitPointNormal()
    {
        return wallHitPoint.normal;
    }

    private void DoWallRun()
    {
        if (isOnWall)
        {
            // get the projection of the plane.
            // orient the direction towards the player
            
        }
    }
}
