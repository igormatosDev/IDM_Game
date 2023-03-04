using UnityEngine;

public class PerlinNoiseGenerator
{
    public static float[,] GenerateNoiseMap(int width, int height, float scale, int octaves, float persistence, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[width, height];

        float maxNoiseValue = float.MinValue;
        float minNoiseValue = float.MaxValue;

        // Loop through each tile in the map and generate a noise value for it.
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Initialize variables to store the noise value and the amplitute and frequency of each octave.
                float amplitude = 1f;
                float frequency = 1f;
                float noiseValue = 0f;

                // Loop through each octave and add the noise value to the total noise map.
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x + offset.x) / scale * frequency;
                    float sampleY = (y + offset.y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2f - 1f;
                    noiseValue += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                // Update the maximum and minimum noise values.
                maxNoiseValue = Mathf.Max(maxNoiseValue, noiseValue);
                minNoiseValue = Mathf.Min(minNoiseValue, noiseValue);

                // Set the noise value for the current tile in the noise map.
                noiseMap[x, y] = noiseValue;
            }
        }

        // Loop through each tile in the noise map and normalize the noise values.
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseValue, maxNoiseValue, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
