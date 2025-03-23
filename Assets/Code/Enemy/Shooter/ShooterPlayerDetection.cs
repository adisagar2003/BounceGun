using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayerDetection : MonoBehaviour
{
    [SerializeField] private Shooter shooter;

    private void OnTriggerEnter(Collider collision)
    {
       if (collision.CompareTag("Player"))
        {
            shooter.AlertShooter();
        }    
    }
}
