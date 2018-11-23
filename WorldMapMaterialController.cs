// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapMaterialController : MonoBehaviour
{
	private void Awake()
	{
		WorldMapMaterialController.switchers = new List<WorldMapMaterialSwitcher>();
	}

	public static void SwitchAllMaterials()
	{
		foreach (WorldMapMaterialSwitcher worldMapMaterialSwitcher in WorldMapMaterialController.switchers)
		{
			if (worldMapMaterialSwitcher != null)
			{
				worldMapMaterialSwitcher.SwitchMaterialBasedOnTerritory();
			}
		}
	}

	public static void RegisterMaterialSwitcher(WorldMapMaterialSwitcher switcher)
	{
		WorldMapMaterialController.switchers.Add(switcher);
	}

	public static void SwitchAllMaterials(WorldTerritory3D onTerritory)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Try Switch materials ",
			WorldMapMaterialController.switchers.Count,
			" onTerritory ",
			onTerritory.name
		}));
		foreach (WorldMapMaterialSwitcher worldMapMaterialSwitcher in WorldMapMaterialController.switchers)
		{
			if (worldMapMaterialSwitcher != null && worldMapMaterialSwitcher.territory == onTerritory)
			{
				worldMapMaterialSwitcher.Invoke("SwitchMaterialBasedOnTerritory", 0.2f * UnityEngine.Random.value);
			}
		}
	}

	private void Update()
	{
	}

	public static List<WorldMapMaterialSwitcher> switchers = new List<WorldMapMaterialSwitcher>();
}
