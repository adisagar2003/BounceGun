using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected float damage;
    // Start is called before the first frame update
    public virtual void DamageObject(Collider collider)
    {
        IDamagable damagable = collider.GetComponent<IDamagable>();
        
        if (damagable != null)
        {
            damagable.TakeDamage(damage);
        }
    }
}
