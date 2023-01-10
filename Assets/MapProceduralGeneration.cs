using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapProceduralGeneration : MonoBehaviour
{
    public int groundWidth;
    public int groundHeight;

    public Tilemap groundTilemap;
    public Tile groundTile;



    private void Awake()
    {
        generateGround();
    }

    public void generateGround()
    {
        for (int x = 0; x < groundWidth; x++)
        {
            for (int y = 0; y < groundHeight; y++)
            {
                groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTile);

            }
        }
    }
}
