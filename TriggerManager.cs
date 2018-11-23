// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
	public static TriggerManager Instance
	{
		get
		{
			return TriggerManager.instance;
		}
	}

	public static Dictionary<NID, Trigger> Triggers
	{
		get
		{
			return TriggerManager.instance.triggers;
		}
		set
		{
			TriggerManager.instance.triggers = value;
		}
	}

	private void Awake()
	{
		TriggerManager.instance = this;
		this.activeActions = new List<TriggerAction>();
		this.triggers = new Dictionary<NID, Trigger>();
		TriggerManager.bombardments = new List<BombardmentRepeat>();
		TriggerManager.triggerVariables = new Dictionary<string, float>();
	}

	public static void Reset()
	{
		foreach (Trigger trigger in TriggerManager.instance.triggers.Values)
		{
			trigger.Reset();
		}
	}

	public static float GetVariableValue(string variableName)
	{
		if (TriggerManager.triggerVariables.ContainsKey(variableName))
		{
			return TriggerManager.triggerVariables[variableName];
		}
		return -1f;
	}

	public static void AlterVariableValue(string variableName, float amount)
	{
		if (TriggerManager.triggerVariables.ContainsKey(variableName))
		{
			Dictionary<string, float> dictionary2;
			Dictionary<string, float> dictionary = dictionary2 = TriggerManager.triggerVariables;
			float num = dictionary2[variableName];
			dictionary[variableName] = num + amount;
		}
		else
		{
			TriggerManager.triggerVariables.Add(variableName, amount);
		}
	}

	public static void CreateScriptedExplosion(Vector3 location, int damage)
	{
		HiddenExplosives hiddenExplosives = UnityEngine.Object.Instantiate(SingletonMono<MapController>.Instance.explosionPrefab, location, Quaternion.identity) as HiddenExplosives;
		hiddenExplosives.damage = damage;
		hiddenExplosives.Explode();
	}

	public static void CreateScriptedCollapse(Vector3 location)
	{
		Block block = Map.GetBlock(Map.GetCollumn(location.x), Map.GetRow(location.y));
		if (block != null)
		{
			Networking.RPC<DamageObject>(PID.TargetAll, new RpcSignature<DamageObject>(block.Damage), new DamageObject(1, DamageType.InstaGib, 0f, 0f, null), false);
		}
		else
		{
			UnityEngine.Debug.LogError("Collapse Block is Null");
		}
	}

	public static void CreateScriptedBurn(Vector3 location)
	{
		Block block = Map.GetBlock(Map.GetCollumn(location.x), Map.GetRow(location.y));
		if (block != null)
		{
			block.gameObject.SendMessage("Damage", new DamageObject(3, DamageType.Fire, 0f, 0f, null));
		}
		else
		{
			UnityEngine.Debug.LogError("Collapse Block is Null");
		}
	}

	public static void CreateScriptedBlock(GridPoint gridPoint, GroundType groundType, bool disturbed)
	{
		float propaneTankSpawnProbability = Map.MapData.propaneTankSpawnProbability;
		Map.MapData.propaneTankSpawnProbability = 0f;
		Block block = Map.Instance.PlaceGround(groundType, gridPoint.collumn, gridPoint.row, ref Map.blocks);
		Map.MapData.propaneTankSpawnProbability = propaneTankSpawnProbability;
		if (block == null)
		{
			UnityEngine.Debug.LogError("Created Block is Null");
		}
		else if (disturbed)
		{
			Networking.RPC(PID.TargetAll, new RpcSignature(block.StepOnBlockForced), false);
		}
	}

	public static void CreateScriptedBombardment(Vector3 pos, bool repeat, int seed)
	{
		ProjectileController.SpawnProjectileLocally(ProjectileController.instance.shellBombardment, SingletonMono<MapController>.Instance, pos.x + 300f, pos.y + 450f, -187.5f, -281.25f, -1);
		if (repeat)
		{
			TriggerManager.AddRepeatingBombardment(pos, seed);
		}
	}

	public static BombardmentRepeat AddRepeatingBombardment(Vector3 pos, int seed)
	{
		BombardmentRepeat bombardmentRepeat = new BombardmentRepeat(pos, seed);
		TriggerManager.bombardments.Add(bombardmentRepeat);
		return bombardmentRepeat;
	}

	private void Update()
	{
		if (Map.isEditing)
		{
			return;
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.03334f);
		TriggerManager.destroyOffscreenPlayers = false;
		TriggerManager.blockingUnitMovement = false;
		foreach (Trigger trigger in this.triggers.Values)
		{
			if (!trigger.hasBeenTriggered && trigger.Evaluate())
			{
				this.ExecuteTrigger_Networked(trigger);
			}
		}
		int count = this.activeActions.Count;
		foreach (TriggerAction triggerAction in this.activeActions)
		{
			if ((triggerAction.state == TriggerActionState.Waiting || triggerAction.repeat) && (triggerAction.Info.type != TriggerActionType.CameraMove || !TriggerManager.PauseCameraMovements))
			{
				triggerAction.timeOffsetLeft -= num;
				if (triggerAction.timeOffsetLeft <= 0f)
				{
					triggerAction.Start();
				}
			}
			if (triggerAction.state == TriggerActionState.Busy)
			{
				triggerAction.Update();
			}
		}
		this.activeActions.RemoveAll((TriggerAction a) => a.state == TriggerActionState.Done);
		if (count > 0)
		{
			bool flag = false;
			for (int i = 0; i < this.activeActions.Count; i++)
			{
				if (this.activeActions[i].Info.type == TriggerActionType.CameraMove)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				SortOfFollow.ControlledByTriggerAction = false;
				SortOfFollow.SetZoomLevel(1f);
				HeroController.EnableHud();
			}
		}
		foreach (BombardmentRepeat bombardmentRepeat in TriggerManager.bombardments)
		{
			bombardmentRepeat.countDown -= num;
			if (bombardmentRepeat.countDown <= 0f)
			{
				float num2 = bombardmentRepeat.random.value * 1.5f;
				bombardmentRepeat.countDown += 3f + num2;
				if (SetResolutionCamera.IsItSortOfVisible(bombardmentRepeat.targetPoint))
				{
					int num3 = bombardmentRepeat.random.Range(0, 11) - 5;
					ProjectileController.SpawnProjectileLocally(ProjectileController.instance.shellBombardment, SingletonMono<MapController>.Instance, bombardmentRepeat.targetPoint.x + 300f + (float)(num3 * 16), bombardmentRepeat.targetPoint.y + 450f, -187.5f, -281.25f, -1);
				}
			}
		}
	}

	public void ExecuteTrigger_Networked(Trigger trigger)
	{
		Networking.RPC<Trigger>(PID.TargetAll, new RpcSignature<Trigger>(this.ExecuteTrigger_Local), trigger, false);
	}

	private void ExecuteTrigger_Local(Trigger trigger)
	{
		if (!trigger.hasBeenTriggered)
		{
			trigger.hasBeenTriggered = true;
			foreach (TriggerAction triggerAction in trigger.actions)
			{
				if ((!(triggerAction is SpawnResourceAction) && !(triggerAction is SpawnMooksAction)) || Connect.IsHost)
				{
					if (trigger.PassesBrotalityCheck(triggerAction))
					{
						this.activeActions.Add(triggerAction);
					}
				}
			}
		}
	}

	public static void LoadTriggers(List<TriggerInfo> triggerInfos)
	{
		if (TriggerManager.instance == null)
		{
			UnityEngine.Debug.LogError("No triggermanager instance - revert your MapController prefab/check script execution order (TriggerManager must be before Map)");
			return;
		}
		TriggerManager.Triggers.Clear();
		TriggerManager.PauseCameraMovements = false;
		foreach (TriggerInfo info in triggerInfos)
		{
			NID nid = Registry.AllocateDeterministicID();
			TriggerManager.Triggers.Add(nid, TriggerFactory.CreateTrigger(info, nid));
		}
	}

	public static Trigger RegisterEntityTrigger(TriggerInfo info)
	{
		if (TriggerManager.instance == null)
		{
			UnityEngine.Debug.LogError("No triggermanager instance - revert your MapController prefab/check script execution order (TriggerManager must be before Map)");
			return null;
		}
		NID nid = Registry.AllocateDeterministicID();
		TriggerManager.Triggers.Add(nid, TriggerFactory.CreateTrigger(info, nid));
		return TriggerManager.Triggers[nid];
	}

	public static void AddCountdownAction(float offset)
	{
		SystemActionInfo systemActionInfo = new SystemActionInfo();
		systemActionInfo.timeOffset = offset;
		systemActionInfo.type = TriggerActionType.System;
		systemActionInfo.systemType = SystemActionType.StartCountdown;
		TriggerManager.instance.activeActions.Add(TriggerFactory.CreateAction(systemActionInfo));
	}

	public static void AddPlayerIntroAction(Player player, float timeOffset)
	{
		if (player.character == null)
		{
			return;
		}
		CameraActionInfo cameraActionInfo = new CameraActionInfo();
		cameraActionInfo.timeOffset = 1f + (float)player.playerNum * 1f + timeOffset;
		cameraActionInfo.time = 0.2f;
		cameraActionInfo.zoom = 0.6f;
		cameraActionInfo.targetPoint = Map.GetGridPoint(player.GetCharacterPosition());
		cameraActionInfo.targetPoint.row++;
		cameraActionInfo.type = TriggerActionType.CameraMove;
		TriggerManager.instance.activeActions.Add(TriggerFactory.CreateAction(cameraActionInfo));
		cameraActionInfo = new CameraActionInfo();
		cameraActionInfo.timeOffset = 1f + (float)player.playerNum * 1f + timeOffset;
		cameraActionInfo.time = 0.8f;
		cameraActionInfo.zoom = 0.6f;
		cameraActionInfo.targetPoint = Map.GetGridPoint(player.GetCharacterPosition());
		cameraActionInfo.targetPoint.row++;
		cameraActionInfo.type = TriggerActionType.CameraMove;
		TriggerManager.instance.activeActions.Add(TriggerFactory.CreateAction(cameraActionInfo));
		SystemActionInfo systemActionInfo = new SystemActionInfo();
		systemActionInfo.timeOffset = 1f + (float)player.playerNum * 1f + timeOffset;
		systemActionInfo.type = TriggerActionType.System;
		systemActionInfo.systemType = SystemActionType.HighlightPlayer;
		systemActionInfo.playernum = player.playerNum;
		TriggerManager.instance.activeActions.Add(TriggerFactory.CreateAction(systemActionInfo));
		systemActionInfo = new SystemActionInfo();
		systemActionInfo.timeOffset = 1f + (float)player.playerNum * 1f + timeOffset;
		systemActionInfo.type = TriggerActionType.System;
		systemActionInfo.systemType = SystemActionType.InfoBarText;
		systemActionInfo.text = HeroController.GetHeroName(player.heroType);
		TriggerManager.instance.activeActions.Add(TriggerFactory.CreateAction(systemActionInfo));
	}

	public static void AddExplosionRunSwoopAction()
	{
		CameraActionInfo cameraActionInfo = new CameraActionInfo();
		cameraActionInfo.timeOffset = 1E-06f;
		cameraActionInfo.time = 1E-06f;
		cameraActionInfo.zoom = 1f;
		cameraActionInfo.targetPoint = Map.GetGridPoint(Map.GetNearestCheckPoint(10000000, 0f, 0f).transform.position);
		cameraActionInfo.type = TriggerActionType.CameraMove;
		TriggerManager.instance.activeActions.Add(TriggerFactory.CreateAction(cameraActionInfo));
		cameraActionInfo = new CameraActionInfo();
		cameraActionInfo.timeOffset = 1f;
		cameraActionInfo.time = 3f;
		cameraActionInfo.zoom = 1f;
		cameraActionInfo.targetPoint = Map.GetGridPoint(SortOfFollow.instance.GetStartPosition());
		cameraActionInfo.targetPoint.collumn++;
		cameraActionInfo.type = TriggerActionType.CameraMove;
		TriggerManager.instance.activeActions.Add(TriggerFactory.CreateAction(cameraActionInfo));
	}

	public static bool PauseCameraMovements { get; set; }

	internal static void ClearActions()
	{
		TriggerManager.instance.activeActions.Clear();
		SortOfFollow.ControlledByTriggerAction = false;
	}

	internal static void ClearActiveCameraActions()
	{
		foreach (TriggerAction triggerAction in TriggerManager.instance.activeActions)
		{
			if (triggerAction.Info.type == TriggerActionType.CameraMove)
			{
				triggerAction.state = TriggerActionState.Done;
			}
		}
	}

	internal static bool CheckAndTriggerLevelEndTriggers()
	{
		foreach (Trigger trigger in TriggerManager.instance.triggers.Values)
		{
			if (!trigger.hasBeenTriggered && trigger.enabled)
			{
				if (trigger.type == TriggerType.LevelFail && GameModeController.GetLevelResult() == LevelResult.Fail)
				{
					TriggerManager.instance.ExecuteTrigger_Networked(trigger);
					return true;
				}
				if (trigger.type == TriggerType.LevelSuccess && GameModeController.GetLevelResult() == LevelResult.Success)
				{
					TriggerManager.instance.ExecuteTrigger_Networked(trigger);
					return true;
				}
			}
		}
		return false;
	}

	internal static void ActivateTrigger(string p)
	{
		foreach (Trigger trigger in TriggerManager.instance.triggers.Values)
		{
			if (!string.IsNullOrEmpty(trigger.name) && trigger.name.ToUpper().Equals(p.ToUpper()))
			{
				trigger.enabled = true;
			}
		}
	}

	internal static void DeactivateTrigger(string p)
	{
		foreach (Trigger trigger in TriggerManager.instance.triggers.Values)
		{
			if (!string.IsNullOrEmpty(trigger.name) && trigger.name.ToUpper().Equals(p.ToUpper()))
			{
				trigger.enabled = false;
			}
		}
	}

	protected static TriggerManager instance;

	protected List<TriggerAction> activeActions;

	protected Dictionary<NID, Trigger> triggers;

	public static bool blockingUnitMovement;

	public static List<BombardmentRepeat> bombardments = new List<BombardmentRepeat>();

	public static Dictionary<string, float> triggerVariables;

	public static bool destroyOffscreenPlayers = false;
}
