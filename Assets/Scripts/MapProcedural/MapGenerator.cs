using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

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

    private float lastBuilt = 0f;

    public int moveForce = 3;
    public float actionDelay = .1f;



    //private void Start()
    //{
    //    GenerateMap();
    //}

    private void GenerateMap()
    {

        print("Map generated");
        lastBuilt = 0f;
        float[,] noiseMap = PerlinNoiseGenerator.GenerateNoiseMap(width, height, scale, octaves, persistence, lacunarity, offset);
        tilemapDisplay.DrawNoiseMap(noiseMap);
    }

    public void Update()
    {
        lastBuilt += Time.deltaTime;
        MapControllers();
    }

    public void MapControllers()
    {
        // randomize offset
        if (Keyboard.current.spaceKey.isPressed && lastBuilt >= actionDelay)
        {
            this.offset = new Vector2(Helpers.GetRandomInt(0, 100000), Helpers.GetRandomInt(0, 100000));
            GenerateMap();
        }

        // moving
        if (Keyboard.current.upArrowKey.isPressed && lastBuilt >= actionDelay)
        {
            this.offset.y += moveForce;
            GenerateMap();
        }
        else if (Keyboard.current.downArrowKey.isPressed && lastBuilt >= actionDelay)
        {
            this.offset.y -= moveForce;
            GenerateMap();
        }
        else if (Keyboard.current.rightArrowKey.isPressed && lastBuilt >= actionDelay)
        {
            this.offset.x += moveForce;
            GenerateMap();
        }
        else if (Keyboard.current.leftArrowKey.isPressed && lastBuilt >= actionDelay)
        {
            this.offset.x -= moveForce;
            GenerateMap();
        }
    }
}
