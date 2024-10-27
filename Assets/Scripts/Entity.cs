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

    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // find player in the scene
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        maxHealth = startingMaxHealth;
        health = maxHealth;
    }

    private void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        // Move the enemy
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
