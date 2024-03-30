using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Obsolete script. Do not use")]
public class SaltController : WeaponController
{
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedSalt = Instantiate(weaponData.Prefab);
        spawnedSalt.transform.position = transform.position;
        spawnedSalt.transform.parent = transform;
    }
}
