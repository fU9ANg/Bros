// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelEventAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (LevelEventActionInfo)value;
		}
	}

	public override void Start()
	{
		this.state = TriggerActionState.Done;
		switch (this.info.levelActionType)
		{
		case LevelEventActionType.LevelEndSuccess:
			GameModeController.LevelFinished = false;
			GameModeController.LevelFinish(LevelResult.Success);
			break;
		case LevelEventActionType.LevelEndFail:
			GameModeController.LevelFinished = false;
			GameModeController.LevelFinish(LevelResult.ForcedFail);
			break;
		case LevelEventActionType.Cutscene:
			CutsceneController.cutsceneToPlay = this.info.cutsceneNumber;
			if (this.info.cutsceneNumber == 0)
			{
				CutsceneController.LoadCutScene("0 PunchingFace");
			}
			else if (this.info.cutsceneNumber == 1)
			{
				CutsceneController.LoadCutScene("1 Abdominal Abscondence");
			}
			else if (this.info.cutsceneNumber == 2)
			{
				CutsceneController.LoadCutScene("2 Stonebanks");
			}
			else if (this.info.cutsceneNumber == 3)
			{
				CutsceneController.LoadCutScene("3 Stonebanks");
			}
			else if (this.info.cutsceneNumber == 4)
			{
				CutsceneController.LoadCutScene("4 Final Cinematic 1");
			}
			else if (this.info.cutsceneNumber == 5)
			{
				CutsceneController.LoadCutScene("5 Final Cinematic 2");
			}
			else if (this.info.cutsceneNumber == 6)
			{
				CutsceneController.LoadCutScene("6 Final Cinematic Heli Escape");
			}
			else
			{
				UnityEngine.Debug.Log("Undefined cutscene");
			}
			break;
		case LevelEventActionType.SwitchToBossMusic:
			Sound.GetInstance().PlayBossFightMusic();
			break;
		case LevelEventActionType.ActivateTrigger:
			TriggerManager.ActivateTrigger(this.info.triggerName);
			break;
		case LevelEventActionType.DeactivateTrigger:
			TriggerManager.DeactivateTrigger(this.info.triggerName);
			break;
		case LevelEventActionType.GoToLevelSilent:
			LevelSelectionController.CurrentLevelNum = this.info.cutsceneNumber;
			Fader.nextScene = LevelSelectionController.CampaignScene;
			Fader.FadeSolid(2f, true);
			break;
		case LevelEventActionType.LevelEndSuccessSilent:
			GameModeController.LevelFinished = false;
			GameModeController.Instance.switchSilently = true;
			GameModeController.LevelFinish(LevelResult.Success);
			GameModeController.MakeFinishInstant();
			break;
		case LevelEventActionType.CallHelicopter:
			Networking.RPC<Vector2, float>(PID.TargetAll, new RpcSignature<Vector2, float>(Map.newestHelicopter.Enter), Map.GetBlockCenter(this.info.pos), 0f, false);
			break;
		case LevelEventActionType.TimeSlowdown:
			TimeController.StopTime(this.info.value2, this.info.value1, 1f, true, false);
			break;
		}
	}

	public override void Update()
	{
	}

	protected LevelEventActionInfo info;
}
