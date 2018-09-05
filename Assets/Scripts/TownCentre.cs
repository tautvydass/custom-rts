using UnityEngine;
using System;

public class TownCentre : Structure, IUnitRecruiter, IResourceDepot
{
    [SerializeField]
    private Transform targetPosition;

	[SerializeField]
	private Transform spawnPoint;

    public Vector3 TargetPosition => targetPosition.position;

    public bool IsOfType(ResourceType resourceType) => true;

    public Vector3 GetClosestPoint(Vector3 point) => collider.ClosestPoint(point);

    public event Action<Unit> OnUnitRecruited;

    private new Collider collider;

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    protected override void Recruit(RecruitmentWorkOrder recruitmentWorkOrder)
	{
		var unit = Instantiate(recruitmentWorkOrder.unit, spawnPoint.position, Quaternion.identity).GetComponent<Unit>();
        unit.Move(targetPosition.position);
        OnUnitRecruited?.Invoke(unit);
	}

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition.position = targetPosition;
    }

    public override void Select()
    {
        selectionProjector.SetActive(true);
        targetPosition.gameObject.SetActive(true);
    }

    public override void Deselect()
    {
        targetPosition.gameObject.SetActive(false);
        selectionProjector.SetActive(false);
    }

    public DeliverInfo GetDeliveryInfo(Unit unit)
    {
        return new DeliverInfo()
        {
            ClosestPoint = GetClosestPoint(unit.Position),
            Distance = Vector3.Magnitude(unit.Position - Position)
        };
    }
}