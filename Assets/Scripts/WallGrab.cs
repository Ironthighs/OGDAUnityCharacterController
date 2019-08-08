using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGrab : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _wallJumpR = new List<Transform>();
    
    [SerializeField]
    private LayerMask _layersToWallJump;

    private bool _canGrabWall = false;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), 0.6f, _layersToWallJump);

        // Does the ray intersect any objects excluding the player layer
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 0.6f, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}