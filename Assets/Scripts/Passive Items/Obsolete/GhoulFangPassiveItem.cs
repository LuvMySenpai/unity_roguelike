using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Obsolete script. Do not use")]
public class GhoulFangPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        player.CurrentMight *= 1 + passiveItemData.Multiplier / 100f;
    }
}
