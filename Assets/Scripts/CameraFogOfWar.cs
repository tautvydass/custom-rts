using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraFogOfWar : MonoBehaviour, IFogOfWarManager
{
    [SerializeField]
    private Material fogOfWarMaterial;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private ComputeShader fowCompute;

    [SerializeField]
    [Range(0, 1)]
    private float fowBlendStrength;

    [SerializeField]
    [Range(0, 1)]
    private float fowStrength;

    [SerializeField]
    private bool useFogOfWar;

    private List<ILighter> lightObjects;

    private Object[] objects;

    private int drawFOWKernel = -1;
    private int calculateFOWKernel = -1;

    private RenderTexture renderTexture;

    private const int OBJECTS_STRUCT_STRIDE = 12;

    private float[] emptyTextureInfoArray;

    private void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = GetComponent<Camera>();
        }
    }

    private void UpdateObjects()
    {
        objects = new Object[lightObjects.Count];
        for (var i = 0; i < lightObjects.Count; i++)
        {
            var position = lightObjects[i].Position;
            objects[i] = new Object(new Vector2(position.x, position.z), lightObjects[i].Radius * Screen.width);
        }
    }

    public void AddLightObject(ILighter lightObject)
    {
        if(lightObjects == null)
        {
            lightObjects = new List<ILighter>();
        }

        lightObjects.Add(lightObject);
        UpdateObjects();
    }

    public void AddLightObjects(IEnumerable<ILighter> lightObjects)
    {
        if(lightObjects == null)
        {
            return;
        }

        if (this.lightObjects == null)
        {
            this.lightObjects = lightObjects.ToList();
            return;
        }

        this.lightObjects.AddRange(lightObjects);
        UpdateObjects();
    }

    private void CalculateUnitViewportPositions()
    {
        for(var i = 0; i < lightObjects.Count; i++)
        {
            var position = mainCamera.WorldToScreenPoint(lightObjects[i].Position);
            objects[i].xPosition = position.x;
            objects[i].yPosition = position.y;
        }
    }

    private void ComputeFogOfWar(RenderTexture texture)
    {
        if (calculateFOWKernel == -1)
        {
            drawFOWKernel = fowCompute.FindKernel("DrawFOW");
            calculateFOWKernel = fowCompute.FindKernel("CalculateFOW");
        }

        if(emptyTextureInfoArray == null || emptyTextureInfoArray.Length != Screen.width * Screen.height)
        {
            emptyTextureInfoArray = new float[Screen.width * Screen.height];
        }

        if (renderTexture == null || renderTexture.width != Screen.width || renderTexture.height != Screen.height)
        {
            if(renderTexture != null)
            {
                renderTexture.Release();
            }

            renderTexture = new RenderTexture(Screen.width, Screen.height, 16) { enableRandomWrite = true };
            renderTexture.Create();
        }

        CalculateUnitViewportPositions();

        var textureInfoBuffer = new ComputeBuffer(Screen.width * Screen.height, sizeof(float));
        textureInfoBuffer.SetData(emptyTextureInfoArray);
        fowCompute.SetBuffer(calculateFOWKernel, "textureInfo", textureInfoBuffer);
        fowCompute.SetBuffer(drawFOWKernel, "textureInfo", textureInfoBuffer);
        
        var objectsBuffer = new ComputeBuffer(objects.Length, OBJECTS_STRUCT_STRIDE);
        objectsBuffer.SetData(objects);
        fowCompute.SetBuffer(calculateFOWKernel, "objects", objectsBuffer);

        fowCompute.SetInt("objectsCount", lightObjects.Count);
        fowCompute.SetInt("screenWidth", Screen.width);
        fowCompute.SetFloat("blendStrength", fowBlendStrength);
        fowCompute.SetFloat("fowStrength", fowStrength);

        fowCompute.SetTexture(drawFOWKernel, "output", renderTexture);
        fowCompute.SetTexture(drawFOWKernel, "input", texture);

        fowCompute.Dispatch(calculateFOWKernel, Screen.width / 8, Screen.height / 8, 1);
        fowCompute.Dispatch(drawFOWKernel, Screen.width / 8, Screen.height / 8, 1);

        objectsBuffer.Release();
        textureInfoBuffer.Release();
    }

    private Vector2 GetCenter()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)), out hit, 1000))
        {
            return new Vector2(hit.point.x, hit.point.z);
        }
        return Vector2.zero;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(!useFogOfWar)
        {
            Graphics.Blit(source, destination);
            return;
        }

        ComputeFogOfWar(source);
        Graphics.Blit(renderTexture, destination);
    }

    private struct Object
    {
        public float xPosition;
        public float yPosition;
        public float radius;

        public Object(Vector2 position, float radius)
        {
            xPosition = position.x;
            yPosition = position.y;
            this.radius = radius;
        }
    };
}
