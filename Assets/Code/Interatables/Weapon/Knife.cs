using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour, IInteractable, IThrowable
{
    [SerializeField] private GameObject KnifePrefab;
    [SerializeField] private float throwingForce = 10.0f;
    [SerializeField] private float throwingUpwardForce = 10.0f;
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
        Debug.Log("Object thrown");
        // throws the gameobject from player(Middle of screen) to player's orientation.forward;
       GameObject knifeProjectile = Instantiate(KnifePrefab,middleOfScreen,Quaternion.identity);
       Rigidbody knifeProjectileRigidbody = knifeProjectile.GetComponent<Rigidbody>();

       Vector3 throwingDirection = middleOfScreen;
       Vector3 upwardsDirection = new Vector3(0,1,0);

       knifeProjectileRigidbody.AddForce(throwingDirection * throwingForce, ForceMode.Impulse);
       knifeProjectileRigidbody.AddForce(upwardsDirection * throwingForce, ForceMode.Impulse);

    }

    private void GetMiddleOfScreen(Vector3 directionFromCamera)
    {
        middleOfScreen = directionFromCamera;
    }
    
}
