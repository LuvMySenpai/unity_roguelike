using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base scrpit for all weapons
/// </summary>
public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;

    protected NewBehaviourScript pm;

    protected virtual void Start()
    {
        pm = FindObjectOfType<NewBehaviourScript>();
        currentCooldown = weaponData.CooldownDuration;
    }

    
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;

        if(currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = weaponData.CooldownDuration;
    }
}
