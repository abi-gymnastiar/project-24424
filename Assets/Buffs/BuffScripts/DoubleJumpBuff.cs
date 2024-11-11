using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpBuff : BuffEffects
{
    private void Start()
    {
        buffName = "DoubleJumpBuff";
        buffValue = 1; // remove later
    }

    public override void BuffEffect(PlayerController player)
    {
        player.jumpCount += buffValue;
        Debug.Log("Double jump buffed");
    }

    public override void RemoveBuff(PlayerController player)
    {
        player.jumpCount -= buffValue;
        Debug.Log("Double jump buff removed");
    }
}
