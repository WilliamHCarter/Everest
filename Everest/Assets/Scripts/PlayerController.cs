using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpVelocity = 1f;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	private Vector2 direction;
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
        direction = new Vector2(x,y);
	}

    void FixedUpdate()
    {
		Jump();
		Walk(direction);
	}

    void Walk(Vector2 direction)
    {
        rBody.velocity = new Vector2(direction.x * moveSpeed, rBody.velocity.y);
    }
    void Jump()
	{
		if (jumpButtonPressed)
		{
			GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
			jumpButtonPressed = false;
		}
		if (rBody.velocity.y < 0)
		{
			rBody.gravityScale = fallMultiplier;
		}
		else if (rBody.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			rBody.gravityScale = lowJumpMultiplier;
		}
		else
		{
			rBody.gravityScale = 1f;
		}
	}
}
