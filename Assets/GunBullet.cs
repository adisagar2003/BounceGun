using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (other.tag == "Player")
        {
            TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
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
