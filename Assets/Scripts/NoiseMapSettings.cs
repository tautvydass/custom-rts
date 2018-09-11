using UnityEngine;

[CreateAssetMenu(fileName = "New Noise Map Settings", menuName = "RTS/Map/Noise Map Settings")]
public class NoiseMapSettings : ScriptableObject
{
    [Range(8, 1024)]
    public int mapWidth = 64;
    [Range(8, 1024)]
    public int mapHeight = 64;
    [Range(0.001f, 1024)]
    public float scale = 32;
    [Range(0, 32)]
    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.5f;
    [Range(1, 1024)]
    public float lacunarity = 2;
    public Vector2 mapOffset;
    public bool useSeed;
    public int seed = 1;
    [Range(0.001f, 1024)]
    public float heightMultiplier = 10;
    public Evaluator heightEvaluator;
}