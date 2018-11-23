// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapTerritory : MonoBehaviour
{
	private void Awake()
	{
		this.numberText.gameObject.SetActive(false);
	}

	public void Complete()
	{
	}

	public void SetTerritoryActive()
	{
		base.GetComponent<Renderer>().sharedMaterial = this.activeMaterial;
	}

	public void SetTerritoryInactive()
	{
		base.GetComponent<Renderer>().sharedMaterial = this.deactiveMaterial;
	}

	public void UpdateTerritoryVisuals()
	{
		if (WorldMapTerritoriesController.IsTerritoryActive(this.collumn, this.row))
		{
			this.SetTerritoryActive();
		}
		else
		{
			this.SetTerritoryInactive();
		}
	}

	public void Run()
	{
		if (this.terrorAmount > 0)
		{
			this.terrorAmount++;
			if (this.terrorAmount >= this.maxTerror)
			{
				this.SpreadTerror();
				this.terrorAmount = this.maxTerror;
			}
		}
	}

	protected void SpreadTerror()
	{
		UnityEngine.Debug.LogError("Broken Now");
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public int collumn = -1;

	public int row = -1;

	public int terrorAmount;

	public int maxTerror = 4;

	public WorldLocation location;

	public TextMesh numberText;

	public Material activeMaterial;

	public Material deactiveMaterial;

	public bool canPlaceBase = true;

	public SpriteSM baseIconPrefab;

	protected SpriteSM baseIcon;

	[HideInInspector]
	public string selectedFunctionName = string.Empty;
}
