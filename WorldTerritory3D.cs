// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldTerritory3D : MonoBehaviour
{
	private void Awake()
	{
		this.centerVector3 = base.transform.localPosition;
		this.yAngle = global::Math.GetAngle(this.centerVector3.x, this.centerVector3.z) / 3.14159274f * 180f + 90f;
		this.xAngle = global::Math.GetAngle(this.centerVector3.y, 1f) / 3.14159274f * 180f - 90f;
		if (this.icon != null)
		{
			this.icon.gameObject.SetActive(false);
		}
		this.currentIconTerrorLevel = this.properties.terroristLevel;
		this.currentTerritoryState = this.properties.state;
		if (this.flagPole != null)
		{
			this.flagPole.gameObject.SetActive(false);
		}
		if (this.terroristMapPiece != null)
		{
			this.terroristMapPiece.gameObject.SetActive(false);
		}
	}

	public void DropFlag()
	{
		if (this.flagPole != null)
		{
			this.flagPole.RaiseFlag();
		}
	}

	public void SetState(TerritoryState state)
	{
		this.properties.state = state;
		WorldMapProgressController.SetTerritoryState(base.name, this.properties.state);
		if (state == TerritoryState.TerroristBurning)
		{
			this.currentMaterial = this.terroristBurningMaterial;
			this.SetMaterial(new Material[]
			{
				this.currentMaterial
			}, 0.33f);
			this.hilightMaterial = this.hilightBurningMaterial;
		}
	}

	private void Start()
	{
		if (this.properties.state == TerritoryState.Liberated && this.previousProperties.state != TerritoryState.Liberated)
		{
			UnityEngine.Debug.Log("Should Liberate Now ... HAS PREVIOUS PROPERTIES ");
		}
		if (this.properties.state != TerritoryState.Liberated && this.properties.state != TerritoryState.TerroristBase && this.becomeTerrorBaseAtTurn < WorldMapProgressController.GetTurn() && WorldMapProgressController.CanBeTerroristBaseIfLiberatedNeighbour(this.becomeTerrorBaseWhenNeighbourIsLiberated))
		{
			this.SetState(TerritoryState.TerroristBase);
		}
		TerritoryState state = this.properties.state;
		switch (state + 1)
		{
		case TerritoryState.Empty:
		case TerritoryState.Liberated:
			this.currentMaterial = this.undiscoveredMaterial;
			goto IL_25C;
		case TerritoryState.TerroristBase:
			this.currentMaterial = this.liberatedMaterial;
			if (this.flagPole != null)
			{
				this.flagPole.gameObject.SetActive(true);
			}
			goto IL_25C;
		case TerritoryState.TerroristAirBase:
			if (this.previousProperties.state != TerritoryState.TerroristBase)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"  name : ",
					base.name,
					"NEW TERRORIST BASE!! ",
					this.previousProperties.state
				}));
				WorldEventController.AddCurrentEvent(new WorldMapEvent(WorldMapEventType.NewMission, this, this, 1.5f));
			}
			else
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"  name : ",
					base.name,
					"WAS ALREADY TERRORIST BASE!! ",
					this.previousProperties.state
				}));
				this.ShowTerrorIcon();
				if (this.terroristMapPiece != null)
				{
					this.terroristMapPiece.Appear(this, false);
				}
			}
			this.currentMaterial = this.terroristMaterial;
			goto IL_25C;
		case TerritoryState.InfestedBurning:
			if (this.icon != null)
			{
				this.icon.Appear(this.currentIconTerrorLevel);
			}
			this.currentMaterial = this.terroristBurningMaterial;
			goto IL_25C;
		case TerritoryState.Brofort:
			this.currentMaterial = this.obliteratedMaterial;
			if (this.hospital != null)
			{
				this.hospital.SetActive(false);
			}
			goto IL_25C;
		case TerritoryState.ForwardBase:
		case (TerritoryState)12:
			this.currentMaterial = this.brofortMaterial;
			goto IL_25C;
		}
		UnityEngine.Debug.LogError("Not Implemented Yet");
		IL_25C:
		this.SetMaterial(new Material[]
		{
			this.currentMaterial
		}, 0f);
	}

	public void BecomeTerroristBase()
	{
		this.ShowTerrorIcon();
		if (this.terroristMapPiece != null)
		{
			this.terroristMapPiece.Appear(this, true);
		}
	}

	public void ShowTerrorIcon()
	{
		if (this.icon != null)
		{
			this.icon.Appear(this.currentIconTerrorLevel);
		}
	}

	public void SetMaterial(Material[] materials, float delay)
	{
		if (this.terrainHolder != null)
		{
			this.terrainHolder.SetMaterial(materials);
		}
		if (delay <= 0f)
		{
			for (int i = 0; i < this.terrainColliders.Count; i++)
			{
				this.terrainColliders[i].GetComponent<Renderer>().sharedMaterials = materials;
			}
		}
		else
		{
			for (int j = 0; j < this.terrainColliders.Count; j++)
			{
				this.terrainColliders[j].setToMaterials = materials;
				this.terrainColliders[j].Invoke("SwitchMaterialsDelay", UnityEngine.Random.value * delay);
			}
		}
	}

	public void ShowTerritoryBorders(bool showThem)
	{
		for (int i = 0; i < this.terrainColliders.Count; i++)
		{
			if (showThem)
			{
				this.terrainColliders[i].ShowBorder();
			}
			else
			{
				this.terrainColliders[i].HideBorder();
			}
		}
	}

	public void AddTerrorVisual(int amount)
	{
		if (this.currentTerritoryState == TerritoryState.Empty)
		{
			this.currentMaterial = this.terroristMaterial;
		}
		this.currentIconTerrorLevel += amount;
		if (this.icon != null)
		{
			this.icon.Appear(this.currentIconTerrorLevel);
		}
	}

	public bool HasHospital()
	{
		return this.hospital != null;
	}

	public bool IsSelected()
	{
		return this.selected;
	}

	public void Select(bool isSelected)
	{
		if (isSelected && this.properties.state != TerritoryState.Empty)
		{
			if (this.icon.gameObject.activeSelf)
			{
				this.icon.Bounce();
			}
			this.SetMaterial(new Material[]
			{
				this.currentMaterial,
				this.hilightMaterial
			}, 0f);
			this.ShowTerritoryBorders(true);
		}
		else if (!isSelected)
		{
			this.SetMaterial(new Material[]
			{
				this.currentMaterial
			}, 0f);
			this.ShowTerritoryBorders(false);
		}
		this.selected = isSelected;
	}

	public Vector3 GetCentreWorldLocal()
	{
		if (this.centreObject != null)
		{
			return this.centreObject.position;
		}
		return base.transform.localPosition;
	}

	public Vector3 GetCentre()
	{
		if (this.centreObject != null)
		{
			return this.centreObject.position;
		}
		return base.transform.TransformPoint(this.centerVector3);
	}

	public WorldTerritory3D GetUpperRightTerritory()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 1f;
		WorldTerritory3D result = null;
		foreach (WorldTerritory3D worldTerritory3D in this.connectedTerritories)
		{
			if (worldTerritory3D.xAngle - this.xAngle < num && worldTerritory3D.yAngle - this.yAngle > num2)
			{
				float num4 = Mathf.Abs(worldTerritory3D.yAngle - this.yAngle) + Mathf.Abs(worldTerritory3D.xAngle - this.xAngle);
				float num5 = Mathf.Abs(Mathf.Abs(worldTerritory3D.yAngle - this.yAngle) / num4 - Mathf.Abs(worldTerritory3D.xAngle - this.xAngle) / num4);
				if (num5 < num3)
				{
					num3 = num5;
					result = worldTerritory3D;
				}
			}
		}
		return result;
	}

	public WorldTerritory3D GetLowerRightTerritory()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 1f;
		WorldTerritory3D result = null;
		foreach (WorldTerritory3D worldTerritory3D in this.connectedTerritories)
		{
			if (worldTerritory3D.xAngle - this.xAngle > num && worldTerritory3D.yAngle - this.yAngle > num2)
			{
				float num4 = Mathf.Abs(worldTerritory3D.yAngle - this.yAngle) + Mathf.Abs(worldTerritory3D.xAngle - this.xAngle);
				float num5 = Mathf.Abs(Mathf.Abs(worldTerritory3D.yAngle - this.yAngle) / num4 - Mathf.Abs(worldTerritory3D.xAngle - this.xAngle) / num4);
				if (num5 < num3)
				{
					num3 = num5;
					result = worldTerritory3D;
				}
			}
		}
		return result;
	}

	public WorldTerritory3D GetUpperLeftTerritory()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 1f;
		WorldTerritory3D result = null;
		foreach (WorldTerritory3D worldTerritory3D in this.connectedTerritories)
		{
			if (worldTerritory3D.xAngle - this.xAngle < num && worldTerritory3D.yAngle - this.yAngle < num2)
			{
				float num4 = Mathf.Abs(worldTerritory3D.yAngle - this.yAngle) + Mathf.Abs(worldTerritory3D.xAngle - this.xAngle);
				float num5 = Mathf.Abs(Mathf.Abs(worldTerritory3D.yAngle - this.yAngle) / num4 - Mathf.Abs(worldTerritory3D.xAngle - this.xAngle) / num4);
				if (num5 < num3)
				{
					num3 = num5;
					result = worldTerritory3D;
				}
			}
		}
		return result;
	}

	public WorldTerritory3D GetLowerLeftTerritory()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 1f;
		WorldTerritory3D result = null;
		foreach (WorldTerritory3D worldTerritory3D in this.connectedTerritories)
		{
			if (worldTerritory3D.xAngle - this.xAngle > num && worldTerritory3D.yAngle - this.yAngle < num2)
			{
				float num4 = Mathf.Abs(worldTerritory3D.yAngle - this.yAngle) + Mathf.Abs(worldTerritory3D.xAngle - this.xAngle);
				float num5 = Mathf.Abs(Mathf.Abs(worldTerritory3D.yAngle - this.yAngle) / num4 - Mathf.Abs(worldTerritory3D.xAngle - this.xAngle) / num4);
				if (num5 < num3)
				{
					num3 = num5;
					result = worldTerritory3D;
				}
			}
		}
		return result;
	}

	public WorldTerritory3D GetUpperTerritory()
	{
		float num = 0f;
		WorldTerritory3D result = null;
		foreach (WorldTerritory3D worldTerritory3D in this.connectedTerritories)
		{
			if (worldTerritory3D.xAngle - this.xAngle < num)
			{
				num = worldTerritory3D.xAngle - this.xAngle;
				result = worldTerritory3D;
			}
		}
		return result;
	}

	public WorldTerritory3D GetLowerTerritory()
	{
		float num = 0f;
		WorldTerritory3D result = null;
		foreach (WorldTerritory3D worldTerritory3D in this.connectedTerritories)
		{
			if (worldTerritory3D.xAngle - this.xAngle > num)
			{
				num = worldTerritory3D.xAngle - this.xAngle;
				result = worldTerritory3D;
			}
		}
		return result;
	}

	public WorldTerritory3D GetRightTerritory()
	{
		float num = 0f;
		WorldTerritory3D result = null;
		foreach (WorldTerritory3D worldTerritory3D in this.connectedTerritories)
		{
			if (worldTerritory3D.yAngle - this.yAngle > num)
			{
				num = worldTerritory3D.yAngle - this.yAngle;
				result = worldTerritory3D;
			}
		}
		return result;
	}

	public WorldTerritory3D GetLeftTerritory()
	{
		float num = 0f;
		WorldTerritory3D result = null;
		foreach (WorldTerritory3D worldTerritory3D in this.connectedTerritories)
		{
			if (worldTerritory3D.yAngle - this.yAngle < num)
			{
				num = worldTerritory3D.yAngle - this.yAngle;
				result = worldTerritory3D;
			}
		}
		return result;
	}

	private void Update()
	{
	}

	public TerritoryProperties properties;

	public TerritoryProperties previousProperties;

	public Material undiscoveredMaterial;

	public Material terroristMaterial;

	public Material terroristBurningMaterial;

	public Material liberatedMaterial;

	public Material obliteratedMaterial;

	public Material brofortMaterial;

	public Material hilightMaterial;

	public Material hilightBurningMaterial;

	protected Material currentMaterial;

	public WMIcon icon;

	protected int currentIconTerrorLevel;

	protected TerritoryState currentTerritoryState;

	public bool selected;

	public bool testForceRotation;

	public WorldTerritory3D[] connectedTerritories;

	public GameObject hospital;

	public GameObject smokeEmitter;

	public WorldMapFlagPole flagPole;

	public WorldMapTerroristMapPiece terroristMapPiece;

	[HideInInspector]
	public List<WorldMapTerritoryCollider> terrainColliders = new List<WorldMapTerritoryCollider>();

	public WorldTerritory3DTerrain terrainHolder;

	[HideInInspector]
	public Vector3 centerVector3 = Vector3.zero;

	[HideInInspector]
	public float yAngle;

	[HideInInspector]
	public float xAngle;

	public Transform centreObject;

	public int becomeTerrorBaseAtTurn = -1;

	public WorldTerritory3D[] becomeTerrorBaseWhenNeighbourIsLiberated;
}
