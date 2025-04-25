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


    [SerializeField] private GameObject bulletPrefab;
    private Animator _shooterAnimator;

    [Header("Bullet")]
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float bulletTimeElapsed = 0.0f;
    [SerializeField] private Vector3 bulletTargetPositionOffset;
    [SerializeField] private float materialChangeCooldown = 0.4f;

    [Header("Combat")]
    [SerializeField] private float rotatingSpeed = 5.0f; 
    [SerializeField] private float bulletSpeed = 50.0f;
    [SerializeField] private float deathCooldown = 2.4f;
    [SerializeField] private float shootAfterThisManySeconds = 1.3f;
    [SerializeField] public float distanceOfDetection = 6f;


    [Header("SoundFX")]
    [SerializeField] private AudioSource shootAudio;

    public override void Start()
    {
        base.Start();
        _shooterAnimator = GetComponent<Animator>();
        InitializeStateMachine();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InitializeStateMachine();
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
        if (!isDeathCalled) shootAudio.Play();
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

        // look at player
        Vector3 transformYPosition = playerRefMovement.GetCurrentPosition();
        transformYPosition.y = transform.position.y;
        Vector3 direction = (transformYPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotatingSpeed);

        PrepareToShoot();
    }

    private void PrepareToShoot()
    {
        bulletTimeElapsed += Time.deltaTime;
        if (bulletTimeElapsed > shootAfterThisManySeconds)
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
        if (isDeathCalled) return;
        isDeathCalled = true;
        GetComponent<Rigidbody>().useGravity = false;
        // makes player float
        //foreach (Rigidbody rbChild in GetComponentsInChildren<Rigidbody>())
        //{
        //    rbChild.useGravity = false;
        //};
        _enemyStateMachine.ChangeState(_shooterIdleState);
        _shooterAnimator.enabled = false;
        SpawnRandomPowerup();
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
