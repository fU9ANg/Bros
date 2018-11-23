// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Trigger
{
	public TriggerType type
	{
		get
		{
			return this.info.type;
		}
	}

	public string name
	{
		get
		{
			return this.info.name;
		}
	}

	public GridPoint bottomLeft
	{
		get
		{
			return this.info.bottomLeft;
		}
	}

	public GridPoint upperRight
	{
		get
		{
			return this.info.upperRight;
		}
	}

	public string variableName
	{
		get
		{
			return this.info.variableName;
		}
	}

	public float evaluateAgainstValue
	{
		get
		{
			return this.info.evaluateAgainstValue;
		}
	}

	public bool isTerrainEmpty
	{
		get
		{
			return this.info.isTerrainEmpty;
		}
	}

	public bool useDefaultBrotality
	{
		get
		{
			return this.info.useDefaultBrotality;
		}
	}

	public bool useCustomBrotality
	{
		get
		{
			return this.info.useCustomBrotality;
		}
	}

	public int minBrotalityLevel
	{
		get
		{
			return this.info.minBrotalityLevel;
		}
	}

	public int enemyDeathFrequency
	{
		get
		{
			return this.info.enemyDeathFrequency;
		}
	}

	public string tag
	{
		get
		{
			return this.info.tag;
		}
	}

	public void AddAction(TriggerAction action)
	{
		this.actions.Add(action);
	}

	public bool Evaluate()
	{
		if (!this.enabled)
		{
			return false;
		}
		switch (this.type)
		{
		case TriggerType.Area:
			foreach (Player player in HeroController.players)
			{
				if (player != null && player.IsAlive() && player.character != null && player.character.health > 0 && player.character.collumn >= this.bottomLeft.collumn - Map.lastXLoadOffset && player.character.row >= this.bottomLeft.row - Map.lastYLoadOffset && player.character.collumn <= this.upperRight.collumn - Map.lastXLoadOffset && player.character.row <= this.upperRight.row - Map.lastYLoadOffset)
				{
					return true;
				}
			}
			break;
		case TriggerType.Entity:
			if (this.triggerNextFrame)
			{
				this.triggerNextFrame = false;
				return true;
			}
			if (!string.IsNullOrEmpty(this.info.tag) && (this.evaluateDelay -= Time.deltaTime) < 0f)
			{
				this.evaluateDelay = 0.05f;
				for (int j = 0; j < Map.MapData.DoodadList.Count; j++)
				{
					if (!string.IsNullOrEmpty(Map.MapData.DoodadList[j].tag) && Map.MapData.DoodadList[j].tag.Equals(this.info.tag))
					{
						if (Map.MapData.DoodadList[j].entity == null)
						{
							return true;
						}
						if (Map.MapData.DoodadList[j].entity.GetComponent<Unit>() != null)
						{
							return Map.MapData.DoodadList[j].entity.GetComponent<Unit>().health <= 0;
						}
					}
				}
			}
			break;
		case TriggerType.Variable:
			if (TriggerManager.GetVariableValue(this.variableName) >= this.evaluateAgainstValue)
			{
				return true;
			}
			break;
		case TriggerType.CheckTerrain:
			if (this.terrainValues != null)
			{
				for (int k = 0; k <= this.terrainValues.GetUpperBound(0); k++)
				{
					for (int l = 0; l <= this.terrainValues.GetUpperBound(1); l++)
					{
						if (this.isTerrainEmpty && this.terrainValues[k, l] && !Map.IsBlockSolid(k + this.bottomLeft.collumn, l + this.bottomLeft.row))
						{
							return true;
						}
						if (!this.isTerrainEmpty && !this.terrainValues[k, l] && Map.IsBlockSolid(k + this.bottomLeft.collumn, l + this.bottomLeft.row))
						{
							return true;
						}
					}
				}
			}
			else
			{
				UnityEngine.Debug.LogError("No Terrain Values");
			}
			break;
		case TriggerType.OnScreen:
			if (this.minX < SortOfFollow.maxX + 16f && this.maxX > SortOfFollow.minX - 16f && this.minY < SortOfFollow.maxY + 32f && this.maxY > SortOfFollow.minY - 64f)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public void EvaluateEnemyDeathEvent(Unit unit)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Enemy Death! ",
			this.enemyDeathCount,
			"  trigger ",
			this.enemyDeathCount % this.enemyDeathFrequency
		}));
		if (this.type == TriggerType.EnemyDeath)
		{
			this.enemyDeathCount++;
			if (this.enemyDeathFrequency <= 0 || this.enemyDeathCount % this.enemyDeathFrequency == 0)
			{
				foreach (TriggerAction triggerAction in this.actions)
				{
					if (triggerAction.state == TriggerActionState.Done)
					{
						triggerAction.state = TriggerActionState.Waiting;
					}
				}
				TriggerManager.Instance.ExecuteTrigger_Networked(this);
			}
		}
		this.hasBeenTriggered = false;
	}

	public void Setup()
	{
		Map.GetBlocksXY(ref this.minX, ref this.minY, this.bottomLeft.row, this.bottomLeft.collumn);
		Map.GetBlocksXY(ref this.maxX, ref this.maxY, this.upperRight.row, this.upperRight.collumn);
		if (this.type == TriggerType.EnemyDeath && !Map.isEditing)
		{
			foreach (TriggerAction triggerAction in this.actions)
			{
				triggerAction.repeat = true;
			}
			Map.RegisterEnemyDeathListener(this);
		}
		this.enabled = this.info.startEnabled;
	}

	public void SetEvaluationType()
	{
		this.brotalityEvaluationType = BrotalityEvaluationType.None;
		foreach (TriggerAction triggerAction in this.actions)
		{
			if (triggerAction is BombardmentAction)
			{
				this.brotalityEvaluationType = BrotalityEvaluationType.Bombardment;
				break;
			}
			SpawnMooksAction spawnMooksAction = triggerAction as SpawnMooksAction;
			if (spawnMooksAction != null)
			{
				if (spawnMooksAction.info.spawnTruck)
				{
					this.brotalityEvaluationType = BrotalityEvaluationType.Trucks;
				}
				else if (spawnMooksAction.info.parachute)
				{
					this.brotalityEvaluationType = BrotalityEvaluationType.Paratroopers;
				}
				else if (spawnMooksAction.info.isAlert || spawnMooksAction.info.mooksSuicideCount > 0)
				{
					this.brotalityEvaluationType = BrotalityEvaluationType.SuicideMooks;
				}
				else
				{
					this.brotalityEvaluationType = BrotalityEvaluationType.Paratroopers;
				}
				break;
			}
		}
	}

	public bool PassesBrotalityCheck(TriggerAction action)
	{
		if (!this.useCustomBrotality && !this.useDefaultBrotality)
		{
			return true;
		}
		if (this.useCustomBrotality && (this.minBrotalityLevel <= 0 || StatisticsController.GetBrotalityLevel() >= this.minBrotalityLevel))
		{
			return true;
		}
		if (action is BombardmentAction)
		{
			return StatisticsController.GetBrotalityLevel() >= 3;
		}
		SpawnMooksAction spawnMooksAction = action as SpawnMooksAction;
		if (spawnMooksAction == null)
		{
			return true;
		}
		if (spawnMooksAction.info.spawnTruck)
		{
			return StatisticsController.GetBrotalityLevel() >= 2;
		}
		if (spawnMooksAction.info.parachute)
		{
			return StatisticsController.GetBrotalityLevel() >= 1;
		}
		if (spawnMooksAction.info.isAlert || spawnMooksAction.info.mooksSuicideCount > 0)
		{
			return StatisticsController.GetBrotalityLevel() >= 2;
		}
		return StatisticsController.GetBrotalityLevel() >= 1;
	}

	protected bool PassesBrotalityCheck()
	{
		if (!this.useCustomBrotality && !this.useDefaultBrotality)
		{
			return true;
		}
		if (this.useCustomBrotality && (this.minBrotalityLevel <= 0 || StatisticsController.GetBrotalityLevel() >= this.minBrotalityLevel))
		{
			return true;
		}
		if (this.useDefaultBrotality)
		{
			switch (this.brotalityEvaluationType)
			{
			case BrotalityEvaluationType.Paratroopers:
				return StatisticsController.GetBrotalityLevel() >= 1;
			case BrotalityEvaluationType.SuicideMooks:
				return StatisticsController.GetBrotalityLevel() >= 3;
			case BrotalityEvaluationType.Trucks:
				return StatisticsController.GetBrotalityLevel() >= 2;
			case BrotalityEvaluationType.Tanks:
				return StatisticsController.GetBrotalityLevel() >= 3;
			case BrotalityEvaluationType.Bombardment:
				return StatisticsController.GetBrotalityLevel() >= 4;
			}
			return true;
		}
		return false;
	}

	public void Reset()
	{
		this.hasBeenTriggered = false;
		foreach (TriggerAction triggerAction in this.actions)
		{
			triggerAction.Reset();
		}
	}

	public bool hasBeenTriggered;

	public bool triggerNextFrame;

	public List<TriggerAction> actions = new List<TriggerAction>();

	public TriggerInfo info;

	protected float minX;

	protected float minY;

	protected float maxX;

	protected float maxY;

	public bool[,] terrainValues;

	protected int enemyDeathCount;

	private List<DoodadInfo> relevantDoodads;

	private float evaluateDelay = 0.05f;

	public BrotalityEvaluationType brotalityEvaluationType;

	public string networkID = "Not Set";

	public bool enabled;
}
