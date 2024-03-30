using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
///  A GameObject that is spawned as an effect of a weapon firing
/// </summary>

public abstract class WeaponEffect : MonoBehaviour
{
    [HideInInspector]
    public PlayerStats owner;
    [HideInInspector]
    public Weapon weapon;

    public float GetDamage()
    {
        return weapon.GetDamage();
    }
}
