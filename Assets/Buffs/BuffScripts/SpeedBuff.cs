using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : BuffEffects
{
    public override void BuffEffect(PlayerController player)
    {
        player.speed += 2;
        Debug.Log("Speed buff effect");
    }
}
