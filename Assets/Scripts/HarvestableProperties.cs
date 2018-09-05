using UnityEngine;

[CreateAssetMenu(fileName = "New Harvestable Properties", menuName = "RTS/Harvestable Properties")]
public class HarvestableProperties : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public Vector2Int capacityBounds;
}