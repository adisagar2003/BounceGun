using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main File for Shooter Enemy 
/// Depenencies :
///         -- Idle State
///         -- Shoot State
///         -- State Machine
/// </summary>
public class Shooter : BaseEnemy
{
    private ShooterIdleState _shooterIdleState;
    private ShooterShootState _shooterShootState;

    [SerializeField] private GameObject playerRef;
    private PlayerMovement playerRefMovement;
    [SerializeField] private float rotatingSpeed = 5.0f; 
    [SerializeField] private float bulletTimeElapsed = 0.0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed = 50.0f;
    [SerializeField] private Vector3 bulletTargetPositionOffset;

    public override void Start()
    {
        base.Start();
        playerRefMovement = playerRef.GetComponentInChildren<PlayerMovement>();
        _shooterIdleState = new ShooterIdleState("Idle", this, _enemyStateMachine);
        _shooterShootState = new ShooterShootState("Shoot", this, _enemyStateMachine);
        _enemyStateMachine.Initialize(_shooterIdleState);

    }

    [ContextMenu("Shoot At Playerrr")]
    public override void ShootAtPlayer()
    {
        Vector3 directionTowardsPlayer = (playerRefMovement.GetCurrentPosition() + bulletTargetPositionOffset - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = gunPoint.position;
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = bulletSpeed * directionTowardsPlayer;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _enemyStateMachine.ChangeState(_shooterShootState);
        }
    }

    public void LookAtPlayer()
    {
        if (playerRefMovement == null) return;
        Vector3 transformYPosition = playerRefMovement.GetCurrentPosition();
        transformYPosition.y = transform.position.y;
        Vector3 direction = (transformYPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotatingSpeed);
        bulletTimeElapsed += Time.deltaTime;
        if (bulletTimeElapsed > 2.0f)
        {
            ShootAtPlayer();
            bulletTimeElapsed = 0.0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _enemyStateMachine.ChangeState(_shooterIdleState);
        }
    }

    private void Update()
    {
        _enemyStateMachine.currentState.OnUpdateState();
    }

    private void FixedUpdate()
    {
        _enemyStateMachine.currentState.OnFixedUpdateState();
    }


}
