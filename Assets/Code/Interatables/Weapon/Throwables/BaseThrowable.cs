using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseThrowable : MonoBehaviour, IThrowable
{
    [SerializeField] protected GameObject ThrowablePrefab;
    [Header("Projectile Forces")]
    [SerializeField] protected float throwingForce = 10.0f;
    [SerializeField] protected float throwingUpwardForce = 10.0f;
    [SerializeField] protected Vector3 throwingTorque = Vector3.zero;
    [SerializeField] protected float destroyTime = 3.0f;
    [SerializeField] protected Transform attackorigin;


    [ContextMenu("Throw")]
    public void Throw()
    {
        // throws the gameobject from player(Middle of screen) to player's orientation.forward;
        Rigidbody knifeProjectileRigidbody = SpawnThrowable(ThrowablePrefab);
        knifeProjectileRigidbody.AddForce(attackorigin.forward * throwingForce, ForceMode.Impulse);
        knifeProjectileRigidbody.AddTorque(throwingTorque);
    }

    protected virtual Rigidbody SpawnThrowable(GameObject prefab)
    {
        GameObject throwableProjectile = Instantiate(prefab, attackorigin.position, Quaternion.Euler(-90, 0, 0));
        Rigidbody throwableProjectileRigidbody = throwableProjectile.GetComponent<Rigidbody>();
        StartCoroutine(AutoDestroy(destroyTime, throwableProjectile));
        return throwableProjectileRigidbody;
    }

    public IEnumerator AutoDestroy(float destroyTime, GameObject throwable)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(throwable);
    }

}
