using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles destruction of the bullet
/// </summary>
public class GunBullet : BaseDamagable
{
    [SerializeField] private float destroyBulletTime = 2.0f;
    [SerializeField] private float bulletDamage = 13.0f;
    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    private void OnTriggerEnter(Collider other)
    {

        // for bullet shot by enemy
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Hit");
            TakeDamage(bulletDamage);
            Destroy(gameObject);
        }

        // for bullet shot by player
        // ignore Enemy layer
    }
    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
