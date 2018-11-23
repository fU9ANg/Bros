// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapTerroristMapPiece : MonoBehaviour
{
	private void Start()
	{
	}

	public void Appear(WorldTerritory3D originTerritory, bool forTheFirstTime)
	{
		base.gameObject.SetActive(true);
		this.territory = originTerritory;
		if (forTheFirstTime)
		{
			this.PerformMapFunction();
		}
	}

	protected void PerformMapFunction()
	{
		this.territory.SetState(TerritoryState.TerroristBurning);
		WorldMapMaterialController.SwitchAllMaterials(this.territory);
		WorldEventController.AddDelay(0.5f);
		UnityEngine.Debug.Log("Perform Map Piece Function ");
	}

	private void Update()
	{
	}

	public bool setTerritoryOnFire = true;

	protected WorldTerritory3D territory;
}
