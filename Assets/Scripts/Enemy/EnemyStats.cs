using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    //CurrentStats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    public float despawnDistance = 20f;

    [SerializeField]
    public AudioClip damageSound;

    AudioSource audioSource;

    Transform player;

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1,0,0,1); //color of the damage flash
    public float damageFlashDuration = 0.2f; //duration of the damage flash
    public float deathFadeTime = 0.6f; //duration of the death fade
    Color originalColor;
    SpriteRenderer sr;
    EnemyMovement movement;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;    
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        movement = GetComponent<EnemyMovement>();
    }

    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }    
    }

    public void TakeDamage(float dmg, Vector2 sourcePosition, float knockbackForce = 5f, float knockbackduration = 0.2f)
    {
        audioSource.PlayOneShot(damageSound);

        currentHealth -= dmg;
        StartCoroutine(DamageFlash());

        //Apply the knockback if the force is greater than zero
        if(knockbackForce > 0)
        {
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackForce, knockbackduration);
        }

        if(currentHealth <= 0) 
        {
            Kill();
        }
    }

    //Coroutine function that makes enemy flash when taking damage
    IEnumerator DamageFlash()
    {
        float t = 0;
        t += Time.deltaTime;
        Debug.Log("FLASH");
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originalColor;
    }

    public void Kill()
    {
        StartCoroutine(KillFade());
    }

    IEnumerator KillFade()
    {
        //Wait for the end of frame for some reason
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, origAlpha = sr.color.a;

        //Loop that fires every frame
        while(t < deathFadeTime)
        {
            yield return w;
            t += Time.deltaTime;

            //Set color for this frame
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (1 - t / deathFadeTime) * origAlpha);
        }

        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnDestroy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        es.OnEnemyKilled();
    }

    void ReturnEnemy()
    {
        EnemySpawner es = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }

    public void FlipSprite(bool isFlipped)
    {
        if (isFlipped)
            sr.flipX = true;
        else
            sr.flipX = false;
    }
}
