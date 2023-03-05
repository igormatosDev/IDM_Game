using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDisplay : MonoBehaviour
{
    public TileBase[] tiles;
    public Tilemap tilemap;
    public Transform objectsParentFolder;
    [SerializeField] private List<GameObjectType> gameObjectTypes;

    public int tileSize = 1;
    public float distanceFromEdge = 1f;

    //public GameObject prefabTree;
    //public float treeSpacing;
    //public float treeOffset;
    //public float treeNoiseScale;
    //public float treeNoiseThreshold;


    //public GameObject prefabRock;
    //public float rockSpacing;
    //public float rockOffset;
    //public float rockNoiseScale;
    //public float rockNoiseThreshold;



    public void DrawMap(float[,] noiseMap)
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
                Vector3Int tilePosition = new Vector3Int(x * tileSize, y * tileSize, 0);
                tilemap.SetTile(tilePosition, tile);
            }
        }
    }

    public void SpawnObjects()
    {
        for (int i = 0; i < gameObjectTypes.Count; i++)
        {
            SpawnObjectsOnTiles(
                tiles[2],
                gameObjectTypes[i].prefab,
                gameObjectTypes[i].spacing,
                gameObjectTypes[i].offset,
                gameObjectTypes[i].noiseScale,
                gameObjectTypes[i].noiseThreshold
            );
        }
        //SpawnObjectsOnTiles(tiles[2], prefabRock, rockSpacing, rockOffset, rockNoiseScale, rockNoiseThreshold);
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

    public void SpawnObjectsOnTiles(TileBase tile, GameObject objectToSpawn, float spacing, float offset, float noiseScale, float noiseThreshold)
    {
        // First, get the bounds of the land tiles
        BoundsInt bounds = tilemap.cellBounds;
        List<Vector3Int> landTilePositions = new List<Vector3Int>();
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {

                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (tilemap.GetTile(tilePos) == tile)
                {
                    landTilePositions.Add(tilePos);
                }
            }
        }

        GameObject objectFolder = new GameObject(objectToSpawn.name);
        objectFolder.transform.parent = objectsParentFolder;
        // Then, use Perlin noise to determine which land tiles to spawn objects on

        foreach (Vector3Int tilePos in landTilePositions)
        {
            float xOffset = Random.Range(0f, 0.1f); // Random offset for x coordinate
            float yOffset = Random.Range(0f, 0.1f); // Random offset for y coordinate
            float noiseValue = Mathf.PerlinNoise((tilePos.x + xOffset) * noiseScale, (tilePos.y + yOffset) * noiseScale);
            if (noiseValue >= noiseThreshold)
            {
                Vector3 tileCenter = tilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0f);
                Vector3 posInt = tileCenter + new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 0f);
                Vector3Int spawnPos = Vector3Int.RoundToInt(posInt);

                if (IsNearLandEdge(spawnPos, landTilePositions, distanceFromEdge))
                {

                    Instantiate(objectToSpawn, spawnPos, Quaternion.identity, objectFolder.transform);

                    if (spacing > 0)
                    {
                        for (float i = spacing; i < offset; i += spacing)
                        {
                            Vector3 additionalPos = spawnPos + new Vector3(Random.Range(-i, i), Random.Range(-i, i), 0f);
                            Instantiate(objectToSpawn, additionalPos, Quaternion.identity, objectFolder.transform);
                        }
                    }
                }

            }
        }
    }

    private void ClearMap()
    {
        tilemap.ClearAllTiles();
        ClearAllChildren(objectsParentFolder.gameObject);
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

    public bool IsNearLandEdge(Vector3Int tilePos, List<Vector3Int> landTilePositions, float maxDistanceFromEdge)
    {
        foreach (Vector3Int landTilePos in landTilePositions)
        {
            float xDist = Mathf.Abs(landTilePos.x - tilePos.x);
            float yDist = Mathf.Abs(landTilePos.y - tilePos.y);

            if (xDist <= maxDistanceFromEdge && xDist < yDist)
            {
                return true;
            }
            else if (yDist <= maxDistanceFromEdge && yDist < xDist)
            {
                return true;
            }
        }

        return false;
    }
}
