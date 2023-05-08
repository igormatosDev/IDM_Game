using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class TilemapDisplay : MonoBehaviour
{
    public TileBase[] tiles;
    public Tilemap tilemapIsland;
    public Tilemap tilemapWater;
    public TileBase waterTile;
    public Transform objectsParentFolder;
    [SerializeField] private List<GameObjectType> gameObjectTypes;

    public int tileSize = 1;
    public int distanceFromEdge = 1;


    public void SetTilemap(Tilemap newTilemap)
    {
        tilemapIsland = newTilemap;
    }


    public void DrawMap(float[,] noiseMap, Vector3Int position)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        this.ClearMap();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Get the noise value at this location
                float noiseValue = noiseMap[x, y];

                // Determine the tile type based on the noise value
                TileBase tile = ChooseTileBasedOnNoise(noiseValue);

                // Set the tile at the current location
                Vector3Int tilePosition = position + new Vector3Int(x * tileSize, y * tileSize, 0);
                tilemapIsland.SetTile(tilePosition, tile);
                tilemapWater.SetTile(tilePosition, waterTile);
            }
        }
    }


    public void SpawnObjects(Vector3Int tilemapPosition)
    {
        for (int i = 0; i < gameObjectTypes.Count; i++)
        {
            SpawnObjectsOnTiles(
                tiles[2],
                gameObjectTypes[i].prefab,
                gameObjectTypes[i].spacing,
                gameObjectTypes[i].offset,
                gameObjectTypes[i].noiseScale,
                gameObjectTypes[i].noiseThreshold,
                tilemapPosition
            );
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

    public void SpawnObjectsOnTiles(TileBase tile, GameObject objectToSpawn, float spacing, float offset, float noiseScale, float noiseThreshold, Vector3Int tilemapPosition)
    {
        BoundsInt bounds = tilemapIsland.cellBounds;
        List<Vector3Int> landTilePositions = new List<Vector3Int>();
        List<Vector3> spawnedObjectPositions = new List<Vector3>(); // Keep track of spawned object positions

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);

                if (tilemapIsland.GetTile(tilePos) == tile)
                {
                    landTilePositions.Add(tilePos);
                }
            }
        }

        GameObject objectFolder = new GameObject(objectToSpawn.name);
        objectFolder.transform.parent = objectsParentFolder;

        foreach (Vector3Int tilePos in landTilePositions)
        {
            float xOffset = Random.Range(0f, 0.1f);
            float yOffset = Random.Range(0f, 0.1f);
            float noiseValue = Mathf.PerlinNoise((tilePos.x + xOffset) * noiseScale, (tilePos.y + yOffset) * noiseScale);
            if (noiseValue >= noiseThreshold)
            {
                Vector3 tileCenter = tilemapIsland.CellToWorld(tilePos + tilemapPosition) + new Vector3(0.5f, 0.5f, 0f);
                Vector3 posInt = tileCenter + new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 0f);
                Vector3Int spawnPos = Vector3Int.RoundToInt(posInt);


                if (HasAdjacentSameTile(spawnPos, distanceFromEdge) && !IsSpawnPositionTooClose(spawnPos, spawnedObjectPositions, spacing))
                {
                    GameObject spawnedObject = Instantiate(objectToSpawn, spawnPos, Quaternion.identity, objectFolder.transform);
                    spawnedObjectPositions.Add(spawnedObject.transform.position); // Add the spawned object's position to the list
                }
            }
        }
    }

    private bool IsSpawnPositionTooClose(Vector3 spawnPos, List<Vector3> spawnedObjectPositions, float minDistance)
    {
        foreach (Vector3 objPos in spawnedObjectPositions)
        {
            if (Vector3.Distance(spawnPos, objPos) < minDistance)
            {
                return true;
            }
        }
        return false;
    }


    private void ClearMap()
    {
        // tilemapIsland.ClearAllTiles();
        // tilemapWater.ClearAllTiles();
        // ClearAllChildren(objectsParentFolder.gameObject);
    }

    public void ClearAllChildren(GameObject parent)
    {
        int childCount = parent.transform.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    public bool HasAdjacentSameTile(Vector3Int tilePos, int maxDistanceFromEdge)
    {

        Vector3Int positionInt = tilemapIsland.WorldToCell(tilePos);
        TileBase tile = tilemapIsland.GetTile(positionInt);


        Vector3Int rightPos = tilemapIsland.WorldToCell(tilePos + (Vector3Int.right * maxDistanceFromEdge));
        TileBase rightTile = tilemapIsland.GetTile(rightPos);
        if (rightTile != tile || rightTile == null)
        {
            return false;
        }

        // Check left tile
        Vector3Int leftPos = tilemapIsland.WorldToCell(tilePos + (Vector3Int.left * maxDistanceFromEdge));
        TileBase leftTile = tilemapIsland.GetTile(leftPos);
        if (leftTile != tile || leftTile == null)
        {
            return false;
        }

        // Check up tile
        Vector3Int upPos = tilemapIsland.WorldToCell(tilePos + (Vector3Int.up * maxDistanceFromEdge));
        TileBase upTile = tilemapIsland.GetTile(upPos);
        if (upTile != tile || upTile == null)
        {
            return false;
        }

        // Check down tile
        Vector3Int downPos = tilemapIsland.WorldToCell(tilePos + (Vector3Int.down * maxDistanceFromEdge));
        TileBase downTile = tilemapIsland.GetTile(downPos);
        if (downTile != tile || downTile == null)
        {
            return false;
        }

        return true;
    }

}
