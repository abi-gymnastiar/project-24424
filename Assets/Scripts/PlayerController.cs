using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float speed = 8.0f;
    public float jumpForce = 16.0f;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

    }

    private void FixedUpdate()
    {
        MovePlayer(horizontal * speed);
    }

    private void MovePlayer(float move)
    {
        Vector2 movement = new Vector2(move, rb.velocity.y);
        rb.velocity = movement;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }
}
