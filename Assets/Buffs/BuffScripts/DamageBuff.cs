using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuff : BuffEffects
{
    public override void BuffEffect(PlayerController player)
    {
        player.damageBuff += 1;
    }
}
