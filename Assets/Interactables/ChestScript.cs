using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : Interactables
{
    public int price = 0;
    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    public Sprite openChestSprite;
    public List<GameObject> itemList = new List<GameObject>();

    private void Start()
    {
        // Get playercontroller from the scene
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Interact()
    {
        if (player.coins >= price)
        {
            player.coins -= price;
            GameObject itemInstance = Instantiate(
                itemList[Random.Range(0, itemList.Count)], transform.position, Quaternion.identity
            );
            // Launch item
            itemInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 30), ForceMode2D.Impulse);
            spriteRenderer.sprite = openChestSprite;
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }
}
