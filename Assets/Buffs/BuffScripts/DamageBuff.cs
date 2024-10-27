using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuff : BuffEffects
{
    private void Start()
    {
        buffName = "DamageBuff";
        buffValue = 1; // remove later
    }

    public override void BuffEffect(PlayerController player)
    {
        player.damageBuff += buffValue;
        Debug.Log("Damage buffed");
    }

    public override void RemoveBuff(PlayerController player)
    {
        player.damageBuff -= buffValue;
        Debug.Log("Damage buff removed");
    }
}
