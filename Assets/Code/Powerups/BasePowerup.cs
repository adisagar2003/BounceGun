using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePowerup : MonoBehaviour
{
    [SerializeField] GameObject powerupModel;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float floatSpeed;
    [SerializeField] private float floatAmplitude = 0.2f;
    private float timeCounter = 0;
    public virtual void OnTriggerEnter(Collider other)
    {
        
    }

    protected abstract void PowerUpFunctionality();

    protected virtual void Update()
    {
        timeCounter += Time.deltaTime;
        float rotationAngle =  timeCounter * rotateSpeed;
        powerupModel.transform.localRotation = Quaternion.Euler(0, rotationAngle, 0);
        float lerpValue = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        powerupModel.transform.localPosition = new Vector3(0, lerpValue, 0);
    }
}
