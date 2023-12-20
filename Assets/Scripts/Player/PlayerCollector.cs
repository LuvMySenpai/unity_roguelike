using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    //Object grabbing speed
    public float pullSpeed;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();    
        playerCollector = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        //Set collect radius
        playerCollector.radius = player.CurrentMagnet;    
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.TryGetComponent(out ICollectable collectable))
        {
            //Object grabbing animation
            //Get Rigidbody of an item
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            //Get direction from item to player
            Vector2 forceDirection = (transform.position - col.transform.position).normalized;
            //simulates force??? idk
            rb.AddForce(forceDirection * pullSpeed);

            collectable.Collect();
        }
    }
}
