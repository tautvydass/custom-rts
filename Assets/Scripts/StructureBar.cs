using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class StructureBar
{
	[SerializeField]
	private GameObject bar;
	[SerializeField]
	private Sprite defaultButtonSprite;

	[SerializeField]
	private List<Button> workOrderButtons;

    [SerializeField]
    private GameObject workOrderQueuePrefab;

    [SerializeField]
    private Transform workOrderQueueParent;

    private List<WorkOrderQueueComponent> workOrderQueueImages = new List<WorkOrderQueueComponent>();

    private Func<GameObject, Transform, WorkOrderQueueComponent> InstantiateWorkOrderQueueImage;

    public Structure CurrentStructure { get; private set; }

    public void SetInstantiateCall(Func<GameObject, Transform, WorkOrderQueueComponent> instantiateCall)
    {
        InstantiateWorkOrderQueueImage = instantiateCall;
    }

    public void DisplayStructureWorkOrders(Structure structure)
	{
        CurrentStructure = structure;

        var index = 0;
        foreach(var workOrder in structure.WorkOrders)
        {
            workOrderButtons[index].GetComponent<Image>().sprite = workOrder.icon;
            workOrderButtons[index].onClick.AddListener(() => structure.BeginWorkOrder(workOrder));
            workOrderButtons[index].interactable = true;

            index++;
        }
		for(var i = structure.WorkOrders.Count; i < workOrderButtons.Count; i++)
		{
			workOrderButtons[i].GetComponent<Image>().sprite = defaultButtonSprite;
			workOrderButtons[i].interactable = false;
		}

        UpdateWorkOrderQueue(structure.WorkOrderQueue);

		bar.SetActive(true);
	}

	public void HideStructureWorkOrders()
	{
		foreach(var button in workOrderButtons)
		{
			button.onClick.RemoveAllListeners();
		}

		bar.SetActive(false);

        CurrentStructure = null;
	}

    public void UpdateWorkOrderQueue(IReadOnlyList<WorkOrder> workOrderQueue)
    {
        foreach(var workOrderQueueImage in workOrderQueueImages)
        {
            workOrderQueueImage.Button.onClick.RemoveAllListeners();
        }

        for (var i = 0; i < Mathf.Min(workOrderQueueImages.Count, workOrderQueue.Count); i++)
        {
            workOrderQueueImages[i].Image.sprite = workOrderQueue[i].icon;
            workOrderQueueImages[i].GameObject.SetActive(true);

            var index = i;
            workOrderQueueImages[i].Button.onClick.AddListener(() => CurrentStructure.CancelWorkOrder(index));
        }
        for(var i = workOrderQueueImages.Count; i < workOrderQueue.Count; i++)
        {
            var newObjectComponent = InstantiateWorkOrderQueueImage(workOrderQueuePrefab, workOrderQueueParent);
            newObjectComponent.Image.sprite = workOrderQueue[i].icon;

            workOrderQueueImages.Add(newObjectComponent);

            var index = i;
            workOrderQueueImages[i].Button.onClick.AddListener(() => CurrentStructure.CancelWorkOrder(index));
        }
        for(var i = workOrderQueue.Count; i < workOrderQueueImages.Count; i++)
        {
            workOrderQueueImages[i].GameObject.SetActive(false);
        }
    }

    public void SetFirstImageFillAmount(float fillAmount)
    {
        workOrderQueueImages[0].Image.fillAmount = fillAmount;
    }
}
