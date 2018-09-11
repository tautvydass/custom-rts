using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class NavMeshBaker
{
    [SerializeField]
    private NavMeshSettings navMeshSettings;

    public void Bake()
    {
        NavMeshBuilder.BuildNavMeshData(
            navMeshSettings.ToBuildSettings(),
            new List<NavMeshBuildSource>(),
            new Bounds(),
            Vector3.zero,
            Quaternion.identity);
    }
}
