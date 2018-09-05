using System.Collections.Generic;

public class Selection
{
    public SelectionType SelectionType { get; set; }
    public List<ISelectable> Selectables { get; set; }
    public bool Selected { get; set; }

    public void Set(SelectionType selectionType, List<ISelectable> selectables)
    {
        SelectionType = selectionType;
        Selectables = selectables;
        Selected = true;
    }

    public void Clear()
    {
        Selectables = null;
        Selected = false;
    }
}
