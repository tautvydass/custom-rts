using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class TopBar
{
	[SerializeField]
	private Text wood;
	[SerializeField]
	private Text food;
	[SerializeField]
	private Text stone;
	[SerializeField]
	private Text metal;

	[SerializeField]
	private Text population;

	public Action<Resources> GetOnResourcesUpdatedAction()
	{
		return (resources) => OnResourcesUpdated(resources);
	}

	public Action<Population> GetOnPopulationUpdatedAction()
	{
		return (population) => OnPopulationUpdated(population);
	}

	private void OnResourcesUpdated(Resources resources)
	{
		wood.text = resources.wood.ToString();
		food.text = resources.food.ToString();
		stone.text = resources.stone.ToString();
		metal.text = resources.metal.ToString();
	}

	private void OnPopulationUpdated(Population population)
	{
		this.population.text = $"{ population.current } / { population.allowed }";
	}
}
