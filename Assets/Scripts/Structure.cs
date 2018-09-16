using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum StructureType
{
	TownCenter = 0,
	House = 1
}

public abstract class Structure : MonoBehaviour, ISelectable, ILighter
{
	[SerializeField]
	protected StructureProperties properties;

	[SerializeField]
	protected List<WorkOrder> workOrders;

    [SerializeField]
    protected GameObject selectionProjector;

    protected IPopulationManager populationManager;
    protected IResourcesManager resourcesManager;

    protected bool recruitmentProcessStuck = false;

	public int CurrentHitpoints { get; protected set; }
	public bool Working { get; protected set; } = false;

    public Vector3 Position => transform.position;

    public float Radius => properties.viewRadius;

    public bool pendingCancel = false;

    public float WorkOrderTimeElapsed { get; protected set; }
    public float WorkOrderDuration { get; protected set; }

	public IReadOnlyList<WorkOrder> WorkOrders => workOrders;
    public IReadOnlyList<WorkOrder> WorkOrderQueue => workOrderQueue.GetReadonlyList();

    private CustomQueue<WorkOrder> workOrderQueue;

    public StructureProperties Properties => properties;

    public event Action<IReadOnlyList<WorkOrder>> OnWorkOrderQueueUpdated;
    public event Action OnWorkOrderStarted;

    public void Initialize(IPopulationManager populationManager, IResourcesManager resourcesManager)
    {
        this.populationManager = populationManager;
        this.resourcesManager = resourcesManager;

        CurrentHitpoints = properties.hitpoints;

        workOrderQueue = new CustomQueue<WorkOrder>();

        populationManager.OnPopulationUpdated += OnPopulationUpdated;
    }

    public SelectionInfo GetSelectionInfo()
	{
		return new SelectionInfo()
		{
			Name = properties.name,
			CurrentHitpoints = this.CurrentHitpoints,
			MaxHitpoints = properties.hitpoints,
			Icon = properties.icon
		};
	}

	public void BeginWorkOrder(WorkOrder workOrder)
	{
        if(!resourcesManager.TryPay(workOrder.cost))
        {
            ErrorManager.Instance.ShowError("Insufficient resources!");
            return;
        }

		workOrderQueue.Enqueue(workOrder);
        OnWorkOrderQueueUpdated?.Invoke(workOrderQueue.GetReadonlyList());

        if (!Working)
		{
			StartCoroutine(Work());
		}
	}

    public void CancelWorkOrder(int index)
    {
        var workOrder = workOrderQueue.Remove(index);
        resourcesManager.Refund(workOrder.cost);
        OnWorkOrderQueueUpdated?.Invoke(workOrderQueue.GetReadonlyList());

        if (index == 0)
        {
            pendingCancel = true;
        }
    }

	private IEnumerator Work()
	{
		Working = true;

		while(workOrderQueue.Count > 0)
		{
			var workOrder = workOrderQueue.Peek();

            if (workOrder is RecruitmentWorkOrder)
            {
                var recruitmentCompleted = false;
                while(!recruitmentCompleted)
                {
                    if (populationManager.TryRecruit(workOrder as RecruitmentWorkOrder))
                    {
                        OnWorkOrderStarted?.Invoke();
                        yield return WaitFor(workOrder.duration);

                        if(!pendingCancel)
                        {
                            Recruit(workOrder as RecruitmentWorkOrder);
                        }
                        else
                        {
                            populationManager.Cancel(workOrder as RecruitmentWorkOrder);
                        }

                        recruitmentCompleted = true;
                    }
                    else
                    {
                        recruitmentProcessStuck = true;
                        while(recruitmentProcessStuck)
                        {
                            yield return null;
                        }
                    }
                }
            }
            else
            {
                OnWorkOrderStarted?.Invoke();
                yield return WaitFor(workOrder.duration);
            }

            if(!pendingCancel)
            {
                workOrderQueue.Dequeue();
                OnWorkOrderQueueUpdated?.Invoke(workOrderQueue.GetReadonlyList());
            }
            else
            {
                pendingCancel = false;
            }
        }

		Working = false;
	}

    private IEnumerator WaitFor(float duration)
    {
        WorkOrderTimeElapsed = 0;
        WorkOrderDuration = duration;

        while(WorkOrderTimeElapsed < WorkOrderDuration)
        {
            WorkOrderTimeElapsed += Time.deltaTime;

            if(pendingCancel)
            {
                break;
            }

            yield return null;
        }

        WorkOrderTimeElapsed = 0;
    }

	protected abstract void Recruit(RecruitmentWorkOrder recruitmentWorkOrder);

    public abstract void Select();
    public abstract void Deselect();

    private void OnPopulationUpdated(Population population)
    {
        if(recruitmentProcessStuck)
        {
            recruitmentProcessStuck = false;
        }
    }
}
