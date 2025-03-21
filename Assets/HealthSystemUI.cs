using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthSystemUI : MonoBehaviour
{
    [SerializeField] private float totalHealth = 100.0f;
    [SerializeField] private float currentHealth = 100.0f;
    [SerializeField] private Image healthBar;

    private void OnEnable()
    {
        BaseDamagable.OnHealthChanged += OnHealthUpdate;    
    }

    private void Start()
    {
        healthBar.fillAmount = (currentHealth / totalHealth);
    }

    private void OnHealthUpdate(float amt)
    {
        currentHealth += amt;
        healthBar.fillAmount = (currentHealth / totalHealth);
    }
}
