using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    [SerializeField]
    Vector2Int tilePosition;

    private void Start()
    {
        GetComponentInParent<TerrainController>().Add(gameObject, tilePosition);
    }
}
