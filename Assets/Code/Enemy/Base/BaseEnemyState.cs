using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyState
{
    public bool isCompleted { get; protected set; }
    protected float startTime;
    protected BaseEnemy enemy;
    protected EnemyStateMachine enemyStateMachine;
    protected Animator? enemyAnimator;
    protected string stateName;
    public float time => Time.time - startTime;

    public BaseEnemyState(string stateName,BaseEnemy enemyRef, EnemyStateMachine enemyStateMachine)
    {
        this.stateName = stateName;
        this.enemy = enemyRef;
        this.enemyStateMachine = enemyStateMachine;
        if (enemy)
        {
            enemyAnimator = enemy.GetComponent<Animator>();
        }
    }


    public abstract void EnterState();

    public abstract void ExitState();

    public abstract string GetStateName();
    public abstract void OnUpdateState();

    public abstract void OnFixedUpdateState();
}
