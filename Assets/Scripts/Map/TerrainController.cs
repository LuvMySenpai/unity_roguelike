using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;

    Vector2Int currentTilePosition = new Vector2Int(0, 0);

    [SerializeField]
    Vector2Int playerTilePosition;

    Vector2Int onTileGridPlayerPostion;

    [SerializeField]
    float tileSize = 10f; 

    GameObject[,] terrainTiles;

    [SerializeField]
    int terrainTileHorizontalCount;

    [SerializeField]
    int terrainTileVerticalCount;

    [SerializeField]
    int fovHeght = 3; //field of vision

    [SerializeField]
    int fovWidth = 3; //field of vision

    private void Awake()
    {
        terrainTiles = new GameObject[terrainTileHorizontalCount, terrainTileVerticalCount];
    }

    private void Update()
    {
        playerTilePosition.x = (int)(playerTransform.position.x / tileSize);
        playerTilePosition.y = (int)(playerTransform.position.y / tileSize);

        playerTilePosition.x -= playerTransform.position.x < 0 ? 1 : 0;
        playerTilePosition.y -= playerTransform.position.y < 0 ? 1 : 0;

        if (currentTilePosition != playerTilePosition)
        {
            currentTilePosition = playerTilePosition;

            onTileGridPlayerPostion.x = CalculatePositionOnAxis(onTileGridPlayerPostion.x, true);
            onTileGridPlayerPostion.y = CalculatePositionOnAxis(onTileGridPlayerPostion.y, false);
            UpdateTilesOnScreen();
        }
    }

    private void UpdateTilesOnScreen()
    {
        for (int fov_x = -(fovWidth / 2); fov_x <= fovWidth / 2; fov_x++)
        {
            for (int fov_y = -(fovHeght / 2); fov_y <= fovHeght / 2; fov_y++)
            {
                int tileToUpdate_x = CalculatePositionOnAxis(playerTilePosition.x + fov_x, true);
                int tileToUpdate_y = CalculatePositionOnAxis(playerTilePosition.y + fov_y, false);

                GameObject tile = terrainTiles[tileToUpdate_x, tileToUpdate_y];
                tile.transform.position = CalculateTilePosition(playerTilePosition.x + fov_x, playerTilePosition.y + fov_y);
            }
        }
    }

    private Vector3 CalculateTilePosition(int x, int y)
    {
        return new Vector3(x * tileSize, y * tileSize, 0f);
    }

    private int CalculatePositionOnAxis(float currentValue, bool isHorizontal)
    {
        if(isHorizontal)
        {
            if(currentValue >= 0)
            {
                currentValue = currentValue % terrainTileHorizontalCount;
            }
            else
            {
                currentValue += 1;
                currentValue = terrainTileHorizontalCount - 1 + currentValue % terrainTileVerticalCount;
            }
        }
        else
        {
            if (currentValue >= 0)
            {
                currentValue = currentValue % terrainTileVerticalCount;
            }
            else
            {
                currentValue += 1;
                currentValue = terrainTileVerticalCount - 1 + currentValue % terrainTileVerticalCount;
            }
        }

        return (int)currentValue;
    }

    public void Add(GameObject tileGameObject, Vector2Int tilePosition)
    {
        terrainTiles[tilePosition.x, tilePosition.y] = tileGameObject;
    }
}
