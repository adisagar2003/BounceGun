using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Player Health
/// Death, damage handle
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float totalHealth = 100.0f;
    [SerializeField] private float damageTimeCooldown = 1.0f;
    private float damageTimeElapsed = 0.0f;
    [SerializeField] private bool isTakingDamage = false;
    private void OnEnable()
    {
        BaseDamagable.OnDamagePlayer += OnPlayerDamaged;
    }

    private void OnPlayerDamaged(float amt)
    {
        // do not damage if already getting damaged
        if (isTakingDamage) return;
        isTakingDamage = true;
        this.totalHealth -= amt;
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTakingDamage)
        {
            damageTimeElapsed += Time.deltaTime;
            if (damageTimeElapsed > damageTimeCooldown)
            {
                isTakingDamage = false;
                damageTimeElapsed = 0.0f;
            }
        }
    }
}
