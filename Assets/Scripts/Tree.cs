using UnityEngine;

public class Tree : MonoBehaviour, IHarvestable, ISelectable
{
    [SerializeField]
    private HarvestableProperties harvestableProperties;

    [SerializeField]
    private GameObject selectionProjector;

    public ResourceType ResourceType { get; } = ResourceType.Wood;
    public int Resources { get; private set; }
    public Vector3 Position => transform.position;
    public SelectionInfo GetSelectionInfo() => selectionInfo;

    private int startingResources;
    private SelectionInfo selectionInfo;

    private void Awake()
    {
        Resources = (startingResources = Random.Range(harvestableProperties.capacityBounds.x, harvestableProperties.capacityBounds.y));

        selectionInfo = new SelectionInfo()
        {
            Icon = harvestableProperties.icon,
            CurrentHitpoints = Resources,
            MaxHitpoints = startingResources,
            Name = harvestableProperties.name
        };
    }

    public int Harvest()
    {
        Resources--;
        if(Resources == 0)
        {
            // todo: destroy tree
        }

        return 1;
    }

    public void Select()
    {
        selectionProjector.SetActive(true);
    }

    public void Deselect()
    {
        selectionProjector.SetActive(false);
    }
}
