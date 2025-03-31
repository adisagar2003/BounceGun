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
    private PlayerMovement playerRefMovement;

    [SerializeField] private GameObject playerRef;
    [SerializeField] private float rotatingSpeed = 5.0f; 
    [SerializeField] private float bulletTimeElapsed = 0.0f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletSpeed = 50.0f;
    [SerializeField] private Vector3 bulletTargetPositionOffset;
    [SerializeField] private float deathCooldown = 2.4f;
    [SerializeField] private float materialChangeCooldown = 1.4f;
    public float distanceOfDetection = 6f;


    private Animator _shooterAnimator;
    public override void Start()
    {
        base.Start();
        playerRef = GameObject.Find("Player");
        playerRefMovement = playerRef.GetComponentInChildren<PlayerMovement>();
        _shooterAnimator = GetComponent<Animator>();
        InitializeStateMachine();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InitializeStateMachine();
    }


    public float GetDistanceFromPlayer()
    {
        Vector3 playerPosition = playerRef.GetComponentInChildren<PlayerMovement>().GetCurrentPosition();
        return Vector3.Distance(playerPosition, transform.position);
    }



    private void InitializeStateMachine()
    {
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
      
    }

    public void AlertShooter()
    {
        _enemyStateMachine.ChangeState(_shooterShootState);
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


    [ContextMenu("Kill Shooter")]
    protected override void Death()
    {
        GetComponent<Rigidbody>().useGravity = false;
        //foreach (Rigidbody rbChild in GetComponentsInChildren<Rigidbody>())
        //{
        //    rbChild.useGravity = false;
        //};
        _enemyStateMachine.ChangeState(_shooterIdleState);
        _shooterAnimator.enabled = false;
        StartCoroutine(ChangeMaterialCoroutine());
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator ChangeMaterialCoroutine()
    {
        yield return new WaitForSeconds(materialChangeCooldown);
        Debug.Log("Material Should Change");
        ChangeMaterialTo(materialOnDeath);
    }

  

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(deathCooldown);
        Destroy(gameObject);
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
