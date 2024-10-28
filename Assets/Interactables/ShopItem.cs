using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : Interactables
{
    public int price = 30;
    private PlayerController player;
    public GameObject item;
    private BuffEffects buffEffects;
    private SpriteRenderer spriteRenderer;
    private GameObject priceTag;
    private TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
        buffEffects = item.GetComponent<BuffEffects>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = buffEffects.buffSprite;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        priceTag = transform.GetChild(0).gameObject;
        textMesh = priceTag.GetComponent<TextMesh>();
        textMesh.text = "$" + price.ToString();
    }

    public override void Interact()
    {
        if (player.coins >= price)
        {
            player.coins -= price;
            player.UpdateCoins();
            player.playerBuffs.AddBuff(buffEffects, 1);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Not enough coins");
        }
    }
}
