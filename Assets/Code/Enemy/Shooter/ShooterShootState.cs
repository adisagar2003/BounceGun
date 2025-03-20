using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterShootState : BaseEnemyState
{
    public ShooterShootState(string stateName, Shooter enemyRef, EnemyStateMachine enemyStateMachine) : base(stateName, enemyRef, enemyStateMachine)
    {
        this.stateName = "Shoot";
    }

    public override void EnterState()
    {
        enemyAnimator.SetBool("isPlayerDetected", true);
        enemyAnimator.SetTrigger("Attack");

        Debug.Log("Player Will Attack now");
        // spawn a projectile towards player ref;
        enemy.ShootAtPlayer();
    }

    public override void ExitState()
    {
    }

    public override string GetStateName()
    {
        return this.stateName;
    }

    public override void OnFixedUpdateState()
    {
        Shooter baseEnemyShooterCast = (Shooter)enemy;
        baseEnemyShooterCast.LookAtPlayer();
    }

    public override void OnUpdateState()
    {
        // look at player...
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
