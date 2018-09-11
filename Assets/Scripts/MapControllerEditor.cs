using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapController))]
[CanEditMultipleObjects]
public class MapControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Map"))
        {
            (target as MapController).GenerateMap();
        }
        if (GUILayout.Button("Bake Nav Mesh"))
        {
            (target as MapController).CreateNavMesh();
        }
    }
}
