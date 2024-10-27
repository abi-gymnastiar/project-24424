using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemBuffMaster : MonoBehaviour
{
    public Collider2D trigCol;
    public BuffEffects buffEffects;
    private bool flag = false;

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
        if (collision.gameObject.tag == "Player" && !flag)
        {
            flag = true;
            collision.gameObject.GetComponent<PlayerController>().playerBuffs.AddBuff(buffEffects, 1);
            Destroy(gameObject);
        }
    }
}

public class BuffEffects : MonoBehaviour
{
    public string buffName;
    public int buffValue;
    // sprite image
    public Sprite buffSprite;

    public virtual void BuffEffect(PlayerController player)
    {
        Debug.Log("Default buff effect");
    }

    public virtual void RemoveBuff(PlayerController player)
    {
        Debug.Log("Default remove buff effect");
    }
}
