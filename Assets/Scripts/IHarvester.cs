public interface IHarvester
{
    bool Harvesting { get; }
    void BeginHarvest(IHarvestable harvestable);
    int Stash { get; }
    ResourceType ResourceType { get; }
}