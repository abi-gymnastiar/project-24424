using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Entity entity;
    //public Transform gunBarrel;
    public Transform patrolCheck;
    public Rigidbody2D rb;

    // enemy states
    public enum EnemyState
    {
        patrolling,
        chasing,
        attacking
    }
    public EnemyState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        entity = GetComponent<Entity>();
        currentState = EnemyState.patrolling;
        entity.horizontal = 1;
    }

    void Update()
    {

        switch (currentState)
        {
            case EnemyState.patrolling:
                Patrolling();
                break;
            case EnemyState.chasing:
                Chasing();
                break;
            case EnemyState.attacking:
                Attacking();
                break;
        }
    }

    private void Patrolling()
    {
        // Raycast to check if there is a wall in front of the enemy
        RaycastHit2D wallCheck = Physics2D.Raycast(patrolCheck.position, patrolCheck.right, 0.25f);
        RaycastHit2D groundCheck = Physics2D.Raycast(patrolCheck.position, Vector2.down, 0.25f);
        // draw
        //Debug.DrawRay(patrolCheck.position, patrolCheck.right, Color.red);
        Debug.DrawRay(patrolCheck.position, Vector2.down, Color.green);
        if (wallCheck.collider != null)
        {
            // debug log what the raycast hit
            Debug.Log(wallCheck.collider.name);

            if (wallCheck.collider.CompareTag("Tilemaps"))
            {
                entity.horizontal = FlipHorizontal(entity.horizontal);
            }
        }
        else if (groundCheck.collider == null)
        {
            entity.horizontal = FlipHorizontal(entity.horizontal);
        }
    }

    private void Chasing()
    {
        //
    }

    private void Attacking()
    {
        //
    }

    private int FlipHorizontal(int horizontal)
    {
        if (horizontal == 1)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}
