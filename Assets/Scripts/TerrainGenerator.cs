using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private NoiseMapSettings noiseMapSettings;

    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private TextureData textureData;

    public void GenerateMap()
    {
        var meshData = MapMeshGenerator.GenerateTerrainMesh(Noise.GenerateNoiseMap(noiseMapSettings), noiseMapSettings.heightMultiplier, noiseMapSettings.heightEvaluator);

        meshFilter.sharedMesh = meshData.CreateMesh();

        textureData.ApplyToMaterial(meshRenderer.sharedMaterial);
    }

    private void Awake()
    {
        textureData.ApplyToMaterial(meshRenderer.sharedMaterial);
    }
}
