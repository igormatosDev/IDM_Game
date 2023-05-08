using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MapGenerator : MonoBehaviour
{
    public int width = 64;
    public int height = 64;
    public float scale = 20f;
    public int octaves = 6;
    public float persistence = 0.5f;
    public float lacunarity = 2f;
    public Vector2 offset = Vector2.zero;
    public TilemapDisplay tilemapDisplay;

    // try to generate chunks
    public int chunksAroundPlayer = 1;
    public int chunkSize = 32;
    private Dictionary<Vector2Int, GameObject> chunkMap = new Dictionary<Vector2Int, GameObject>();
    public Transform player;
    private bool isGeneratingChunks = false;


    private Vector2Int GetPlayerChunkCoordinate()
    {
        int x = Mathf.FloorToInt(player.position.x / chunkSize);
        int y = Mathf.FloorToInt(player.position.y / chunkSize);
        return new Vector2Int(x, y);
    }

    private async void UpdateChunksAroundPlayer()
    {
        if (isGeneratingChunks)
        {
            return;
        }

        Vector2Int playerChunkCoord = GetPlayerChunkCoordinate();

        bool shouldGenerateChunks = false;

        for (int x = -chunksAroundPlayer; x <= chunksAroundPlayer; x++)
        {
            for (int y = -chunksAroundPlayer; y <= chunksAroundPlayer; y++)
            {
                Vector2Int chunkCoord = playerChunkCoord + new Vector2Int(x, y);

                if (!chunkMap.ContainsKey(chunkCoord))
                {
                    shouldGenerateChunks = true;
                    break;
                }
            }

            if (shouldGenerateChunks)
            {
                break;
            }
        }

        if (shouldGenerateChunks)
        {
            isGeneratingChunks = true;
            await Task.Run(() => GenerateChunksAroundPlayerAsync(playerChunkCoord));
            isGeneratingChunks = false;
        }
    }

    private async void GenerateChunksAroundPlayerAsync(Vector2Int playerChunkCoord)
    {
        for (int x = -chunksAroundPlayer; x <= chunksAroundPlayer; x++)
        {
            for (int y = -chunksAroundPlayer; y <= chunksAroundPlayer; y++)
            {
                Vector2Int chunkCoord = playerChunkCoord + new Vector2Int(x, y);

                if (!chunkMap.ContainsKey(chunkCoord))
                {
                    await Task.Run(() =>
                    {
                        MainThreadDispatcher.Instance.EnqueueAction(() =>
                        {
                            Vector2 chunkOffset = new Vector2(chunkCoord.x * chunkSize, chunkCoord.y * chunkSize);
                            float[,] noiseMap = GenerateMap(chunkCoord, chunkOffset);

                            Vector3Int tilemapPosition = new Vector3Int(chunkCoord.x * chunkSize, chunkCoord.y * chunkSize, 0);
                            tilemapDisplay.DrawMap(noiseMap, tilemapPosition);
                            // tilemapDisplay.SpawnObjects(tilemapPosition);
                            GameObject chunkObject = new GameObject($"Chunk_{chunkCoord.x}_{chunkCoord.y}");
                            print(chunkObject.name + $" - {chunkCoord.x}, {chunkCoord.y} - {tilemapPosition} - {chunkOffset}");
                            chunkMap.Add(chunkCoord, chunkObject);
                        });
                    });
                }
            }
        }
    }

    private float[,] GenerateMap(Vector2Int chunkCoord, Vector2 chunkOffset)
    {
        float[,] noiseMap = PerlinNoiseGenerator.GenerateNoiseMap(width, height, scale, octaves, persistence, lacunarity, offset + chunkOffset);
        return noiseMap;
    }


    public void Update()
    {
        UpdateChunksAroundPlayer();
    }
}