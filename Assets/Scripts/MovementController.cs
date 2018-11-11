using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    
    public float xVelocity = 140f;
    public float jumpVelocity = 500f;
    public Vector2 groundCheckRadius;
    public LayerMask whatIsGround;

    public float fallMultiplier = 5f;
    public float lowJumpMultiplier = 2.5f;

    [SerializeField]
    private Transform groundCheck;

    private Rigidbody2D rb;
    private float inputX;
    private bool jumpPressed = false;
    private bool jumpStillPressed = false;
    private bool grounded = false;
    private Vector2 newVel = Vector2.zero;
    private Vector2 pointA, pointB;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        pointA = new Vector2(groundCheck.position.x - groundCheckRadius.x, groundCheck.position.y - groundCheckRadius.y);
        pointB = new Vector2(groundCheck.position.x + groundCheckRadius.x, groundCheck.position.y + groundCheckRadius.y);
        Debug.Log("Init");
        Debug.Log(pointA);
        Debug.Log(pointB);
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        jumpPressed |= Input.GetButtonDown("Jump");
        jumpStillPressed = Input.GetButton("Jump");
        grounded = IsGrounded();
    }

    private void FixedUpdate()
    {
        newVel.x = inputX * xVelocity; 
        newVel.y = rb.velocity.y;
        if(jumpPressed && grounded)
        {
            jumpPressed = false;
            newVel.y += jumpVelocity;
        }

        if (newVel.y < 0)
        {
            newVel.y += Physics2D.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
        else if (newVel.y > 0 && !jumpStillPressed)
        {
            newVel.y += Physics2D.gravity.y * lowJumpMultiplier * Time.fixedDeltaTime;
        }
        rb.velocity = newVel;
    }

    private bool IsGrounded()
    {
        pointA.x = groundCheck.position.x - groundCheckRadius.x;
        pointA.y = groundCheck.position.y - groundCheckRadius.y;
        pointB.x = groundCheck.position.x + groundCheckRadius.x;
        pointB.y = groundCheck.position.y + groundCheckRadius.y;
        Debug.Log(pointA);
        Debug.Log(pointB);
        var result = Physics2D.OverlapArea(pointA, pointB, whatIsGround) != null;
        return result;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(groundCheck.position, groundCheckRadius);
    }

}
