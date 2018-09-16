using System.Collections;
using UnityEngine;

public class Worker : Unit, IHarvester
{
    public bool Harvesting { get; private set; }
    public int Stash { get; private set; }
    public ResourceType ResourceType { get; private set; }

    private IHarvestable currentHarvestable;

    private IResourceDepotManager resourceDepotManager;

    private IResourceDepot resourceDepot;

    public void SetResourceDepotManager(IResourceDepotManager resourceDepotManager) => this.resourceDepotManager = resourceDepotManager;

    private bool harvestableUpdated = false;

    private new void Awake()
    {
        base.Awake();

        Harvesting = false;
        Stash = 0;
        ResourceType = ResourceType.None;
    }

    public override void Move(Vector3 position) => Move(position, properties.stoppingDistance);

    public override void Move(Vector3 position, float stoppingDistance)
	{
        if (Harvesting)
        {
            StopCoroutine(Harvest());
            Harvesting = false;
        }

        agent.isStopped = false;
        agent.stoppingDistance = stoppingDistance;
		agent.SetDestination(position);
	}

    public void BeginHarvest(IHarvestable harvestable)
    {
        currentHarvestable = harvestable;

        if (Harvesting)
        {
            harvestableUpdated = true;
            return;
        }

        StartCoroutine(Harvest());
    }

    private IEnumerator Harvest()
    {
        Harvesting = true;

        while(Harvesting)
        {
            agent.isStopped = false;
            agent.SetDestination(currentHarvestable.Position);
            while(Vector3.Magnitude(Position - currentHarvestable.Position) > agent.stoppingDistance * 2)
            {
                if(!Harvesting)
                {
                    break;
                }
                else if(harvestableUpdated)
                {
                    agent.SetDestination(currentHarvestable.Position);
                    harvestableUpdated = false;
                }
                yield return null;
            }
            if(!Harvesting)
            {
                break;
            }
            agent.isStopped = true;

            if (Stash != 0)
            {
                if(ResourceType != currentHarvestable.ResourceType)
                {
                    Stash = 0;
                }
            }

            ResourceType = currentHarvestable.ResourceType;

            var timeElapsed = 0f;

            while (Stash < Settings.MaxWorkerStash)
            {
                timeElapsed = 0f;
                while(timeElapsed < Settings.WorkIterationLength)
                {
                    timeElapsed += Time.deltaTime;
                    if (!Harvesting || harvestableUpdated)
                    {
                        break;
                    }
                    yield return null;
                }
                if (!Harvesting || harvestableUpdated)
                {
                    break;
                }
                else
                {
                    Stash += currentHarvestable.Harvest();
                }
            }
            if (!Harvesting)
            {
                break;
            }
            else if(harvestableUpdated)
            {
                harvestableUpdated = false;
            }
            else
            {
                if (resourceDepot == null)
                {
                    resourceDepot = resourceDepotManager.GetClosestDepot(this, ResourceType);
                }

                var closestPoint = resourceDepot.GetClosestPoint(Position);
                agent.isStopped = false;
                agent.SetDestination(closestPoint);
                while (Vector3.Magnitude(Position - closestPoint) > agent.stoppingDistance * 2)
                {
                    yield return null;
                }
                agent.isStopped = true;

                resourceDepotManager.Deliver(ResourceType, Stash);
                Stash = 0;
            }
        }
    }
}