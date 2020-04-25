using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapplingHook : MonoBehaviour
{
    public GameObject targetUI;

    private Vector2 pivotPoint;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        mousePosIndicator();

        if (Input.GetMouseButtonDown(0))
        {
            if (gameObject.GetComponent<DistanceJoint2D>() != null)
            {
                Destroy(gameObject.GetComponent<DistanceJoint2D>());
            }
            else
            {
                gameObject.AddComponent<DistanceJoint2D>();
                DistanceJoint2D joint = gameObject.GetComponent<DistanceJoint2D>();
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pivotPoint = new Vector2(mousePos.x, mousePos.y);
                joint.connectedAnchor = pivotPoint;
                joint.enableCollision = true;
                joint.maxDistanceOnly = true;
            }
        }

    }
    private void mousePosIndicator()
    {
        if(gameObject.GetComponent<DistanceJoint2D>() != null)
        {
            targetUI.transform.position = Camera.main.WorldToScreenPoint(pivotPoint);   
        }
        else
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            targetUI.transform.position = mousePos;
        }

    }
}
