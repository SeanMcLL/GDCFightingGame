using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int grid_width, grid_height;

    [SerializeField] private Tile tilePrefab;

    [SerializeField] private Transform cam;


    void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
    {
        for (int x = 0; x < grid_width; x++)
        {
            for (int y = 0; y < grid_height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
            }
        }
    
        cam.transform.position = new Vector3((float)grid_width/2 -0.5f, (float)grid_height/2 -0.5f, -10);
    }
}
