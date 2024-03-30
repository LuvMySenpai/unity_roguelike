using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingRingWeapon : ProjectileWeapon
{
    List<EnemyStats> allSelectedEnemies = new List<EnemyStats>();

    protected override bool Attack(int attackCount = 1)
    {
        //Check if projectile prefab is assigned
        if (!currentStats.hitEffect)
        {
            Debug.LogWarning(string.Format("Hit effect prefab has not been set for {0}", name));
            currentCooldown = data.baseStats.cooldown;
            return false;
        }

        //If there is no projectile assigned, set the weapon on cooldown
        if (!CanAttack()) return false;

        //if this is the first time the attack has been fired,
        //refresh the array of selected enemies
        if (currentCooldown <= 0)
        {
            allSelectedEnemies = new List<EnemyStats>(FindObjectsOfType<EnemyStats>());
            currentCooldown += currentStats.cooldown;
            currentAttackCount = attackCount;
        }

        //Find an enemy in the map to attack
        EnemyStats target = PickEnemy();
        if (target)
        {
            DamageArea(target.transform.position, currentStats.area, GetDamage());
            Instantiate(currentStats.hitEffect, target.transform.position, Quaternion.identity);

            //Play the damage vfx
            attackSource.PlayOneShot(currentStats.attackSound);
        }

        //If there are more than 1 attack count
        if (attackCount > 0)
        {
            currentAttackCount = attackCount - 1;
            currentAttackInterval = currentStats.projectileInterval;
        }

        return true;
    }

    //Randomly picks an enemy on the screen
    EnemyStats PickEnemy()
    {
        EnemyStats target = null;
        while(!target && allSelectedEnemies.Count > 0)
        {
            int idx = Random.Range(0, allSelectedEnemies.Count);
            target = allSelectedEnemies[idx];

            //If the target is alredy dead, remove it and skip it
            if(!target)
            {
                allSelectedEnemies.RemoveAt(idx);
                continue;
            }

            //Check for enemy render and check if it is on the screen
            Renderer r = target.GetComponent<Renderer>();
            if(!r || !r.isVisible)
            {
                allSelectedEnemies.Remove(target);
                target = null;
                continue;
            }
        }

        allSelectedEnemies.Remove(target);
        return target;
    }

    //Deals damage in an area
    void DamageArea(Vector2 position, float radious, float damage)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(position, radious);
        foreach (Collider2D target in targets) 
        {
            EnemyStats es = target.GetComponent<EnemyStats>();
            if (es) es.TakeDamage(damage, transform.position);
        }
    }
}
