using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class NewPlayerController : MonoBehaviour
{
    public Vector2 playerSize;
    [Header("Stats")]
    public float moveSpeed = 3f;
    public float jumpVelocity = 1f;
    public float jumpGravity = 2f;
    public float fallGravity = 2.5f;
    [Range(0, 1)]
    public float wallSlideSpeed = 1f;
    public float boxCollisionWidth = 0.05f;
    public LayerMask mask;

    private Rigidbody2D rBody;
    Vector3 respawnPoint;
    //Vector2 playerSize;
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
    bool touchingIce = false;


    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        rBody.freezeRotation = true;
        Time.fixedDeltaTime = 1f / 120f;
        //playerSize = GetComponent<BoxCollider2D>().size;
        groundBoxSize = new Vector2(playerSize.x - 0.05f, boxCollisionWidth);
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

        if (jumpButtonPressed && !wallSliding && (onRightWall||onLeftWall)&&!touchingIce)
        {
            WallSlideJump();
        }

        if (!wallSliding)
        {
            Move(direction);
        }

        if (wallSliding && !touchingIce)
        {
            WallSlide();
        }
    }

    void Move(Vector2 direction)
    {
        Vector2 vel = new Vector2(direction.x * moveSpeed, rBody.velocity.y);

        //check to make sure that you are not moving into a wall
        if (onLeftWall && direction.x < 0)
            vel = new Vector2(0,rBody.velocity.y);
        if (onRightWall && direction.x > 0)
            vel = new Vector2(0, rBody.velocity.y);

        rBody.velocity = vel;

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

    void WallSlide()
    {
        rBody.velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().AddForce(Vector2.down * 0.1f * wallSlideSpeed, ForceMode2D.Impulse);
    }

    void WallSlideJump()
    {
        Debug.Log("Jumping off walls");
        rBody.velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);
        //rBody.velocity = Vector2.Lerp(rBody.velocity, (new Vector2(direction.x * moveSpeed, rBody.velocity.y)), .5f * Time.deltaTime);
        jumpButtonPressed = false;
        onGround = false;
    }

    void CollisionCheck()
    {
        touchingIce = isTouchingIce();

        //These update the positions of the Overlap Boxes. The player has three of these boxes that it uses to check collisions.
        Vector2 groundBoxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + groundBoxSize.y) * 0.5f;
        Collider2D groundCollider = Physics2D.OverlapBox(groundBoxCenter, groundBoxSize, 0f, mask);
        onGround = (groundCollider != null);
        DrawBoxCast(groundBoxCenter,groundBoxSize,transform.up*-1,mask);

        Vector2 leftWallBoxCenter = (Vector2)transform.position + Vector2.left * (playerSize.x + leftWallBoxSize.x) * 0.5f;
        Collider2D leftWallCollider = Physics2D.OverlapBox(leftWallBoxCenter, leftWallBoxSize, 0f, mask);
        onLeftWall = (leftWallCollider != null);
        DrawBoxCast(leftWallBoxCenter, leftWallBoxSize, transform.right * -1, mask);

        Vector2 rightWallBoxCenter = (Vector2)transform.position + Vector2.right * (playerSize.x + rightWallBoxSize.x) * 0.5f;
        Collider2D rightWallCollider = Physics2D.OverlapBox(rightWallBoxCenter, rightWallBoxSize, 0f, mask);
        onRightWall = (rightWallCollider != null);

        DrawBoxCast(rightWallBoxCenter, rightWallBoxSize, transform.right, mask);

        //These manage when the player slides on walls.
        if ((onLeftWall && !onGround && (direction.x < -0.5f)))
        {
            wallSliding = true;
        }
        else if (onRightWall && !onGround && (direction.x > 0f))
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (touchingIce)
        {
            wallSliding = false;
        }

    }
    private bool isTouchingIce()
    {
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position,Vector3.left,playerSize.x/2+0.05f);
        Debug.DrawRay(transform.position,Vector3.left*(playerSize.x / 2 + 0.05f),Color.red );
        if (hit.transform != null && hit.transform.gameObject.tag == "Ice")
            return true;
        hit = Physics2D.Raycast(transform.position, Vector3.right, playerSize.x / 2 +0.05f);
        Debug.DrawRay(transform.position, Vector3.right * (playerSize.x / 2 +0.05f), Color.red);
        if (hit.transform != null && hit.transform.gameObject.tag == "Ice")
            return true;

        return false;
    }


    //For Obstacle collision, please rename to desired layers.

        //this functionality will be moved to a different script
    /*
    void OnCollisionEnter2D(Collision2D collision)
    { 
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Debug.Log("Collided with Obstacle");
            respawnPoint = new Vector3(0, 0, 0);
            Respawn();
        }
    }

    void Respawn()
    {
        this.transform.position = respawnPoint;
    }
    */

    //debug stuff
    public static void DrawBoxCast(Vector2 origin, Vector2 size, Vector2 direction, int mask)
    {
        //RaycastHit2D hit = Physics2D.BoxCast(origen, size, angle, direction, distance, mask);
        float angle = 0.0f;
        float distance = 0.0f;
        //Setting up the points to draw the cast
        Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
        float w = size.x * 0.5f;
        float h = size.y * 0.5f;
        p1 = new Vector2(-w, h);
        p2 = new Vector2(w, h);
        p3 = new Vector2(w, -h);
        p4 = new Vector2(-w, -h);

        Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        p1 = q * p1;
        p2 = q * p2;
        p3 = q * p3;
        p4 = q * p4;

        p1 += origin;
        p2 += origin;
        p3 += origin;
        p4 += origin;

        Vector2 realDistance = direction.normalized * distance;
        p5 = p1 + realDistance;
        p6 = p2 + realDistance;
        p7 = p3 + realDistance;
        p8 = p4 + realDistance;


        //Drawing the cast
        //Color castColor = hit ? Color.red : Color.green;
        Color castColor = Color.red;
        Debug.DrawLine(p1, p2, castColor);
        Debug.DrawLine(p2, p3, castColor);
        Debug.DrawLine(p3, p4, castColor);
        Debug.DrawLine(p4, p1, castColor);

        Debug.DrawLine(p5, p6, castColor);
        Debug.DrawLine(p6, p7, castColor);
        Debug.DrawLine(p7, p8, castColor);
        Debug.DrawLine(p8, p5, castColor);

        Debug.DrawLine(p1, p5, Color.grey);
        Debug.DrawLine(p2, p6, Color.grey);
        Debug.DrawLine(p3, p7, Color.grey);
        Debug.DrawLine(p4, p8, Color.grey);
        /*
        if (hit)
        {
            Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow);
        }
        */
        //return hit;
    }
}
