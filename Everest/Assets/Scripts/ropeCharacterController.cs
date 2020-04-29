using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DistanceJoint2D))]

public class ropeCharacterController : MonoBehaviour
{


    public float swingForce;
    public float maxSwingSpeed;
    public NewPlayerController playerController;

    private Rigidbody2D rb;
    private DistanceJoint2D joint;
    private bool playerIsUsingHook = false;
    private bool grounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
    }

    
    void Update()
    {
        playerIsUsingHook = joint.enabled;
        if (playerIsUsingHook) { //if the player is using the grappling hook
            playerController.enabled = false;
            float sf = swingForce;
            if (rb.velocity.magnitude > maxSwingSpeed)
                sf = 0;
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Vector2 direction = Vector2.left * sf;
                rb.AddForce(direction, ForceMode2D.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Vector2 direction = Vector2.right * sf;
                rb.AddForce(direction, ForceMode2D.Impulse);
            }
        }
        else //if the player is not using the hook
        {
            if (grounded) //and the player is on the ground
                playerController.enabled = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        grounded = true;
        Debug.Log("Collision!");
        if (!playerIsUsingHook && playerController.enabled == false)
            playerController.enabled = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }

    

}
