using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, ICollectable
{
    public bool hasBeenCollected = false;

    public virtual void Collect()
    {
        hasBeenCollected = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //Destroy item when it touches the player
        if (col.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
