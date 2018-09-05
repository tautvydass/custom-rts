using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour, IPopulationManager, IResourcesManager, IResourceDepotManager
{
	[SerializeField]
	private UserInterfaceController userInterfaceController;
	[SerializeField]
	private PlayerController playerController;

	private Population population;
	private Resources resources;

	public event Action<Resources> OnResourcesUpdated;
	public event Action<Population> OnPopulationUpdated;

	private List<Unit> units;
	private List<Structure> structures;
    private List<ISelectable> harvestables;

    private Camera mainCamera;

    [SerializeField]
    private List<Structure> unregisteredStructures;

	private void Awake()
	{
        mainCamera = Camera.main;

		population = new Population(0, Settings.DefaultPopulation);

		resources = new Resources(
			Settings.DefaultResources.Wood,
			Settings.DefaultResources.Food,
			Settings.DefaultResources.Stone,
			Settings.DefaultResources.Metal
		);

		OnResourcesUpdated += userInterfaceController.GetOnResourcesUpdatedAction();
		OnPopulationUpdated += userInterfaceController.GetOnPopulationUpdatedAction();

		playerController.OnSelected += userInterfaceController.GetOnSelectedAction();
		playerController.OnDeselected += userInterfaceController.GetOnDeselectedAction();

		playerController.OnStructuresSelected += userInterfaceController.OnStructuresSelected;
		playerController.OnStructuresDeselected += userInterfaceController.OnStructuresDeselected;

        units = new List<Unit>();

        structures = new List<Structure>();
        foreach(var structure in unregisteredStructures)
        {
            structures.Add(structure);
            structure.Initialize(this, this);

            if(structure is IUnitRecruiter)
            {
                (structure as IUnitRecruiter).OnUnitRecruited += OnUnitRecruited;
            }

            population.allowed += structure.Properties.housing;
        }

        harvestables = new List<ISelectable>();
        foreach(var harvestable in GameObject.FindGameObjectsWithTag(Settings.Tags.Harvestable))
        {
            harvestables.Add(harvestable.GetComponent<ISelectable>());
        }

        playerController.SetGetSelectablesCall(GetSelectablesInBounds);

        OnResourcesUpdated?.Invoke(resources);
        OnPopulationUpdated?.Invoke(population);
    }

    public int GetAvailablePopulation()
    {
        return population.allowed - population.current;
    }

    public bool TryRecruit(RecruitmentWorkOrder recruitmentWorkOrder)
    {
        if(population.CanReqruit(recruitmentWorkOrder.Population))
        {
            population.current += recruitmentWorkOrder.Population;
            OnPopulationUpdated?.Invoke(population);
            return true;
        }
        return false;
    }

    public void Cancel(RecruitmentWorkOrder recruitmentWorkOrder)
    {
        population.current -= recruitmentWorkOrder.Population;
        OnPopulationUpdated?.Invoke(population);
    }

    public bool TryPay(Resources price)
    {
        if(resources >= price)
        {
            resources.Subtract(price);
            OnResourcesUpdated?.Invoke(resources);
            return true;
        }

        return false;
    }

    public void Refund(Resources resources)
    {
        this.resources.Add(resources);
        OnResourcesUpdated?.Invoke(this.resources);
    }

    private List<ISelectable> GetSelectablesInBounds(Bounds bounds, bool prioritiseTypes = true)
    {
        var list = new List<ISelectable>();
        foreach(var unit in units)
        {
            if(bounds.Contains(mainCamera.WorldToViewportPoint(unit.Position)))
            {
                list.Add(unit);
            }
        }
        if(prioritiseTypes && list.Count > 0)
        {
            return list;
        }
        foreach (var structure in structures)
        {
            if (bounds.Contains(mainCamera.WorldToViewportPoint(structure.Position)))
            {
                list.Add(structure);
            }
        }
        if (prioritiseTypes && list.Count > 0)
        {
            return list;
        }
        foreach(var harvestable in harvestables)
        {
            if (bounds.Contains(mainCamera.WorldToViewportPoint(harvestable.Position)))
            {
                list.Add(harvestable);
                break;
            }
        }
        return list;
    }

    private void OnUnitRecruited(Unit unit)
    {
        units.Add(unit);
        if(unit is Worker)
        {
            (unit as Worker).SetResourceDepotManager(this);
        }
    }

    public IResourceDepot GetClosestDepot(Unit unit, ResourceType resourceType)
    {
        var resourceDepots = structures
            .Where(structure => structure is IResourceDepot && (structure as IResourceDepot).IsOfType(resourceType))
            .Select(structure => new { Depot = (structure as IResourceDepot), DeliveryInfo = (structure as IResourceDepot).GetDeliveryInfo(unit) }).ToList();

        if(resourceDepots.Count == 1)
        {
            return resourceDepots[0].Depot;
        }

        var closestDistance = resourceDepots.Select(depot => depot.DeliveryInfo.Distance).Min();
        return resourceDepots.First(depot => depot.DeliveryInfo.Distance == closestDistance).Depot;
    }

    public void Deliver(ResourceType resourceType, int amount)
    {
        resources.AddAmount(resourceType, amount);
        OnResourcesUpdated?.Invoke(resources);
    }
}

public class Population
{
	public int current;
	public int allowed;

    public bool CanReqruit(int population) => population <= (allowed - current);

	public Population(int current, int allowed)
	{
		this.current = current;
		this.allowed = allowed;
	}
}
