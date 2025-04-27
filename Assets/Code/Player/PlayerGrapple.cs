using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Grapple Functionality of player
/// Dependencies: 
///     -- Player input handler
///     -- Line renderer in child object
///     -- pLAYER MOVEMENT
/// </summary>
public class PlayerGrapple : MonoBehaviour
{

    private PlayerMovement _playerMovement;

    [Header("Grapple Checks")]
    [SerializeField] private bool isGrappleKeyPressed = false;
    [SerializeField] private bool isGrappling = false;
    [SerializeField] private Transform gunTip;
    [SerializeField] private Vector3 initialGrappleVelocity;

    [Header("Grapple Controls")]
    [SerializeField] private float grappleTimeElapsed = 0.0f;
    [SerializeField] private float maxGrappleDistance = 40.3f;
    [SerializeField] private float grappleDuration = 1.2f;
    [SerializeField] private float grappleHeightOffset = 3.2f;
    [Header("Grapple Speed")]
    [SerializeField] private float grappleLinearForce = 500.2f;
    [SerializeField] private float stopGrappleAtThisDistance = 2.2f;
    
    [Header("Grapple Points")]
    [SerializeField] private LayerMask whatIsGrappable;
    [SerializeField] private Vector3 grappleHitPoint;
    [SerializeField] private Camera cam;

    [Header("Line Handle")]
    [SerializeField] private LineRenderer lineRenderer;

    [Header("FX")]
    [SerializeField] private GameObject sparkPrefab;
    private void Awake()
    {
       
        lineRenderer = GetComponentInChildren<LineRenderer>();
        _playerMovement = GetComponent<PlayerMovement>();
        lineRenderer.enabled = true;
    }
    public void SetIsGrappleKeyPressed(bool value)
    {
        isGrappleKeyPressed = value;
        if (!isGrappling && grappleTimeElapsed == 0.0f)
        {
            StartGrapple();
        } 
    }
    private void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position,cam.transform.forward, out hit, maxGrappleDistance,whatIsGrappable))
        {
            grappleHitPoint = hit.point;
            Instantiate(sparkPrefab, hit.transform.position, Quaternion.Euler(90f, 0f, 0f));
            GenerateZapSoundFromGrappleSource(hit);
            ExecuteGrapple();
        }
    }

    private void GenerateZapSoundFromGrappleSource(RaycastHit hit)
    {
        AudioSource grappleZapSoundOrigin = hit.transform.gameObject.GetComponent<AudioSource>();
        if (grappleZapSoundOrigin)
        {
            if (grappleZapSoundOrigin.isPlaying) return;
            grappleZapSoundOrigin.Play();
        };
    }

    // draw lineRenderer to `hit point` from gun tip to hit point
    private void DrawLineToHitPoint()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, grappleHitPoint);
    }

    private void ExecuteGrapple()
    {
        isGrappling = true;
    }

    private void StopGrapple()
    {
        isGrappling = false;
        grappleTimeElapsed = 0.0f;
    }

    private void ResetLineRenderer()
    {
        lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));
        lineRenderer.enabled = true;
    }
    private void LateUpdate()
    {
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, grappleHitPoint);
        }   
    }

    private void Update()
    {
        MoveTowardsHitPoint();
    }


    private void MoveTowardsHitPoint()
    {
        if (isGrappling)
        {
            Vector3 direction = grappleHitPoint - transform.position;
            _playerMovement.AddLinearForceTowards(direction * grappleLinearForce);
        }

        if (Vector3.Distance(grappleHitPoint,transform.position) < stopGrappleAtThisDistance && isGrappling )
        {
            StopGrapple();
            
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(maxGrappleDistance, 0, 0), 5.0f);
        Gizmos.DrawWireSphere(grappleHitPoint, 5.0f);
    }
    private void OnGUI()
    {
        
    }
}
