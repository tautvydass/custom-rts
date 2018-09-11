using UnityEngine;
using System;

public class MapMeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, Evaluator heightEvaluator)
    {
        var width = heightMap.GetLength(0);
        var height = heightMap.GetLength(1);

        var topLeftX = (width - 1) / -2f;
        var topLeftZ = (height - 1) / 2f;

        var meshData = new MeshData(width, height);

        var vertexIndex = 0;

        for(var y = 0; y < height; y++)
        {
            for(var x = 0; x < width; x++)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightMultiplier * heightEvaluator.Evaluate(heightMap[x, y]), topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if(x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}
