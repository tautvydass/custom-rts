using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "New Nav Mesh Build Settings", menuName = "RTS/Map/Nav Mesh Build Settings")]
public class NavMeshSettings : ScriptableObject
{
    public float agentClimb;

    public float agentHeight;

    public float agentRadius;

    public float agentSlope;

    public int agentTypeID;


    public NavMeshBuildSettings ToBuildSettings()
    {
        var navMeshBuildSettings = new NavMeshBuildSettings
        {
            agentClimb = agentClimb,
            agentHeight = agentHeight,
            agentRadius = agentRadius,
            agentSlope = agentSlope,
            agentTypeID = agentTypeID
        };

        return navMeshBuildSettings;
    }
}
