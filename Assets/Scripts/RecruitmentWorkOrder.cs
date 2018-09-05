using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Recruitment Work Order Properties", menuName = "RTS/Recruitment Work Order Properties")]
public class RecruitmentWorkOrder : WorkOrder
{
	public GameObject unit;

	[SerializeField]
	private UnitProperties unitProperties;

	public int Population => unitProperties.population;
}
