using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3;
    public float jumpVelocity = 1;
    public bool jumpButtonPressed;
    public Rigidbody2D rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpButtonPressed |= Input.GetButtonDown("Jump");
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(x,y);
        Walk(direction);
    }

    void FixedUpdate()
    {
        if (jumpButtonPressed)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);

            jumpButtonPressed = false;
        }
    }

    void Walk(Vector2 direction)
    {
        rBody.velocity = new Vector2(direction.x * moveSpeed, rBody.velocity.y);
    }
}
