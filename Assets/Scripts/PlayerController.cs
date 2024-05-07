using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;

    private bool isFacingRight = true;

    private float collStandingHeight;
    private float collStandingOffset;
    private float collCrouchingHeight = 0.5f;
    private float collCrouchingOffset = -0.15f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider2D coll;

    void Start()
    {
        collStandingHeight = coll.size.y;
        collStandingOffset = coll.offset.y;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // Crouching input
        if (Input.GetButtonDown("Crouch"))
        {
            animator.SetBool("isCrouching", true);
            coll.size = new Vector2(coll.size.x, collCrouchingHeight);
            coll.offset = new Vector2(coll.offset.x, collCrouchingOffset);
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            animator.SetBool("isCrouching", false);
            coll.size = new Vector2(coll.size.x, collStandingHeight);
            coll.offset = new Vector2(coll.offset.x, collStandingOffset);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}