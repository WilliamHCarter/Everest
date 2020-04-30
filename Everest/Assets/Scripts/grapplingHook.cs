using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(DistanceJoint2D))]

public class grapplingHook : MonoBehaviour
{
    public int throwDistance;
    private GameObject ropeTargetSprite;

    private Vector2 pivotPoint;
    private LineRenderer ropeRenderer;
    private DistanceJoint2D joint;

    // Start is called before the first frame update
    void Start()
    {
        ropeTargetSprite = GameObject.FindGameObjectWithTag("RopeTarget");
        ropeRenderer = GetComponent<LineRenderer>();
        pivotPoint = Vector2.zero;
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        drawRope();
        ropeTargetIndicator();

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = (mousePos - playerPos).normalized;

            RaycastHit2D hit = Physics2D.Raycast(playerPos, direction, throwDistance); //when the player clicks anywhere raycast in that direction
            if(hit.transform != null && canBeGrappledOnTo(hit.transform.gameObject)) //if the raycast hits a game object that can be grappled on to
            {
                //then grab on to that object
                pivotPoint = hit.point;
                joint.enabled = true;
                joint.connectedAnchor = pivotPoint;
                joint.maxDistanceOnly = true;
                joint.enableCollision = true;
            }
            else //if there is no object in range that the player can grab on to 
            {
                //disable distance joint
                joint.enabled = false;
                pivotPoint = Vector2.zero;
            }

            /*
            if(hit.transform != null && canBeGrappledOnTo(hit.transform.gameObject)) //if the player clicked on an object that can be grappled on to
            {
                Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
                Vector2 direction = (mousePos - playerPos).normalized;
                hit = Physics2D.Raycast(playerPos, direction, throwDistance); //raycast in the direction of the object
                if (hit.transform != null && canBeGrappledOnTo(hit.transform.gameObject)) //if the raycast hit a game object and that game object can be grappled on to
                {
                    pivotPoint = hit.point;
                    joint.enabled = true;
                    joint.connectedAnchor = pivotPoint;
                    joint.maxDistanceOnly = true;
                    joint.enableCollision = true;
                }
            }
            else //if the player clicked in the air or on an object that cannot be grappled on to
            {
                //disable distance joint
                joint.enabled = false;
                pivotPoint = Vector2.zero;
            }
            */

        }

        if (Input.GetMouseButtonDown(1)) //if right click then disable distance joint
        {
            joint.enabled = false;
            pivotPoint = Vector2.zero;
        }

        if (pivotPoint != Vector2.zero)
        {
            Debug.DrawLine(transform.position, pivotPoint, Color.red);
            ropeRenderer.enabled = true;
            drawRope();
        }
        else
        {
            ropeRenderer.enabled = false;
        }
        
    }
    private void drawRope()
    {
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        ropeRenderer.startWidth = 0.05f;
        ropeRenderer.endWidth = 0.05f;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, playerPos);
        ropeRenderer.SetPosition(1, pivotPoint);
        
    }
    private bool canBeGrappledOnTo(GameObject obj)
    {
        return obj.tag == "Snow" || obj.tag == "Ice";      
    }

    private void ropeTargetIndicator()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 targetPos;
        Vector2 direction = (mousePos - playerPos).normalized;
        Color col = Color.red;


        RaycastHit2D hit = Physics2D.Raycast(playerPos,direction,throwDistance); //raycast towards the object that the player is pointing at
        if (hit.transform != null) 
        {
            targetPos = hit.point;  
            if (canBeGrappledOnTo(hit.transform.gameObject))
                col = Color.green;
        }
        else
        {
            targetPos = playerPos + (direction * throwDistance);
            /*
            if(Vector2.Distance(playerPos,hit.point) < Vector2.Distance(playerPos, mousePos))
            {
                targetPos = playerPos + (direction * throwDistance);
            }
            else
            {
                targetPos = mousePos;
            } 
            */
        }


        Debug.DrawLine(playerPos, targetPos, Color.blue);
        targetPos = Camera.main.WorldToScreenPoint(targetPos);
        ropeTargetSprite.transform.position = targetPos;
        ropeTargetSprite.GetComponent<Image>().color = col;
    }

    public void respawn()
    {
        joint.enabled = false;
        pivotPoint = Vector2.zero;
    }
}
