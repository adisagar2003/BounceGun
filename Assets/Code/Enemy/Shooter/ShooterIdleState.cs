using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Play idle animation
/// Wait for player to come in attack reigon
/// </summary>
public class ShooterIdleState : BaseEnemyState
{
    public ShooterIdleState(string stateName, BaseEnemy enemyRef, EnemyStateMachine enemyStateMachine) : base(stateName, enemyRef, enemyStateMachine)
    {
        
    }

    public override void EnterState()
    {
        enemyAnimator.SetBool("isPlayerDetected", false);
        Debug.Log("Entered Idle State");
    }

    public override void ExitState()
    {

    }

    public override string GetStateName()
    {
        return "Idle";
    }

    public override void OnFixedUpdateState()
    {
    }

    public override void OnUpdateState()
    {
        Shooter shooterCast = (Shooter) enemy;
        if (shooterCast.GetDistanceFromPlayer() < shooterCast.distanceOfDetection)
        {
            shooterCast.AlertShooter();
        }
    }
}
