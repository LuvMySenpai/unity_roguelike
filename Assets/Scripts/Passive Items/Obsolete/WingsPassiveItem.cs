using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Obsolete script. Do not use")]
public class WingsPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMoveSpeed *= 1 + passiveItemData.Multiplier / 100f;
    }
}
