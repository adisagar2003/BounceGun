using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Interface for objects that damage player 
/// </summary>
public interface IAttackable
{
    public void DamagePlayer();
    void ShootAtPlayer();
}
