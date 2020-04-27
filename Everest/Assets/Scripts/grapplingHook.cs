using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplingHook : MonoBehaviour
{
    public int throwDistance;

    private Vector2 pivotPoint;
    private Vector2 hitPoint;
    private LineRenderer targetRenderer;

    // Start is called before the first frame update
    void Start()
    {
        targetRenderer = GetComponent<LineRenderer>();
        hitPoint = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        targetIndicator();

        if (Input.GetMouseButtonDown(0))
        {
            if (hitPoint != Vector2.zero) //if the player clickes on an object that can be grappled on to
            {
                pivotPoint = hitPoint;
                if(gameObject.GetComponent<DistanceJoint2D>() == null)
                    gameObject.AddComponent<DistanceJoint2D>();
                DistanceJoint2D joint = GetComponent<DistanceJoint2D>();
                joint.enabled = true;
                joint.connectedAnchor = pivotPoint;
                joint.maxDistanceOnly = true;
                joint.enableCollision = true;
            }
            else //if the player clicks in the air or clicks on an object that can't be grappled on to
            {
                if(gameObject.GetComponent<DistanceJoint2D>()!=null)
                    gameObject.GetComponent<DistanceJoint2D>().enabled = false;
                pivotPoint = Vector2.zero;
            }
        }
        if (pivotPoint != Vector2.zero)
            Debug.DrawLine(transform.position, pivotPoint, Color.red);
        
    }
    private void targetIndicator()
    {
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = (mousePos-playerPos).normalized;
        Vector2 endPoint = Vector2.zero;
        
        int layerMask = 1 << 2;
        layerMask = ~layerMask; //collide against everything that is not the "ignoreRaycast" layer
        RaycastHit2D hit = Physics2D.Raycast(playerPos,direction,throwDistance,layerMask);
        Debug.DrawLine(playerPos, hit.point);

        hitPoint = hit.point;

        if(hitPoint != Vector2.zero) //if the ray hit an object within the throwDistance
        {
            endPoint = hitPoint;
        }
        else
        {
            endPoint = playerPos + (direction*throwDistance);
        }

        targetRenderer.startWidth = 0.05f;
        targetRenderer.endWidth = 0.05f;
        targetRenderer.positionCount = 2;
        targetRenderer.SetPosition(0,playerPos);
        targetRenderer.SetPosition(1, endPoint);
    }
    
}
