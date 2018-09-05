using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[Serializable]
public class InfoBar
{
	[SerializeField]
	private Image icon;

	[SerializeField]
	private Text nameText;

	[SerializeField]
	private Text healthText;

	[SerializeField]
	private GameObject infoGameObject;

	public void DisplayInfo(List<ISelectable> selectables)
	{
		infoGameObject.SetActive(true);

		var selectionInfo = selectables[0].GetSelectionInfo();

		icon.sprite = selectionInfo.Icon;
		healthText.text = $"{ selectionInfo.CurrentHitpoints } / { selectionInfo.MaxHitpoints }";
		nameText.text = $"{ selectionInfo.Name }";
	}

	public void HideUnitInfo()
	{
		infoGameObject.SetActive(false);
	}
}
