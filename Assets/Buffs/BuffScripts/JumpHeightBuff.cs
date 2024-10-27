using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHeightBuff : BuffEffects
{
    private void Start()
    {
        buffName = "JumpBuff";
        buffValue = 3; // remove later
    }
    public override void BuffEffect(PlayerController player)
    {
        player.jumpingPower += (float)buffValue;
        Debug.Log("Jump height buff effect");
    }
    public override void RemoveBuff(PlayerController player)
    {
        player.jumpingPower -= (float)buffValue;
        Debug.Log("Jump height buff removed");
    }
}
