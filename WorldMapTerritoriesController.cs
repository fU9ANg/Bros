// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapTerritoriesController : MonoBehaviour
{
	private void Awake()
	{
		WorldMapTerritoriesController.instance = this;
	}

	private void Start()
	{
		List<WorldMapTerritory> list = new List<WorldMapTerritory>();
		list.AddRange(base.gameObject.GetComponentsInChildren<WorldMapTerritory>());
		UnityEngine.Debug.Log(" Territories " + list.Count);
		foreach (WorldMapTerritory worldMapTerritory in list)
		{
			if (worldMapTerritory.transform.localPosition.x < (float)this.xMin)
			{
				this.xMin = (int)Mathf.Round(worldMapTerritory.transform.localPosition.x);
			}
			if (worldMapTerritory.transform.localPosition.x > (float)this.xMax)
			{
				this.xMax = (int)Mathf.Round(worldMapTerritory.transform.localPosition.x);
			}
			if (worldMapTerritory.transform.localPosition.y < (float)this.yMin)
			{
				this.yMin = (int)Mathf.Round(worldMapTerritory.transform.localPosition.y);
			}
			if (worldMapTerritory.transform.localPosition.y > (float)this.yMax)
			{
				this.yMax = (int)Mathf.Round(worldMapTerritory.transform.localPosition.y);
			}
			this.collumns = (this.xMax - this.xMin) / 8 + 1;
			this.rows = (this.yMax - this.yMin) / 8 + 1;
		}
		WorldMapTerritoriesController.territories = new WorldMapTerritory[this.collumns, this.rows];
		foreach (WorldMapTerritory worldMapTerritory2 in list)
		{
			float f = worldMapTerritory2.transform.localPosition.x - (float)this.xMin;
			float f2 = worldMapTerritory2.transform.localPosition.y - (float)this.yMin;
			int num = (int)(Mathf.Round(f) / 8f);
			int num2 = (int)(Mathf.Round(f2) / 8f);
			WorldMapTerritoriesController.territories[num, num2] = worldMapTerritory2;
			worldMapTerritory2.collumn = num;
			worldMapTerritory2.row = num2;
		}
		UnityEngine.Debug.Log("RELOAD SAVED territories " + WorldMapTerritoriesController.savedTerritories.Count);
		WorldMapTerritoriesController.UpdateTerritoryVisuals();
	}

	public static void AddBaseToList(WorldMapTerritory territory)
	{
		WorldMapTerritoriesController.instance.bases.Add(territory);
	}

	public static void SetTemporaryBase(int collumn, int row)
	{
		WorldMapTerritoriesController.temporaryBaseGridPoint = new GridPoint(collumn, row);
		WorldMapTerritoriesController.UpdateTerritoryVisuals();
	}

	public static void ClearTemporaryBase()
	{
		WorldMapTerritoriesController.temporaryBaseGridPoint = new GridPoint(-100, -100);
		WorldMapTerritoriesController.UpdateTerritoryVisuals();
	}

	public static void UpdateTerritoryVisuals()
	{
		for (int i = 0; i <= WorldMapTerritoriesController.territories.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= WorldMapTerritoriesController.territories.GetUpperBound(1); j++)
			{
				if (WorldMapTerritoriesController.territories[i, j] != null)
				{
					WorldMapTerritoriesController.territories[i, j].UpdateTerritoryVisuals();
				}
			}
		}
	}

	public static bool IsTerritoryActive(int c, int r)
	{
		int num = Mathf.Abs(c - WorldMapTerritoriesController.temporaryBaseGridPoint.collumn);
		int num2 = Mathf.Abs(r - WorldMapTerritoriesController.temporaryBaseGridPoint.row);
		if (Mathf.Sqrt((float)(num * num + num2 * num2)) <= 2.6f)
		{
			return true;
		}
		foreach (WorldMapTerritory worldMapTerritory in WorldMapTerritoriesController.instance.bases)
		{
			num = Mathf.Abs(c - worldMapTerritory.collumn);
			num2 = Mathf.Abs(r - worldMapTerritory.row);
			if (Mathf.Sqrt((float)(num * num + num2 * num2)) <= 2.6f)
			{
				return true;
			}
		}
		return false;
	}

	public static void SetToSelectTerritories(bool canBeSelected, string functionName)
	{
		if (!canBeSelected)
		{
			WorldMapTerritoriesController.temporaryBaseGridPoint = new GridPoint(-1, -1);
		}
		for (int i = 0; i <= WorldMapTerritoriesController.territories.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= WorldMapTerritoriesController.territories.GetUpperBound(1); j++)
			{
				if (WorldMapTerritoriesController.territories[i, j] != null)
				{
					WorldMapTerritoriesController.territories[i, j].GetComponent<Collider>().enabled = canBeSelected;
					if (canBeSelected)
					{
						WorldMapTerritoriesController.territories[i, j].selectedFunctionName = functionName;
					}
				}
			}
		}
	}

	public static void RunTerritories()
	{
		for (int i = 0; i <= WorldMapTerritoriesController.territories.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= WorldMapTerritoriesController.territories.GetUpperBound(1); j++)
			{
				if (WorldMapTerritoriesController.territories[i, j] != null)
				{
					WorldMapTerritoriesController.territories[i, j].Run();
				}
			}
		}
	}

	public static void SaveTerritories()
	{
		WorldMapTerritoriesController.savedTerritories = new List<WorldMapTerrainSaveInfo>();
		for (int i = 0; i <= WorldMapTerritoriesController.territories.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= WorldMapTerritoriesController.territories.GetUpperBound(1); j++)
			{
				if (WorldMapTerritoriesController.territories[i, j] != null && WorldMapTerritoriesController.territories[i, j].location != null)
				{
					if (WorldMapTerritoriesController.territories[i, j].location.isBase)
					{
						WorldMapTerritoriesController.savedTerritories.Add(new WorldMapTerrainSaveInfo(WorldMapTerritoriesController.territories[i, j].location.isCompleted, WorldMapTerritoriesController.territories[i, j].location.isBase, i, j));
					}
					else
					{
						WorldMapTerritoriesController.savedTerritories.Add(new WorldMapTerrainSaveInfo(WorldMapTerritoriesController.territories[i, j].location.isCompleted, i, j, WorldMapTerritoriesController.territories[i, j].location.name, WorldMapTerritoriesController.territories[i, j].location.transform.position, WorldMapTerritoriesController.territories[i, j].location.missionDifficulty, WorldMapTerritoriesController.territories[i, j].location.missionLength));
					}
				}
			}
		}
		UnityEngine.Debug.Log("SAVE TERRITORIES " + WorldMapTerritoriesController.savedTerritories.Count);
	}

	public static bool HasNearbyBase(int collumn, int row)
	{
		for (int i = 0; i <= WorldMapTerritoriesController.territories.GetUpperBound(0); i++)
		{
			for (int j = 0; j <= WorldMapTerritoriesController.territories.GetUpperBound(1); j++)
			{
				if (WorldMapTerritoriesController.territories[i, j] != null && WorldMapTerritoriesController.territories[i, j].location != null && WorldMapTerritoriesController.territories[i, j].location.isBase)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool HasNearbyTerroristAirDefence(int collumn, int row)
	{
		for (int i = collumn - 1; i <= collumn + 1; i++)
		{
			if (i >= 0 && i <= WorldMapTerritoriesController.territories.GetUpperBound(0))
			{
				for (int j = row; j <= row + 1; j++)
				{
					if (j >= 0 && j <= WorldMapTerritoriesController.territories.GetUpperBound(1) && WorldMapTerritoriesController.territories[i, j] != null && WorldMapTerritoriesController.territories[i, j].location != null && WorldMapTerritoriesController.territories[i, j].location.hasTerroristAirDefence)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private void Update()
	{
	}

	public static Vector3 GetPos(int collumn, int row)
	{
		if (collumn >= 0 && collumn <= WorldMapTerritoriesController.territories.GetUpperBound(0) && row >= 0 && row <= WorldMapTerritoriesController.territories.GetUpperBound(1) && WorldMapTerritoriesController.territories[collumn, row] != null)
		{
			return WorldMapTerritoriesController.territories[collumn, row].transform.position;
		}
		return Vector3.zero;
	}

	public const int MAX_TERROR_LEVEL = 6;

	public const int MAX_INFESTATION_LEVEL = 3;

	protected int xMin = 1000;

	protected int xMax = -1000;

	protected int yMin = 1000;

	protected int yMax = -1000;

	protected int rows = 1;

	protected int collumns = 1;

	protected List<WorldMapTerritory> bases = new List<WorldMapTerritory>();

	protected static WorldMapTerritory[,] territories;

	protected static List<WorldMapTerrainSaveInfo> savedTerritories = new List<WorldMapTerrainSaveInfo>();

	protected static WorldMapTerritoriesController instance;

	protected static GridPoint temporaryBaseGridPoint = new GridPoint(-100, -100);
}
