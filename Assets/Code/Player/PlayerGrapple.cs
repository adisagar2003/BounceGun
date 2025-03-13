using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private float maxGrappleDistance = 5000.3f;
    [SerializeField] private float grappleDuration = 1.2f;
    [SerializeField] private float grappleHeightOffset = 3.2f;
    [SerializeField] private float grappleLinearForce = 310.2f;
    [SerializeField] private float stopGrappleAtThisDistance = 10.0f;
    
    [Header("Grapple Points")]
    [SerializeField] private LayerMask whatIsGrappable;
    [SerializeField] private Vector3 grappleHitPoint;
    [SerializeField] private Camera cam;

    [Header("Line Hanlde")]
    [SerializeField] private GameObject lineRendererComponent;
    [SerializeField] private LineRenderer lineRenderer;

    private void Awake()
    {
       
        lineRenderer = lineRendererComponent.GetComponent<LineRenderer>();
        _playerMovement = GetComponent<PlayerMovement>();
        lineRenderer.enabled = false;
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
            DrawLineToHitPoint();
            ExecuteGrapple();
        }
    }

    private void DrawLineToHitPoint()
    {
        lineRenderer.SetPosition(0, gunTip.transform.position);
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
        ResetLineRenderer();
        Debug.Log("GrappleStopped");
    }
    private void ResetLineRenderer()
    {
        lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));
        lineRenderer.enabled = false;
    }
    private void LateUpdate()
    {
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, gunTip.position);
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
            Debug.Log("grappling moving towards the target hit point");
            Vector3 direction = grappleHitPoint - transform.position;
            _playerMovement.AddLinearForceTowards(direction * grappleLinearForce);
        }

        if (Vector3.Distance(grappleHitPoint,transform.position) < stopGrappleAtThisDistance && isGrappling )
        {
            StopGrapple();
        } 
    }

    private void OnGUI()
    {
        
    }
}
