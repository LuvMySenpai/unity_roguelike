using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<NewBehaviourScript>().transform;    
    }

    
    void Update()
    {
        //If enemy currently being knocked back, then process the knockback
        if(knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            //Otherwise, move enemy towards the player
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);

            if (player.transform.position.x < transform.position.x)
                enemy.FlipSprite(true);
            else
                enemy.FlipSprite(false);
		}
    }

    public void Knockback(Vector2 velocity, float duration)
    {
        //ignore the knockback if duration is greater than 0
        if (knockbackDuration > 0) return;

        //Begin the knockback
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
