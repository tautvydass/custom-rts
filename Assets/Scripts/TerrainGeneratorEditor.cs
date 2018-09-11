using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGenerator))]
[CanEditMultipleObjects]
public class TerrainGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate Terrain"))
        {
            (target as TerrainGenerator).GenerateMap();
        }
    }
}