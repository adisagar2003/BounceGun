using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Controls View of Health UI
/// Attaches to Healthbar Component of canvas
/// </summary>
public class HealthSystemUI : MonoBehaviour
{
    private PlayerHealth _playerHealth;
    [SerializeField] private float totalHealth = 100.0f;
    [SerializeField] private float currentHealth = 100.0f;
    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
    }
    
    private void OnEnable()
    {
        BaseDamagable.OnHealthChanged += OnHealthUpdate;    
    }

    private void Start()
    {
        healthSlider.value = (_playerHealth.GetCurrentHealth());
    }

    private void OnHealthUpdate(float amt)
    {
        currentHealth += amt;
        healthSlider.value = _playerHealth.GetCurrentHealth();
    }

    private void Update()
    {
    }
}
