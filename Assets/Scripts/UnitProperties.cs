using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Properties", menuName = "RTS/Unit Properties")]
public class UnitProperties : ScriptableObject
{
	public string name;
	public int maxHealth;
	public float moveSpeed;
	public float rotationSpeed;
	public float acceleration;
	public Sprite icon;
	public int population;
    public float stoppingDistance;
    public float radius;
}
