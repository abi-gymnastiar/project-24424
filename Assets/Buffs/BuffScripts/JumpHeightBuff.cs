using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHeightBuff : BuffEffects
{
    public override void BuffEffect(PlayerController player)
    {
        player.jumpingPower *= 2;
        Debug.Log("Jump height buff effect");
    }
}
