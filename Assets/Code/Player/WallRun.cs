using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    [Header("Wall Check")]
    public bool isOnWall = false;
    public bool isOnRightWall = false;
    public bool isOnLeftWall = false;
    [SerializeField] private float raycastMaxDistance = 50.0f;
    [SerializeField] private LayerMask wallLayer;
    // check for walls left and right
    // raycast transform.left, transform.right

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        WallCheck();
        
        // Debug 
        Debug.DrawRay(transform.position, transform.right * raycastMaxDistance, Color.yellow);
        Debug.DrawRay(transform.position, transform.right * -1 * raycastMaxDistance, Color.red);
    }


    // checks if the player is hitting left or right walls
    private void WallCheck()
    {
        RaycastHit rightWall;
        if (Physics.Raycast(transform.position, transform.right, out rightWall, raycastMaxDistance, wallLayer))
        {
            Debug.Log("Right Wall Detection");
            isOnRightWall = true;
            isOnWall = true;
        }

        else if (!isOnLeftWall)
        {
            isOnRightWall = false;
            isOnWall = false;
        };

        RaycastHit leftWall;
        if (Physics.Raycast(transform.position, transform.right * -1, out leftWall, raycastMaxDistance, wallLayer))
        {
            Debug.Log("Left Wall Detection");
            isOnWall = true;
            isOnLeftWall = true;    
        }
        else if (!isOnRightWall)
        {

            isOnLeftWall = false;
            isOnWall = false;
        };
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
