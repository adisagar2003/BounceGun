using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shooting, Enemy Detection, Gun VFX Generation
/// Attaches to Player FP Physics Object
/// </summary>
public class PlayerGun : MonoBehaviour
{
    [Header("Player Gun")]
    [SerializeField] private Camera _cameraRef;
    [SerializeField] private float maxDetectionDistance = 40.0f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float damageAmount = 10.0f;
    [SerializeField] private bool isShootPressed = false;
    [SerializeField] private float shootDelay = 0.3f;
    [SerializeField] private GameObject spark;
    [SerializeField] private Transform gunTip;
    [SerializeField] private AudioSource gunShotAudio;
    public delegate void SendDetectionData();

    // future implementation 
    public static event SendDetectionData OnEnemyDetection;
    public static event SendDetectionData OnNoEnemyDetection;

    private void Start()
    {
    }


    public void ShootIsReleased()
    {
        Debug.Log("Shoot is released");
        isShootPressed = false;
    }

    public void ShootButtonPressed()
    {
        isShootPressed = true;
        Shoot();
    }

    public void Shoot()
    {
        RaycastHit hit;
        Instantiate(spark, gunTip, false);
        gunShotAudio.Play();
        bool isEnemyDetected = Physics.Raycast
        (_cameraRef.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f))
        ,out hit
        ,enemyLayer
        );
        if (!hit.Equals(null))
        {

            if (hit.transform.gameObject.GetComponent<BaseEnemy>())
            {
                BaseEnemy enemyRef = hit.transform.gameObject.GetComponent<BaseEnemy>();
                Debug.Log("Target Enemy Should Take Damage");
                if (enemyRef) enemyRef.TakeDamage(damageAmount);
            }
        }

        if (isShootPressed)
        {
        }
    }

    private void Update()
    {
        EnemyDetection();
    }

    private void EnemyDetection()
    {
        RaycastHit hit;
        bool isEnemyDetected = Physics.Raycast(_cameraRef.ViewportPointToRay(new Vector3(0.5f,0.5f,0f)), out hit, enemyLayer);
        if (!isEnemyDetected) return;
        if (isEnemyDetected && hit.transform.gameObject.layer == 8)
        {
          
            // Send signal to crosshair UI to become red
            //OnEnemyDetection?.Invoke();
        } else
        {
            //OnNoEnemyDetection?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_cameraRef.transform.position, _cameraRef.transform.forward * maxDetectionDistance);
    }
}
