using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDisplay : MonoBehaviour
{
    public TileBase[] tiles;
    public Tilemap tilemap;

    public int tileSize = 1;

    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        tilemap.ClearAllTiles();

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Get the noise value at this location
                float noiseValue = noiseMap[x, y];

                // Determine the tile type based on the noise value
                TileBase tile = ChooseTileBasedOnNoise(noiseValue);

                // Set the tile at the current location
                Vector3Int tilePosition = new Vector3Int(x * tileSize, y * tileSize, 0);
                tilemap.SetTile(tilePosition, tile);
            }
        }
    }

    private TileBase ChooseTileBasedOnNoise(float noiseValue)
    {
        int numTiles = tiles.Length;

        // Define the range of noise values for each tile type
        float rangePerTile = 1f / numTiles;
        float[] rangeStart = new float[numTiles];

        for (int i = 0; i < numTiles; i++)
        {
            rangeStart[i] = i * rangePerTile;
        }

        // Determine the tile type based on the noise value
        for (int i = 0; i < numTiles; i++)
        {
            if (noiseValue >= rangeStart[i] && noiseValue < rangeStart[i] + rangePerTile)
            {
                return tiles[i];
            }
        }

        // If no condition was met, return the first tile as a fallback
        return tiles[0];
    }


}