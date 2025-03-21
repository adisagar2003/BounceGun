using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamagable : MonoBehaviour
{
    public  delegate void DamagePlayer(float amount);
    public static  event DamagePlayer OnHealthChanged;

    protected void TakeDamage(float amount)
    {
        OnHealthChanged?.Invoke(amount*-1); // negative for damage
    }
}
