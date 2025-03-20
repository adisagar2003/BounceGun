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
    
    public virtual void Start()
    {
        _enemyStateMachine = new EnemyStateMachine();
    }

    public abstract void ShootAtPlayer();

    public virtual void DamagePlayer()
    {

    }


}
