using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santoso : MonoBehaviour
{
    [SerializeField] private AIDestinationSetter aiDestinationSetter;
    [SerializeField] private GameObject explosionEffect;

    private Entity entity;

    private void Start()
    {
        aiDestinationSetter.target = GameObject.Find("Player").transform;
        entity = GetComponent<Entity>();
        // subscribe to the death event
        entity.OnDeath += Death;
    }

    // collider enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // debug log
        Debug.Log("Santoso collided with " + collision.name);

        // player
        if (collision.gameObject.tag == "Player")
        {
            Explode(collision.gameObject);
        }
    }

    private void Death()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }

    public void Explode(GameObject target)
    {
        target.GetComponent<PlayerController>().TakeDamage(1);
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
