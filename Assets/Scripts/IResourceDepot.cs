using UnityEngine;

public interface IResourceDepot
{
    bool IsOfType(ResourceType resourceType);
    Vector3 GetClosestPoint(Vector3 point);
    DeliverInfo GetDeliveryInfo(Unit unit);
}

public class DeliverInfo
{
    public Vector3 ClosestPoint { get; set; }
    public float Distance { get; set; }
}

