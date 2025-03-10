using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour, IInteractable, IThrowable
{
    [SerializeField] private GameObject KnifePrefab;
    [Header("Projectile Forces")]
    [SerializeField] private float throwingForce = 10.0f;
    [SerializeField] private float throwingUpwardForce = 10.0f;
    [SerializeField] private Vector3 throwingTorque = Vector3.zero;
    [SerializeField] private float destroyTime = 3.0f;
    [SerializeField] private Transform attackorigin;
    private Vector3 middleOfScreen;


    private void Awake()
    {
        Debug.Log("Knife Awakened");
    }

    private void OnEnable()
    {
        CameraUtils.OnSendRayData += GetMiddleOfScreen;
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        CameraUtils.OnSendRayData -= GetMiddleOfScreen;
    }

    public void Interact()
    {
        Debug.Log("Future interactions");
    }

    [ContextMenu("Throw")]
    public void Throw()
    {
        // throws the gameobject from player(Middle of screen) to player's orientation.forward;
        Rigidbody knifeProjectileRigidbody = SpawnThrowable(KnifePrefab);
        knifeProjectileRigidbody.AddForce(attackorigin.forward * throwingForce, ForceMode.Impulse);
        knifeProjectileRigidbody.AddTorque(throwingTorque);
    }

    private Rigidbody SpawnThrowable(GameObject prefab)
    {
        GameObject throwableProjectile = Instantiate(prefab, attackorigin.position, Quaternion.Euler(-90,0,0));
        Rigidbody throwableProjectileRigidbody = throwableProjectile.GetComponent<Rigidbody>();
        StartCoroutine(AutoDestroy(destroyTime, throwableProjectile));
        return throwableProjectileRigidbody;
    }

    public IEnumerator AutoDestroy(float destroyTime, GameObject throwable)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(throwable);
    }

    private void GetMiddleOfScreen(Vector3 directionFromCamera)
    {
        middleOfScreen = directionFromCamera;
    }
    
}
