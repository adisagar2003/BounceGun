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

    [Header("Grapple Controls")]
    [SerializeField] private float grappleTimeElapsed = 0.0f;
    [SerializeField] private float grappleTimeCooldown = 1.3f;
    [SerializeField] private float grappleStartAfterThisManySeconds = 0.5f;
    [SerializeField] private float maxGrappleDistance = 5000.3f;
    [SerializeField] private float grappleForce = 100.3f;
    [SerializeField] private float grappleUpwardForce = 100.3f;

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
        isGrappling = true;
        grappleTimeElapsed = 2.0f;

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position,cam.transform.forward, out hit, maxGrappleDistance,whatIsGrappable))
        {
            grappleHitPoint = hit.point;
            DrawLineToHitPoint();
            Invoke(nameof(ExecuteGrapple), grappleStartAfterThisManySeconds);
        }
    }

    private void DrawLineToHitPoint()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, grappleHitPoint);
    }

    private void ExecuteGrapple()
    {
        if (isGrappling)
        {
            // multiplying with -1;
            Vector3 directionTowardsGrapplePoint = (grappleHitPoint - cam.transform.position).normalized;
            _playerMovement.AddImpulsiveForceTowards(directionTowardsGrapplePoint * grappleForce);
            _playerMovement.AddImpulsiveForceTowards(Vector3.up * grappleUpwardForce);
        }
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
        if (grappleTimeElapsed > 0.0f)
        {
            grappleTimeElapsed -= Time.deltaTime;

            if (grappleTimeElapsed < 0.0f)
            {
                StopGrapple();
            }
           
        }

        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.red);
    }

    private void OnGUI()
    {
        
    }
}
