using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// script to hold the player's buffs
public class PlayerBuffs : MonoBehaviour
{
    public PlayerController playerController;

    // map of buffs to their effects
    public Dictionary<BuffEffects, int> buffs = new Dictionary<BuffEffects, int>();
    // panel UI to display the buffs
    public GameObject panel;
    // image UI to display the buffs
    public GameObject image;
    // Array of Image UI to display the buffs
    public GameObject[] images;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void AddBuff(BuffEffects buffEffects, int amount)
    {
        // log something to the console
        Debug.Log("Adding buff: " + buffEffects.buffName + " with value: " + amount);
        // check if the buff is already in the dictionary
        if (buffs.ContainsKey(buffEffects))
        {
            // if it is, add the amount to the existing value
            buffs[buffEffects] += amount;

        }
        else
        {
            // if it isn't, add the buff to the dictionary
            buffs.Add(buffEffects, amount);
            // add image as child of panel
            GameObject buffimage = Instantiate(image, panel.transform);
            // change source image to buff sprite
            buffimage.GetComponent<UnityEngine.UI.Image>().sprite = buffEffects.buffSprite;
            // reparent the image to the panel
            buffimage.transform.SetParent(panel.transform);
        }

        // apply the buff effect
        buffEffects.BuffEffect(playerController);
    }
}
