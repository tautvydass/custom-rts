using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UserInterfaceController : MonoBehaviour
{
	[SerializeField]
	private InfoBar infoBar;

	[SerializeField]
	private TopBar topBar;

	[SerializeField]
	private StructureBar structureBar;

    [SerializeField]
    private ErrorManager errorManager;

    public Action<Resources> GetOnResourcesUpdatedAction() => topBar.GetOnResourcesUpdatedAction();
    public Action<Population> GetOnPopulationUpdatedAction() => topBar.GetOnPopulationUpdatedAction();

    public Action<List<ISelectable>> GetOnSelectedAction() => infoBar.DisplayInfo;
    public Action GetOnDeselectedAction() => infoBar.HideUnitInfo;

    private bool updatingWorkOrderFillAmount = false;

    private bool showingError = false;

    private float errorTimeElapsed = 0f;
    private const float errorDuration = 1.5f;

    public void OnStructuresSelected(List<Structure> structures)
    {
        if(structureBar.CurrentStructure != null)
        {
            structureBar.CurrentStructure.OnWorkOrderQueueUpdated -= structureBar.UpdateWorkOrderQueue;
            structureBar.CurrentStructure.OnWorkOrderStarted -= OnWorkOrderStarted;
        }
        structureBar.DisplayStructureWorkOrders(structures[0]);

        structures[0].OnWorkOrderQueueUpdated += structureBar.UpdateWorkOrderQueue;
        structures[0].OnWorkOrderStarted += OnWorkOrderStarted;

        if(structures[0].Working && !updatingWorkOrderFillAmount)
        {
            StartCoroutine(UpdateWorkOrderQueueFillAmount());
        }
    }

    public void OnStructuresDeselected()
    {
        if(updatingWorkOrderFillAmount)
        {
            StopCoroutine(UpdateWorkOrderQueueFillAmount());
            updatingWorkOrderFillAmount = false;
        }

        structureBar.CurrentStructure.OnWorkOrderQueueUpdated -= structureBar.UpdateWorkOrderQueue;
        structureBar.CurrentStructure.OnWorkOrderStarted -= OnWorkOrderStarted;

        structureBar.HideStructureWorkOrders();
    }

    private WorkOrderQueueComponent InstantiateImage(GameObject prefab, Transform parent)
    {
        var newGameObject = Instantiate(prefab, parent);
        return new WorkOrderQueueComponent()
        {
            GameObject = newGameObject,
            Button = newGameObject.GetComponent<Button>(),
            Image = newGameObject.GetComponent<Image>()
        };
    }

    private void Awake()
    {
        structureBar.SetInstantiateCall(InstantiateImage);
        errorManager.Initialize();
        errorManager.OnErrorThrown += ShowError;
    }

    public void OnWorkOrderStarted()
    {
        if(!updatingWorkOrderFillAmount && structureBar.CurrentStructure.Working)
        {
            StartCoroutine(UpdateWorkOrderQueueFillAmount());
        }
    }

    private IEnumerator UpdateWorkOrderQueueFillAmount()
    {
        updatingWorkOrderFillAmount = true;
        var structure = structureBar.CurrentStructure;
        while(structure.Working)
        {
            structureBar.SetFirstImageFillAmount((structure.WorkOrderDuration - structure.WorkOrderTimeElapsed) / structure.WorkOrderDuration);
            yield return null;
        }
        updatingWorkOrderFillAmount = false;
    }

    private void ShowError()
    {
        if(showingError)
        {
            errorTimeElapsed = 0f;
        }
        else
        {
            StartCoroutine(DisplayError());
        }
    }

    private IEnumerator DisplayError()
    {
        showingError = true;
        errorTimeElapsed = 0f;
        while(errorTimeElapsed < errorDuration)
        {
            errorTimeElapsed += Time.deltaTime;
            yield return null;
        }
        errorManager.HideError();
        showingError = false;
    }
}
