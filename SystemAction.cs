// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SystemAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (SystemActionInfo)value;
		}
	}

	public override void Update()
	{
	}

	public override void Start()
	{
		switch (this.info.systemType)
		{
		case SystemActionType.InfoBarText:
			InfoBar.Appear(1f, this.info.text, new Color(0.5f, 0.5f, 0.5f, 0.5f), 1f);
			this.state = TriggerActionState.Done;
			break;
		case SystemActionType.HighlightPlayer:
			HeroController.players[this.info.playernum].character.ShowStartBubble();
			this.state = TriggerActionState.Done;
			break;
		case SystemActionType.StartCountdown:
			HeroController.StartCountdown();
			this.state = TriggerActionState.Done;
			break;
		default:
			UnityEngine.Debug.LogError("Unsupported system action type (ask ruan)");
			break;
		}
	}

	private SystemActionInfo info;
}
