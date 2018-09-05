using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private Camera mainCamera;

	public event Action<List<ISelectable>> OnSelected;
	public event Action OnDeselected;
	public event Action<List<Structure>> OnStructuresSelected;
	public event Action OnStructuresDeselected;

    public Func<Bounds, bool, List<ISelectable>> GetSelectablesInBounds;

    private Selection selection;

    private Vector2 startingMousePosition;
    private bool selecting = false;

    public void SetGetSelectablesCall(Func<Bounds, bool, List<ISelectable>> getSelectablesCall)
    {
        GetSelectablesInBounds = getSelectablesCall;
    }

    private void Awake()
	{
		if(mainCamera == null)
		{
            mainCamera = Camera.main;
		}

        selection = new Selection();
	}

    private void Select(List<ISelectable> selectables)
    {
        Deselect();

        if (selectables.Count == 0)
        {
            return;
        }

        if (selectables[0] is Unit)
        {
            selection.Set(SelectionType.Unit, selectables);
        }
        else if (selectables[0] is Structure)
        {
            selection.Set(SelectionType.Structure, selectables);
            OnStructuresSelected?.Invoke(selectables.Select(selectable => selectable as Structure).ToList());
        }
        else if(selectables[0] is IHarvestable)
        {
            selection.Set(SelectionType.Harvestable, selectables);
        }

        foreach(var selectable in selectables)
        {
            selectable.Select();
        }
        OnSelected?.Invoke(selectables);
    }

    private void Deselect()
    {
        if(selection.Selected)
        {
            foreach (var selectable in selection.Selectables)
            {
                selectable.Deselect();
            }

            if (selection.SelectionType == SelectionType.Structure)
            {
                OnStructuresDeselected?.Invoke();
            }

            OnDeselected?.Invoke();
            selection.Clear();
        }
    }

	private void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                startingMousePosition = Input.mousePosition;
                selecting = true;
            }
		}

		if(Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 1000))
			{
				if(EventSystem.current.IsPointerOverGameObject())
				{
					return;
				}

                if(selection.Selected)
                {
                    if(hit.collider.tag == Settings.Tags.Ground)
                    {
                        if(selection.SelectionType == SelectionType.Unit)
                        {
                            // todo: cache these values
                            var stoppingDistance = Mathf.Sqrt(selection.Selectables.Count);

                            foreach(var selectable in selection.Selectables)
                            {
                                (selectable as Unit).Move(hit.point, stoppingDistance);
                            }
                        }
                        else if(selection.SelectionType == SelectionType.Structure)
                        {
                            foreach(var unitRecruiter in selection.Selectables.Where(selectable => selectable is IUnitRecruiter).Select(selectable => selectable as IUnitRecruiter))
                            {
                                unitRecruiter.SetTargetPosition(hit.point);
                            }
                        }
                    }
                    else if(hit.collider.tag == Settings.Tags.Harvestable)
                    {
                        var harvestable = hit.collider.GetComponent<IHarvestable>();
                        foreach(var harvester in selection.Selectables.Where(s => s is IHarvester))
                        {
                            (harvester as IHarvester).BeginHarvest(harvestable);
                        }
                    }
                }
			}
		}

        if (Input.GetMouseButtonUp(0))
        {
            selecting = false;

            if(selection.Selectables != null && selection.Selectables.Count != 0)
            {
                return;
            }

            RaycastHit raycastHit;
            if(Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out raycastHit, 200))
            {
                var selectable = raycastHit.collider.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    Select(new List<ISelectable>() { selectable });
                }
                else
                {
                    Deselect();
                }
            }
        }

        if(selecting)
        {
            var selectables = GetSelectablesInBounds(Utils.GetViewportBounds(mainCamera, startingMousePosition, Input.mousePosition), true);
            Select(selectables);
        }
    }

    private void OnGUI()
    {
        if (selecting)
        {
            var rect = Utils.GetScreenRect(startingMousePosition, Input.mousePosition);
            Utils.DrawScreenRectBorder(rect, 1, Color.green);
        }
    }
}
