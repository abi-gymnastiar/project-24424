using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private PlayerController player;
    public int coinsDrop = 5;
    public int startingMaxHealth = 4;
    public int maxHealth = 4;
    public int health;
    public int attack;
    public int defense;
    public float speed = 4;
    public int horizontal;
    public bool isFacingRight = true;
    public bool isFacingPlayer = false;

    public Rigidbody2D rb;

    private Vector3 startingScale;

    // Start is called before the first frame update
    void Start()
    {
        // find player in the scene
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        maxHealth = startingMaxHealth;
        health = maxHealth;
        startingScale = transform.localScale;
    }

    private void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        // Move the enemy
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (isFacingPlayer)
        {
            if (player.transform.position.x < transform.position.x)
            {
                gameObject.transform.localScale = new Vector3 (-1 * startingScale.x, startingScale.y, startingScale.z);
            }
            else
            {
                gameObject.transform.localScale = new Vector3(startingScale.x, startingScale.y, startingScale.z);
            }
        }
    }

    public void TakeDamage (int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        player.coins += coinsDrop;
        player.UpdateCoins();
        Destroy(gameObject);
    }

    public void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}
