using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    private readonly Dictionary<int, Color> RegionColors = new Dictionary<int, Color>()
    {
        //[0] = new Color(91f / 255f, 91f/ 255f, 91f / 255f),
        [0] = new Color(255f / 255f, 242f / 255f, 150f / 255f),
        [1] = new Color(29f / 255f, 173f / 255f, 10f / 255f)
    };

    [SerializeField]
    private NoiseMapSettings heightMapSettings;
    [SerializeField]
    private NoiseMapSettings regionMapSettings;

    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private TextureData textureData;

    [SerializeField]
    [Range(0, 1)]
    private float forrestry;

    private NavMeshSurface navMeshSurface;

    [SerializeField]
    private bool generateOnAwake = false;

    [SerializeField]
    [Range(0, 1)]
    private float forrestDensity;
    [SerializeField]
    private Vector2 forrestHeightBounds;
    [SerializeField]
    private GameObject treePrefab;
    [SerializeField]
    private Transform treeParent;

    private MapData mapData;

    private List<GameObject> trees;

    private void Awake()
    {
        if(generateOnAwake)
        {
            GenerateMap();
        }
    }

    public void GenerateMap()
    {
        GenerateMapMesh();
        GenerateMapRegions();
        GenerateForrests();
        //ApplyAndApplyMapTexture();
        CreateNavMesh();

        textureData.ApplyToMaterial(meshRenderer.sharedMaterial);
    }

    public void CreateNavMesh()
    {
        if(navMeshSurface == null)
        {
            navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
        }

        navMeshSurface.BuildNavMesh();
    }

    private void GenerateMapMesh()
    {
        mapData = new MapData()
        {
            Width = heightMapSettings.mapWidth,
            Height = heightMapSettings.mapHeight,
            HeightMap = Noise.GenerateNoiseMap(heightMapSettings)
        };

        var meshData = MapMeshGenerator.GenerateTerrainMesh(mapData.HeightMap, heightMapSettings.heightMultiplier, heightMapSettings.heightEvaluator);

        meshFilter.sharedMesh = meshData.CreateMesh();

        textureData.UpdateMeshHeights(meshRenderer.sharedMaterial, 0, heightMapSettings.heightMultiplier);
    }

    private void GenerateMapRegions()
    {
        var noiseMap = Noise.GenerateNoiseMap(regionMapSettings);
        mapData.RegionMap = RegionGenerator.GenerateRegionMap(noiseMap, mapData.HeightMap, forrestry, forrestHeightBounds);
    }

    private void GenerateForrests()
    {
        if(mapData.RegionMap == null)
        {
            return;
        }

        if(trees != null)
        {
            foreach(var tree in trees)
            {
                DestroyImmediate(tree);
            }
            trees.Clear();
        }

        trees = new List<GameObject>();

        var halfWidth = mapData.Width / 2f;
        var halfHeight = mapData.Height / 2f;

        for (var y = 0; y < mapData.Height; y++)
        {
            for(var x = 0; x < mapData.Width; x++)
            {
                if(mapData.RegionMap[x, y] == 1)
                {
                    if(Random.Range(0f, 1f) < forrestDensity)
                    {
                        var tree = Instantiate(treePrefab, new Vector3((x - halfWidth + 1) * transform.localScale.x, mapData.HeightMap[x, y] * heightMapSettings.heightMultiplier, (halfHeight - y - 1) * transform.localScale.z), Quaternion.identity, treeParent);
                        tree.transform.localEulerAngles += new Vector3(0, Random.Range(0, 360), 0);
                        trees.Add(tree);
                    }
                }
            }
        }
    }

    private void ApplyAndApplyMapTexture()
    {
        var colorMap = new Color[mapData.Height * mapData.Width];
        var index = 0;
        for(var y = 0; y < mapData.Height; y++)
        {
            for(var x = 0; x < mapData.Width; x++)
            {
                colorMap[index] = RegionColors[mapData.RegionMap[x, y]];
                index++;
            }
        }

        var texture = TextureGenerator.GenerateTexture(colorMap, mapData.Width, mapData.Height);

        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
