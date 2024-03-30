using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipWeapon : ProjectileWeapon
{
    int currentSpawnCount;
    int currentSpawnYOffset; //if more than 2 whips - start offsetting upwards

    protected override bool Attack(int attackCount = 1)
    {
        //Check if projectile prefab is assigned
        if(!currentStats.projectilePrefab)
        {
            Debug.LogWarning(string.Format("Projectile prefab has not been set for {0}", name));
            currentCooldown = data.baseStats.cooldown;
            return false;
        }

        //If there is no projectile assigned, set the weapon on cooldown
        if (!CanAttack()) return false;

        //if this is the first time the attack has been fired,
        //we reset the currentSpawnCount
        if (currentCooldown <= 0)
        {
            currentSpawnCount = 0;
            currentSpawnYOffset = 0;
        }

        //Otherwise, calculate the angle and offset of spawned projectile
        float spawnDir = Mathf.Sign(movement.lastMovedVector.x) * (currentSpawnCount % 2 != 0 ? -1 : 1);
        Vector2 spawnOffset = new Vector2(
            spawnDir * Random.Range(currentStats.spawnVariance.xMin, currentStats.spawnVariance.xMax),
            currentSpawnYOffset
        );

        //Spawn a copy of the projectile
        Projectile prefab = Instantiate(
            currentStats.projectilePrefab,
            owner.transform.position + (Vector3)spawnOffset,
            Quaternion.identity
        );
        prefab.owner = owner;

        //Play the damage sfx
        attackSource.clip = currentStats.attackSound;
        attackSource.Play();

        //Flip the projectile's sprite
        if (spawnDir < 0)
        {
            prefab.transform.localScale = new Vector3(
                -Mathf.Abs(prefab.transform.localScale.x),
                prefab.transform.localScale.y,
                prefab.transform.localScale.z
            );
            Debug.Log(spawnDir + " | " + prefab.transform.localScale);
        }

        //Assign the stats
        prefab.weapon = this;
        currentCooldown = data.baseStats.cooldown;
        attackCount--;

        //Determine where the next projectile should spawn
        currentSpawnCount++;
        if (currentSpawnCount > 1 && currentSpawnCount % 2 == 0)
            currentSpawnYOffset += 1;

        //Check if another attack is possible
        if (attackCount > 0)
        {
            currentAttackCount = attackCount;
            currentAttackInterval = data.baseStats.projectileInterval;
        }

        return true;
    }
}
