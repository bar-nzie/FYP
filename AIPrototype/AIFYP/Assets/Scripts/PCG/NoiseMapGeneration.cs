using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour
{
    public float[,] GenerateNoiseMap(int width, int height, float scale)
    {
        float[,] noiseMap = new float[width, height];

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = x / scale;
                float sampleZ = y / scale;

                float noise = Mathf.PerlinNoise(sampleX, sampleZ);
                noiseMap[y, x] = noise;
            }
        }

        return noiseMap;
    }
}
