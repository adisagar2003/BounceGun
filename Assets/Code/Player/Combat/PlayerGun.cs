using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shooting, Enemy Detection
/// </summary>
public class PlayerGun : MonoBehaviour
{
    [Header("Player Gun")]
    [SerializeField] private Camera _cameraRef;
    [SerializeField] private float maxDetectionDistance = 40.0f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float damageAmount = 10.0f;
    public delegate void SendDetectionData();
    public static event SendDetectionData OnEnemyDetection;
    public static event SendDetectionData OnNoEnemyDetection;

    public void Shoot()
    {
        RaycastHit hit;
        bool isEnemyDetected = Physics.Raycast
        (_cameraRef.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f))
        ,out hit
        ,enemyLayer
        );
        if (isEnemyDetected)
        {
            BaseEnemy enemyRef = hit.transform.gameObject.GetComponent<BaseEnemy>();
            if (enemyRef) enemyRef.TakeDamage(damageAmount);
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
        Debug.Log(isEnemyDetected);
        Debug.Log((hit.transform.gameObject.layer) + "-Bool val");
        if (isEnemyDetected && hit.transform.gameObject.layer == 8)
        {
          
            // Send signal to crosshair UI to become red
            OnEnemyDetection?.Invoke();
        } else
        {
            OnNoEnemyDetection?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_cameraRef.transform.position, _cameraRef.transform.forward * maxDetectionDistance);
    }
}
