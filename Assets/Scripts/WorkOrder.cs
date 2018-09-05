using UnityEngine;

public abstract class WorkOrder : ScriptableObject
{
	public string name;
	public int duration;
	public bool repeatable;
	public Resources cost;
	public Sprite icon;
}