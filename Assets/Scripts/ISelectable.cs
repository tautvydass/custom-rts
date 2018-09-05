using UnityEngine;

public interface ISelectable
{
	SelectionInfo GetSelectionInfo();
    Vector3 Position { get; }
    void Select();
    void Deselect();
}

public class SelectionInfo
{
	public string Name { get; set; }
	public int CurrentHitpoints { get; set; }
	public int MaxHitpoints { get; set; }
	public Sprite Icon { get; set; }
}
