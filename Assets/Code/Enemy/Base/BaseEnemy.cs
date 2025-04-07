using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IAttackable
{
    // Start is called before the first frame update
    protected float damage = 10.0f;
    protected float health = 40.0f;
    protected EnemyStateMachine _enemyStateMachine;
    // enemy needs to have at least one state
    protected BaseEnemyState baseEnemyState;
    [SerializeField] protected Material materialOnDeath;
    [SerializeField] protected List<GameObject> powerups;
    protected bool isDeathCalled = false;
    [SerializeField] protected Vector3 powerupSpawnOffset = new Vector3(0, 1.0f, 0);
    public virtual void Start()
    {
        _enemyStateMachine = new EnemyStateMachine();
    }

    protected virtual void OnEnable()
    {
        _enemyStateMachine = new EnemyStateMachine();
    }

    protected virtual void OnDisable()
    {
    }

    public abstract void ShootAtPlayer();

    public virtual void DamagePlayer()
    {

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

}
