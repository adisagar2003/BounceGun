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


    protected virtual void Death()
    {
        Destroy(gameObject);
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
