using System;
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
    [SerializeField] private GameObject smoke;
    [SerializeField] private Transform gunTip;
    [SerializeField] private AudioSource gunShotAudio;  

    [Header("Ammo")]
    [SerializeField] public int ammoNotInClip = 100;
    [SerializeField] public int currentAmmoOnClip = 13;
    [SerializeField] public int clipSize = 13;
    [SerializeField] public bool isReloading = false;
    [SerializeField] private float reloadDelay ;


    [Header("Damage")]
    [SerializeField] private GameObject sparkPrefab;
    [SerializeField] private float particleHeightOffset = 3.0f;

    [Header("Particle FX")]
    [SerializeField] private GameObject bulletPrefab;
    public delegate void SendDetectionData();
    [SerializeField] private float bulletAdditionalForce = 10.0f;
    [SerializeField] private Vector3 bulletScale;

    // future implementation 
    public static event SendDetectionData OnEnemyDetection;
    public static event SendDetectionData OnNoEnemyDetection;

    private void OnEnable()
    {
        Ammo.OnAddAmmo += AddAmmo;
    }

    private void OnDisable()
    {
        Ammo.OnAddAmmo -= AddAmmo;
    }

    private void AddAmmo(int amount)
    {
        ammoNotInClip += amount;
    }

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


    [ContextMenu("Reload")]
    private void Reload()
    {
        StartCoroutine(StartReload());
        Debug.Log("Reload Called \n" + $"{currentAmmoOnClip}/{ammoNotInClip}");

    }

    private void FillClip()
    {
        // fill the clip completely 
        if (ammoNotInClip < 1)
        {
            Debug.Log("Out of ammo ");
            return;
        }

        
        if (ammoNotInClip + currentAmmoOnClip > clipSize)
        {
            // amount needed to fill clip
            int amountNeededToFillClip = clipSize - currentAmmoOnClip;
            ammoNotInClip -= amountNeededToFillClip;
            currentAmmoOnClip = currentAmmoOnClip + amountNeededToFillClip;
        }

        // Fill ammo if total ammo is less than the clip size
        else if (ammoNotInClip + currentAmmoOnClip < clipSize)
        {
            currentAmmoOnClip = currentAmmoOnClip + ammoNotInClip;
            ammoNotInClip = 0;
        }
       
    }

    private IEnumerator StartReload()
    {

        isReloading = true;
        yield return new WaitForSeconds(reloadDelay);
        FillClip();
        isReloading = false;
    }

    private void Shoot()
    {
        if (isReloading) return;
        if (currentAmmoOnClip == 0 && ammoNotInClip == 0) return;
        // do not shoot if there is no ammo 
        if (currentAmmoOnClip < 1)
        {
            Reload();
            return;
        }

        // reduce ammo amount;
        currentAmmoOnClip -= 1;



        RaycastHit hit;
        // instantiate bullet from guntip
        BulletTrailInstantiate();
        Instantiate(smoke, gunTip, false);
        gunShotAudio.Play();
        bool isEnemyDetected = Physics.Raycast
        (_cameraRef.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f))
        , out hit
        , enemyLayer
        );
        if (isEnemyDetected)
        {

            if (hit.transform.gameObject.GetComponent<BaseEnemy>())
            {
                BaseEnemy enemyRef = hit.transform.gameObject.GetComponent<BaseEnemy>();
                Debug.Log("Target Enemy Should Take Damage");
                if (enemyRef) enemyRef.TakeDamage(damageAmount);
                // Instantiate a spark particle system at hit position
                GameObject sparkFX = Instantiate(sparkPrefab, hit.point, Quaternion.identity);
                sparkFX.transform.LookAt(_cameraRef.transform.position);
            }
        }

        if (isShootPressed)
        {
        }

    }

    private void BulletTrailInstantiate()
    {
        Debug.Log("Bullet should instantiate");
        GameObject bullet = Instantiate(bulletPrefab, gunTip.position, Quaternion.identity);
        bullet.transform.localScale = bulletScale;
        bullet.GetComponent<Rigidbody>().AddForce(gunTip.forward * bulletAdditionalForce);
    }

    private void Update()
    {

    }

    //private void EnemyDetection()
    //{
    //    RaycastHit hit;
    //    bool isEnemyDetected = Physics.Raycast(_cameraRef.ViewportPointToRay(new Vector3(0.5f,0.5f,0f)), out hit, enemyLayer);
    //    if (!isEnemyDetected) return;
    //    if (isEnemyDetected && hit.transform.gameObject.layer == 8)
    //    {
          
    //    } else
    //    {
    //        //OnNoEnemyDetection?.Invoke();
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_cameraRef.transform.position, _cameraRef.transform.forward * maxDetectionDistance);

    }
}
