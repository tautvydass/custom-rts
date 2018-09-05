using UnityEngine;

public interface IHarvestable
{
    ResourceType ResourceType { get; }
    int Resources { get; }
    Vector3 Position { get; }

    int Harvest();
}
