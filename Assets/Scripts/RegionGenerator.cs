using UnityEngine;

public static class RegionGenerator
{
    private const float MAX_REGION_HEIGHT = 0.2f;
    public static int[,] GenerateRegionMap(float[,] noiseMap, float[,] heightMap, float regionValue, Vector2 forrestHeightBounds)
    {
        var width = noiseMap.GetLength(0);
        var height = noiseMap.GetLength(1);

        var regionMap = new int[width, height];

        for(var y = 0; y < height; y++)
        {
            for(var x = 0; x < width; x++)
            {
                if(heightMap[x, y] < forrestHeightBounds.y && heightMap[x, y] > forrestHeightBounds.x && noiseMap[x, y] < regionValue)
                {
                    regionMap[x, y] = 1;
                }
                else
                {
                    regionMap[x, y] = 0;
                }
            }
        }

        return regionMap;
    }
}
