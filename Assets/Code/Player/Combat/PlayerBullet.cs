using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Bullet that player shoots to hit enemy
/// Future: A base class for enemy bullet and player bullet can be made.
/// </summary>
public class PlayerBullet : MonoBehaviour
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
        if (other.CompareTag("Enemy"))
        {
            BaseEnemy enemyRef = other.GetComponent<BaseEnemy>();
            if (enemyRef)
            {
                enemyRef.TakeDamage(bulletDamage);
            }
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
