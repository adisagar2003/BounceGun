using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Controls View of Health UI
/// Depenendies 
/// </summary>
public class HealthSystemUI : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    [SerializeField] private float totalHealth = 100.0f;
    [SerializeField] private float currentHealth = 100.0f;
    [SerializeField] private Image healthBar;

    private void Awake()
    {
        _playerHealth = GameObject.FindFirstObjectByType<PlayerHealth>();
    }
    
    private void OnEnable()
    {
        BaseDamagable.OnHealthChanged += OnHealthUpdate;    
    }

    private void Start()
    {
        healthBar.fillAmount = (_playerHealth.GetCurrentHealth() / totalHealth);
    }

    private void OnHealthUpdate(float amt)
    {
        currentHealth += amt;
        healthBar.fillAmount = (_playerHealth.GetCurrentHealth() / totalHealth);
    }

    private void Update()
    {
    }
}
