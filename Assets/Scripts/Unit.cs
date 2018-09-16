using UnityEngine;
using UnityEngine.AI;

public abstract class Unit : MonoBehaviour, ISelectable, ILighter
{
	[SerializeField]
	protected UnitProperties properties;

    [SerializeField]
    protected GameObject selectionProjector;

	public UnitProperties GetProperties() => properties;

	public int CurrentHealth { get; private set; }

    public Vector3 Position => transform.position;

    public float Radius => properties.viewRadius;

    protected SelectionInfo selectionInfo;

    protected NavMeshAgent agent;

	protected void Awake()
	{
		CurrentHealth = properties.maxHealth;
				
		agent = gameObject.AddComponent<NavMeshAgent>();

		agent.speed = properties.moveSpeed;
		agent.angularSpeed = properties.rotationSpeed;
		agent.acceleration = properties.acceleration;
        agent.stoppingDistance = properties.stoppingDistance;
        agent.radius = properties.radius;
	}

    public abstract void Move(Vector3 position);
    public abstract void Move(Vector3 position, float stoppingDistance);

	public SelectionInfo GetSelectionInfo()
	{
		return new SelectionInfo()
		{
			Name = properties.name,
			CurrentHitpoints = CurrentHealth,
			MaxHitpoints = properties.maxHealth,
			Icon = properties.icon
		};
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
