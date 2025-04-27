using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IAttackable
{
    // Start is called before the first frame update
    protected float damage = 10.0f;
    protected float health = 40.0f;

    // enemy needs to have at least one state
    protected BaseEnemyState baseEnemyState;
    protected EnemyStateMachine _enemyStateMachine;

    [SerializeField] protected Material materialOnDeath;
    [SerializeField] protected List<GameObject> powerups;
    protected bool isDeathCalled = false;

    [SerializeField] protected Vector3 powerupSpawnOffset = new Vector3(0, 1.0f, 0);
    [SerializeField] private AudioSource hurtAudio;

    [Header("References")]
    [SerializeField] protected GameObject playerRef;
    [SerializeField] protected PlayerMovement playerRefMovement;

    public virtual void Start()
    {
        _enemyStateMachine = new EnemyStateMachine();
        playerRef = GameObject.Find("Player");
        playerRefMovement = playerRef.GetComponentInChildren<PlayerMovement>();
    }

    protected virtual void OnEnable()
    {
        _enemyStateMachine = new EnemyStateMachine();
    }

    protected virtual void OnDisable()
    {
    }

    public abstract void ShootAtPlayer();

    public virtual void DamagePlayer(float amt)
    {

    }

    public float GetDistanceFromPlayer()
    {
        Vector3 playerPosition = playerRef.GetComponentInChildren<PlayerMovement>().GetCurrentPosition();
        return Vector3.Distance(playerPosition, transform.position);
    }


    public virtual void TakeDamage(float amt)
    {

        health -= amt;

        if (health < 0)
        {
            Death();
        }
    }

    // Take damage with a direction vector as param 
    public virtual void TakeDamage(float amt, Vector3 direction) {
        // Taking damage 
        hurtAudio.Play();
        Debug.Log("Hurt Audio should be heard");    
        health -= amt;
       
    }

    protected virtual void Death()
    {
        // prevents 
        if (isDeathCalled) return;
        isDeathCalled = true;
        // instantiate a random powerup on this location
        SpawnRandomPowerup();
        Destroy(gameObject);
    }

    protected void SpawnRandomPowerup()
    {
        int r = UnityEngine.Random.Range(0, powerups.Count - 1);
        Instantiate(powerups[r], transform.position + powerupSpawnOffset, Quaternion.identity);
    }

    public virtual float GetHealth()
    {
        return health;
    }

    protected virtual void ChangeMaterialTo(Material targetMaterial)
    {
        SkinnedMeshRenderer meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material = targetMaterial;
        }
    }

    public void DamagePlayer()
    {
        throw new NotImplementedException();
    }
}
