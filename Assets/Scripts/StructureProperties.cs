using UnityEngine;

[CreateAssetMenu(fileName = "New Structure Properties", menuName = "RTS/Structure Properties")]
public class StructureProperties : ScriptableObject
{
	public string name;
	public int hitpoints;
	public Sprite icon;
    public int housing;
    public float viewRadius;
}