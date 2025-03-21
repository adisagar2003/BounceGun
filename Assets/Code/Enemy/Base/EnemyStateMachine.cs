using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State Machine for every Enemy AI
/// Switches and initializes states
/// </summary>
public class EnemyStateMachine 
{
    public BaseEnemyState currentState { get; private set; }

    public void Initialize(BaseEnemyState state)
    {
        if (currentState != null) currentState.ExitState();
        currentState = state;
        currentState.EnterState();
    }

    public void ChangeState(BaseEnemyState state)
    {
        currentState.ExitState();
        currentState = state;
        currentState.EnterState();
    } 
    

    

    
}
