using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuffMaster : MonoBehaviour
{
    public Collider2D trigCol;
    public BuffEffects buffEffects;

    private void Start()
    {
        Invoke("EnableTriggerCollider", 0.5f);
    }

    private void EnableTriggerCollider()
    {
        trigCol.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            buffEffects.BuffEffect(collision.gameObject.GetComponent<PlayerController>());
            Destroy(gameObject);
        }
    }
}

public class BuffEffects : MonoBehaviour
{
    public virtual void BuffEffect(PlayerController player)
    {
        Debug.Log("Default buff effect");
    }
}
