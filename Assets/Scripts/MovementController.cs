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
    private LayerMask layersToWallJump;

    [SerializeField]
    private Transform groundCheck;

    private Rigidbody2D rb;
    private float inputX;
    private float inputXDirection;
    private bool jumpPressed = false;
    private bool jumpStillPressed = false;
    private bool grounded = false;
    private Vector2 newVel = Vector2.zero;
    private Vector2 pointA, pointB;

    private bool canGrabWall = false;
    private bool isGrabbingWall = false;
    private Vector2 jumpDir = Vector2.one.normalized;
    private float grabbedWallDirection = 0f;

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
        inputXDirection = Mathf.Sign(inputX);
        jumpPressed |= Input.GetButtonDown("Jump");
        jumpStillPressed = Input.GetButton("Jump");
        grounded = IsGrounded();

        canGrabWall = !grounded;

        Debug.Log("Layer mask: " + layersToWallJump.value);

        if(!isGrabbingWall)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right * inputXDirection), 0.6f, layersToWallJump.value);
            // Does the ray intersect any objects excluding the player layer
            if (canGrabWall && hit.collider != null)
            {
                isGrabbingWall = true;
                grabbedWallDirection = inputXDirection;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right * inputXDirection) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            else
            {
                //            isGrabbingWall = false;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right * inputXDirection) * 0.6f, Color.white);
                Debug.Log("Did not Hit");
            }
        }
    }

    private void FixedUpdate()
    {
        newVel.x = inputX * xVelocity;
        newVel.y = rb.velocity.y;

        if(isGrabbingWall)
        {
            Debug.Log("Is Grabbing Wall");
            newVel.x = 0;
            newVel.y = 0;
            rb.gravityScale = 0;

            if(jumpPressed)
            {
                jumpPressed = false;
                newVel.y = jumpDir.y * jumpVelocity;
                newVel.x = jumpDir.x * -grabbedWallDirection * xVelocity;
                isGrabbingWall = false;
                rb.gravityScale = 1;
            }
        }
        else
        {
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

            //newVel.x = newVel.x + (inputX * xVelocity);
            
        }

        rb.velocity = newVel;
    }

    private bool IsGrounded()
    {
        pointA.x = groundCheck.position.x - groundCheckRadius.x;
        pointA.y = groundCheck.position.y - groundCheckRadius.y;
        pointB.x = groundCheck.position.x + groundCheckRadius.x;
        pointB.y = groundCheck.position.y + groundCheckRadius.y;
        var result = Physics2D.OverlapArea(pointA, pointB, whatIsGround.value) != null;
        return result;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(groundCheck.position, groundCheckRadius);
    }

}
