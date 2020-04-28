using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{

	[Header("Stats")]
	public float moveSpeed = 3f;
	public float jumpVelocity = 1f;
	public float jumpGravity = 2f;
	public float fallGravity = 2.5f;
	public float wallSlideSpeed = 1f;
	public float boxCollisionWidth = 0.05f;
	public LayerMask mask;

	private Rigidbody2D rBody;
	Vector3 respawnPoint;
    Vector2 playerSize;
	Vector2 groundBoxSize;
	Vector2 groundBoxCenter;
	Vector2 leftWallBoxSize;
	Vector2 leftWallBoxCenter;
	Vector2 rightWallBoxSize;
	Vector2 rightWallBoxCenter;
	Vector2 direction;

	bool jumpButtonPressed;
	bool onGround;
    bool onLeftWall;
    bool onRightWall;
	bool wallSliding;


	void Start()
	{
		rBody = GetComponent<Rigidbody2D>();
		Time.fixedDeltaTime = 1f / 120f;
		playerSize = GetComponent<BoxCollider2D>().size;
		groundBoxSize = new Vector2(playerSize.x-0.05f, boxCollisionWidth);
		leftWallBoxSize = new Vector2(boxCollisionWidth, playerSize.y - 0.05f);
		rightWallBoxSize = new Vector2(boxCollisionWidth, playerSize.y - 0.05f);
	}

	// Update is called once per frame
	void Update()
	{
		jumpButtonPressed |= Input.GetButtonDown("Jump");
	}

	//FixedUpdate is called twice per frame, set at Start.
	void FixedUpdate()
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		direction = new Vector2(x, y);
		CollisionCheck();

		if (jumpButtonPressed && onGround)
		{
			Jump();
		}

		if (jumpButtonPressed && wallSliding)
		{
			WallSlideJump();
		}


		if (!wallSliding)
		{
			Move(direction);
		}

		if (wallSliding)
		{
			WallSlide();
		}
	}

	void Move(Vector2 direction)
	{
		rBody.velocity = new Vector2(direction.x * moveSpeed, rBody.velocity.y);
		if (rBody.velocity.y < 0)
		{
			rBody.gravityScale = fallGravity;
		}

		else if (rBody.velocity.y > 0 && !Input.GetButton("Jump"))
		{
			rBody.gravityScale = jumpGravity;
		}

		else
		{
			rBody.gravityScale = 1f;
		}
	}

	void Jump()
	{
	    GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
		jumpButtonPressed = false;
		onGround = false;
	}


	void CollisionCheck()
	{
        //These update the positions of the Overlap Boxes. The player has three of these boxes that it uses to check collisions.
		Vector2 groundBoxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + groundBoxSize.y) * 0.5f;
        onGround = (Physics2D.OverlapBox(groundBoxCenter, groundBoxSize, 0f, mask) != null);

		Vector2 leftWallBoxCenter = (Vector2)transform.position + Vector2.left * (playerSize.x + leftWallBoxSize.x) * 0.5f;
	    onLeftWall = (Physics2D.OverlapBox(leftWallBoxCenter, leftWallBoxSize, 0f, mask) != null);

		Vector2 rightWallBoxCenter = (Vector2)transform.position + Vector2.right * (playerSize.x + rightWallBoxSize.x) * 0.5f;
		onRightWall = (Physics2D.OverlapBox(rightWallBoxCenter, rightWallBoxSize, 0f, mask) != null);

        //These manage when the player slides on walls.
		if (onLeftWall && !onGround && (Input.GetAxis("Horizontal") < 0))
		{
			wallSliding = true;
		}
		if (onRightWall && !onGround && (Input.GetAxis("Horizontal") > 0))
		{
			wallSliding = true;
		}
		else
		{
			wallSliding = false;
		}

	}
    //For Obstacle collision, please rename to desired layers.
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
		{
			Debug.Log("Collided with Obstacle");
			respawnPoint = new Vector3(0,0,0);
			Respawn();
		}
	}

	void Respawn()
	{
		this.transform.position = respawnPoint;
	}

	void WallSlide()
	{
		rBody.velocity = new Vector2(rBody.velocity.x, -wallSlideSpeed);
	}

	void WallSlideJump()
	{
		GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
		jumpButtonPressed = false;
		onGround = false;
	}
}
