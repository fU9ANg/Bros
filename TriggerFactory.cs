// dnSpy decompiler from Assembly-CSharp.dll
using System;

public static class TriggerFactory
{
	public static Trigger CreateTrigger(TriggerInfo info, NID nid)
	{
		Trigger trigger = null;
		trigger = new Trigger();
		trigger.info = info;
		Registry.RegisterObject(ref nid, trigger);
		if (trigger.type == TriggerType.CheckTerrain)
		{
			trigger.terrainValues = new bool[trigger.upperRight.collumn - trigger.bottomLeft.collumn + 1, trigger.upperRight.row - trigger.bottomLeft.row + 1];
			for (int i = 0; i <= trigger.terrainValues.GetUpperBound(0); i++)
			{
				for (int j = 0; j <= trigger.terrainValues.GetUpperBound(1); j++)
				{
					trigger.terrainValues[i, j] = Map.IsBlockSolid(i + trigger.bottomLeft.collumn, j + trigger.bottomLeft.row);
				}
			}
		}
		foreach (TriggerActionInfo info2 in info.actions)
		{
			trigger.actions.Add(TriggerFactory.CreateAction(info2));
		}
		trigger.SetEvaluationType();
		trigger.Setup();
		return trigger;
	}

	public static TriggerAction CreateAction(TriggerActionInfo info)
	{
		TriggerAction triggerAction;
		switch (info.type)
		{
		case TriggerActionType.CameraMove:
			triggerAction = new CameraMoveAction();
			break;
		case TriggerActionType.Explosion:
			triggerAction = new ExplosionAction();
			break;
		case TriggerActionType.System:
			triggerAction = new SystemAction();
			break;
		case TriggerActionType.Collapse:
			triggerAction = new CollapseAction();
			break;
		case TriggerActionType.Burn:
			triggerAction = new BurnAction();
			break;
		case TriggerActionType.SpawnResource:
			triggerAction = new SpawnResourceAction();
			break;
		case TriggerActionType.SpawnMooks:
			triggerAction = new SpawnMooksAction();
			break;
		case TriggerActionType.LevelEvent:
			triggerAction = new LevelEventAction();
			break;
		case TriggerActionType.Bombardment:
			triggerAction = new BombardmentAction();
			break;
		case TriggerActionType.SpawnBlock:
			triggerAction = new SpawnBlockAction();
			break;
		case TriggerActionType.Variable:
			triggerAction = new VariableAction();
			break;
		case TriggerActionType.Weather:
			triggerAction = new WeatherAction();
			break;
		case TriggerActionType.ExecuteFunction:
			triggerAction = new ExecuteFunctionAction();
			break;
		case TriggerActionType.Character:
			triggerAction = new CharacterAction();
			break;
		default:
			throw new Exception("Could not instantiate action for actioninfo of type " + info.type + " \n god dammit ruan");
		}
		triggerAction.Info = info;
		triggerAction.timeOffsetLeft = info.timeOffset;
		return triggerAction;
	}
}
