using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePowerup : MonoBehaviour
{
    [SerializeField] GameObject powerupModel;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float floatSpeed;
    [SerializeField] private float floatAmplitude = 0.2f;
    private PlayerMovement? playerRef;
    private bool isStatic = true;
    private float timeCounter = 0;
    private float timeElapsed = 0;
    [SerializeField] private float powerupMoveSpeed = 10.0f;
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PowerUpFunctionality();
            Destroy(gameObject);
        }
    }

    protected abstract void PowerUpFunctionality();

    protected virtual void Start()
    {
        playerRef = FindFirstObjectByType<PlayerMovement>();
    }

    protected virtual void Update()
    {
        timeElapsed += Time.deltaTime;
        timeCounter += Time.deltaTime * rotateSpeed;
        float rotationAngle = timeCounter;
        powerupModel.transform.localRotation = Quaternion.Euler(0, rotationAngle, 0);
        float lerpValue = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        powerupModel.transform.localPosition = new Vector3(0, lerpValue, 0);
        if (timeElapsed > 3.0f)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
            if (playerRef == null) return;
        
            transform.position = Vector3.MoveTowards(transform.position, playerRef.transform.position, powerupMoveSpeed * Time.deltaTime * timeElapsed);
    
    }
}
