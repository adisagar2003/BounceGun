using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{

    [SerializeField] private bool isGrappleKeyPressed = false;
    [SerializeField] private bool isGrappling = false;
    [SerializeField] private Transform gunTip;
    [SerializeField] private float grappleTimeElapsed = 0.0f;
    [SerializeField] private float grappleTimeCooldown = 1.3f;
    [SerializeField] private float grappleStartAfterThisManySeconds = 0.5f;
    [SerializeField] private float maxGrappleDistance = 5000.3f;
    [SerializeField] private LayerMask whatIsGrappable;
    [SerializeField] private Vector3 grappleHitPoint;
    [SerializeField] private Camera cam;
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
        Debug.Log("Grappling Started");
        isGrappling = true;
        grappleTimeElapsed = 2.0f;
        //isGrappling = true;
        // raycast to required target 
        RaycastHit hit;
        
        if (Physics.Raycast(cam.transform.position,cam.transform.forward, out hit, maxGrappleDistance,whatIsGrappable))
        {
            grappleHitPoint = hit.point;
            Debug.Log(grappleHitPoint);
            Invoke(nameof(ExecuteGrapple), grappleStartAfterThisManySeconds);
        }
    }

    private void ExecuteGrapple()
    {
        Debug.Log("Player will be pushed towards the hit point");
    }

    private void StopGrapple()
    {
        isGrappling = false;
        grappleTimeElapsed = 0.0f;
        Debug.Log("GrappleStopped");
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
