using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int startingMaxHealth = 4;
    public int maxHealth = 4;
    public int health;
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

    public int jumpCount = 2; // 1 for single jump, 2 for double jump, etc.
    private int currentJumpCount = 0;

    public int damageBuff = 0;
    public int coins = 0;
    // text UI for coins using legacy UI
    [SerializeField] private UnityEngine.UI.Text coinsText;
    [SerializeField] private GameObject healthPanel;
    [SerializeField] private GameObject healthImage;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private Animator animator;
    [SerializeField] private CapsuleCollider2D coll;
    [SerializeField] private BoxCollider2D interactCollider;
    public PlayerBuffs playerBuffs;
    private GameObject currentOneWayPlatform;

    // coyote time
    public float coyoteTime = 0.2f;
    private float coyoteCounter;

    void Start()
    {
        maxHealth = startingMaxHealth;
        health = maxHealth;
        playerBuffs = GetComponent<PlayerBuffs>();
        collStandingHeight = coll.size.y;
        collStandingOffset = coll.offset.y;
        UpdateCoins();
        UpdateHealth();
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

        // onAir
        animator.SetFloat("yVelocity", rb.velocity.y);

        // coyote time
        if (IsGrounded())
        {
            coyoteCounter = coyoteTime;
            currentJumpCount = 0;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (coyoteCounter > 0f || (currentJumpCount > 0 && currentJumpCount < jumpCount))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                Debug.Log("Jumping... currentJump = " + currentJumpCount);
            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            currentJumpCount++;
            coyoteCounter = 0f;
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
            bulletSpawn.GetComponent<BulletSpawn>().SpawnBullet(damageBuff);
        }

        // Dashing input
        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(interactCollider.bounds.center, interactCollider.bounds.size, 0f);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Interactables"))
                {
                    hitCollider.GetComponent<Interactables>().Interact();
                }
            }
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

    public void UpdateCoins()
    {
        coinsText.text = coins.ToString();
    }

    public void UpdateHealth()
    {
        // Clear the health panel
        foreach (Transform child in healthPanel.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < health; i++)
        {
            Image image = Instantiate(healthImage, healthPanel.transform).GetComponent<Image>();
            image.transform.SetParent(healthPanel.transform);
        }
    }
}