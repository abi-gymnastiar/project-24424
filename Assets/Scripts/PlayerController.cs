using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;

    public bool isFacingRight = true;
    private bool canDash = true;
    private bool isDashing = false;

    private float dashSpeed = 5f;
    private float dashDuration = 0.1f;
    private float dashCooldown = 0.5f;

    private float collStandingHeight;
    private float collStandingOffset;
    private float collCrouchingHeight = 0.5f;
    private float collCrouchingOffset = -0.15f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider2D coll;
    private GameObject currentOneWayPlatform;

    void Start()
    {
        collStandingHeight = coll.size.y;
        collStandingOffset = coll.offset.y;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

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

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }

        // Firing input
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("shooting");
            bulletSpawn.GetComponent<BulletSpawn>().SpawnBullet();
        }

        // Dashing input
        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player collided with " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        CompositeCollider2D platformCollider = currentOneWayPlatform.GetComponent<CompositeCollider2D>();

        Physics2D.IgnoreCollision(coll, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(coll, platformCollider, false);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(rb.velocity.x * dashSpeed, rb.velocity.y);
        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
        rb.velocity = new Vector2(rb.velocity.x / dashSpeed, rb.velocity.y);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}