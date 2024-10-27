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
    private bool isOpen = false;

    private void Start()
    {
        // Get playercontroller from the scene
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Interact()
    {
        if (player.coins >= price && !isOpen)
        {
            player.coins -= price;
            player.UpdateCoins();
            GameObject itemInstance = Instantiate(
                itemList[Random.Range(0, itemList.Count)], transform.position, Quaternion.identity
            );
            // Launch item
            itemInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 30), ForceMode2D.Impulse);
            spriteRenderer.sprite = openChestSprite;
            isOpen = true;
        }
        else if (isOpen)
        {
            Debug.Log("Chest is already open");
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }
}
