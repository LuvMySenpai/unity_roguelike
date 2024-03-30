using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Obsolete script. Do not use")]
public class KnifeBehaviour : ProjectileWeaponBehaviour
{
    protected override void Start()
    {
        base.Start();
    }

    
    void Update()
    {
        transform.position += direction * currentSpeed * Time.deltaTime;
    }
}
