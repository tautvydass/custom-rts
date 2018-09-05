public interface IResourceDepotManager
{
    IResourceDepot GetClosestDepot(Unit unit, ResourceType resourceType);
    void Deliver(ResourceType resourceType, int amount);
}