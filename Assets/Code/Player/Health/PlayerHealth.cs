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
    [SerializeField] private bool isTakingDamage = false;
    
    private float damageTimeElapsed = 0.0f;

    public delegate void Death();
    public static event Death OnDeathEvent;

    private void OnEnable()
    {
        BaseDamagable.OnHealthChanged += OnPlayerHealthChanged;
    }

    private void OnDisable()
    {
        BaseDamagable.OnHealthChanged -= OnPlayerHealthChanged;
    }

    public float GetCurrentHealth()
    {
        return totalHealth;
    }

    private void OnPlayerHealthChanged(float amt)
    {
        if (isTakingDamage) return;
        isTakingDamage = true;
        this.totalHealth += amt;
        if (totalHealth <= 0)
        {
            OnDeathEvent?.Invoke();
        }
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
