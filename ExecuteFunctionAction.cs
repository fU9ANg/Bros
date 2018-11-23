// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteFunctionAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (ExecuteFunctionActionInfo)value;
		}
	}

	public override void Start()
	{
		List<GameObject> list;
		if (this.info.callFunctionOnBros)
		{
			list = new List<GameObject>();
			for (int i = 0; i < 4; i++)
			{
				if (HeroController.players[i] != null && HeroController.IsPlaying(i) && HeroController.players[i].character != null)
				{
					list.Add(HeroController.players[i].character.gameObject);
				}
			}
		}
		else
		{
			list = Map.GetDoodadsByTag(this.info.entity);
		}
		foreach (GameObject gameObject in list)
		{
			if (this.info.useScriptedVariable)
			{
				gameObject.SendMessage(this.info.function, TriggerManager.GetVariableValue(this.info.variableName));
			}
			else if (!string.IsNullOrEmpty(this.info.parameter))
			{
				gameObject.SendMessage(this.info.function, this.info.parameter);
			}
			else
			{
				gameObject.SendMessage(this.info.function);
			}
		}
		this.state = TriggerActionState.Done;
	}

	public override void Update()
	{
	}

	private ExecuteFunctionActionInfo info;
}
