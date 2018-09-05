using UnityEngine;

public class HarvestableInfo
{
    public Sprite Icon { get; private set; }
    public string Name { get; private set; }
    public int Resources { get; private set; }
    public ResourceType ResourceType { get; private set; }

    public HarvestableInfo(Sprite icon, string name, int resources, ResourceType resourceType)
    {
        Icon = icon;
        Name = name;
        Resources = resources;
        ResourceType = resourceType;
    }

    public void UpdateResources(int resources) => Resources = resources;
}
