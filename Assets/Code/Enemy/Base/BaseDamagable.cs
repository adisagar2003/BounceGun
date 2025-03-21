using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamagable : MonoBehaviour
{
    public  delegate void DamagePlayer(float amount);
    public static  event DamagePlayer OnDamagePlayer;

    protected void TakeDamage(float amount)
    {
        OnDamagePlayer?.Invoke(amount);
    }
}
