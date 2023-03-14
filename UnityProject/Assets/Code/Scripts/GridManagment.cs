using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagment : MonoBehaviour
{
    private int rows = 5;
    private int columns = 1;
    private float tileSize = 1;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        GameObject blankTile = (GameObject)Instantiate(Resources.Load("BlankTile"));
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject tile = (GameObject)Instantiate(blankTile, transform);

                float psX = col * tileSize;
                float psY = row * -tileSize;

                tile.transform.position = new Vector2(psX, psY);
            }
        }

        Destroy(blankTile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
