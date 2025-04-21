using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for objects that can damage or attack the player.
/// </summary>

public interface IAttackable
{
    /// <summary>
    /// Applies damage to the player.
    /// </summary>
    void DamagePlayer();

    /// <summary>
    /// Performs a shooting attack towards the player.
    /// </summary>
    void ShootAtPlayer();
}
