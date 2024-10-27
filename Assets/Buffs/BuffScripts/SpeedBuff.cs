using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : BuffEffects
{
    private void Start()
    {
        buffName = "SpeedBuff";
    }
    public override void BuffEffect(PlayerController player)
    {
        // cast buffValue to float
        player.speed += (float)buffValue;
        Debug.Log("Speed buff effect");
    }
    public override void RemoveBuff(PlayerController player)
    {
        // cast buffValue to float
        player.speed -= (float)buffValue;
        Debug.Log("Speed buff removed");
    }
}
