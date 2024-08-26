using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;
    public int additionalDamage = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Destroy the bullet after 2 seconds
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        //GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        // Move the bullet to the right
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet collided with " + collision.name + ".. dmg = " + (damage + additionalDamage));
        Entity entity = collision.GetComponent<Entity>();
        if (entity != null)
        {
            entity.TakeDamage(damage + additionalDamage);
        }

        // Destroy the bullet when it collides with something
        Destroy(gameObject);
    }
}
