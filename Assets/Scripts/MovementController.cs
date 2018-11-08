using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private const float AXIS_THRESHOLD = 0.02f;
    private const float MAX_X_SPEED = 10f;

    [Range(1, 10)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Vector2 groundRadius = new Vector2(0.2f, 0.2f);

    [SerializeField]
    private LayerMask whatIsGround;

    private bool grounded = true;
    private bool holdingJump = false;
    private Rigidbody2D rb2d;
    private Vector2 inputDir = new Vector2();
    private Vector2 prevVel = new Vector2();
    private Vector2 currentVel = new Vector2();
    private bool pressedJump = false;

    private BoxCollider2D objCollider;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        objCollider = GetComponent<BoxCollider2D>();
    }

    // Input
    // Update
    // Render

    private void Update()
    {
        var hAxis = Input.GetAxis("Horizontal");
        if (hAxis > AXIS_THRESHOLD || hAxis < -AXIS_THRESHOLD)
        {
            inputDir.x = hAxis;
        }
        else
        {
            inputDir.x = 0;
        }
        inputDir.Normalize();
        var vAxis = Input.GetAxis("Vertical");
        if (vAxis > AXIS_THRESHOLD || vAxis < -AXIS_THRESHOLD)
        {
            inputDir.y = 0; // vAxis;
        }
        else
        {
            inputDir.y = 0;
        }
        


        pressedJump = Input.GetButtonDown("Jump");
        holdingJump = Input.GetButton("Jump");
        grounded = IsGrounded();
    }

    private void FixedUpdate()
    {
        currentVel.x = inputDir.x * 800f * Time.fixedDeltaTime;
        rb2d.velocity = new Vector2(currentVel.x, rb2d.velocity.y);
        if (grounded && pressedJump)
        {
            rb2d.velocity = Vector2.up * jumpVelocity;
        }

        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb2d.velocity.y > 0 && !holdingJump)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private bool IsGrounded()
    {
        var collider = Physics2D.OverlapArea(
            new Vector2(groundCheck.position.x - groundRadius.x, groundCheck.position.y - groundRadius.y), 
            new Vector2(groundCheck.position.x + groundRadius.x, groundCheck.position.y + groundRadius.y),  
            whatIsGround);
        Debug.Log(collider != null ? "Is Grounded" : "Not Grounded");
        return collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75f);
        Gizmos.DrawCube(groundCheck.position, groundRadius);
    }

}
