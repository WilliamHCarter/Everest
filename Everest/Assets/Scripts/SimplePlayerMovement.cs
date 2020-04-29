using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public int jumpHeight;
    public int speed;

    private Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"),0f);
        if(rb.velocity.magnitude < speed)
            rb.AddForce(direction*speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, 5f), ForceMode2D.Impulse);
        }
    }
   
    
}
