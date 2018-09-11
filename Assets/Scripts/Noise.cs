using UnityEngine;

using Random = System.Random;

public static class Noise
{
    private const int MIN_OFFSET_VALUE = -100000;
    private const int MAX_OFFSET_VALUE = 100000;

    public static float[,] GenerateNoiseMap(NoiseMapSettings noiseMapSettings)
    {
        var noiseMap = new float[noiseMapSettings.mapWidth, noiseMapSettings.mapHeight];

        var prng = noiseMapSettings.useSeed ? new Random(noiseMapSettings.seed) : new Random();

        var octaveOffsets = new Vector2[noiseMapSettings.octaves];
        for(var i = 0; i < noiseMapSettings.octaves; i++)
        {
            var offsetX = prng.Next(MIN_OFFSET_VALUE, MAX_OFFSET_VALUE) + noiseMapSettings.mapOffset.x;
            var offsetY = prng.Next(MIN_OFFSET_VALUE, MAX_OFFSET_VALUE) + noiseMapSettings.mapOffset.y;

            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        var maxNoiseHeight = float.MinValue;
        var minNoiseHeight = float.MaxValue;

        var halfMapWidth = noiseMapSettings.mapWidth / 2f;
        var halfMapHeight = noiseMapSettings.mapHeight / 2f;

        for (var y = 0; y < noiseMapSettings.mapHeight; y++)
        {
            for(var x = 0; x < noiseMapSettings.mapWidth; x++)
            {
                var amplitude = 1f;
                var frequency = 1f;
                var noiseHeight = 0f;

                for(var i = 0; i < noiseMapSettings.octaves; i++)
                {
                    var currentX = (x - halfMapWidth) / noiseMapSettings.scale * frequency + octaveOffsets[i].x;
                    var currentY = (y - halfMapHeight) / noiseMapSettings.scale * frequency + octaveOffsets[i].y;

                    var perlinValue = Mathf.PerlinNoise(currentX, currentY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= noiseMapSettings.persistance;
                    frequency *= noiseMapSettings.lacunarity;
                }

                if(noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }

                if(noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (var y = 0; y < noiseMapSettings.mapHeight; y++)
        {
            for (var x = 0; x < noiseMapSettings.mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}