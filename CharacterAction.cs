// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (CharacterActionInfo)value;
		}
	}

	public override void Update()
	{
		this.timeOnCurrentCommand += Mathf.Clamp(Time.deltaTime, 0f, 0.0333334f);
	}

	public override void Start()
	{
		this.state = TriggerActionState.Busy;
		this.characters = new List<Unit>();
		if (this.info.commandingBros)
		{
			for (int i = 0; i < 4; i++)
			{
				if (HeroController.players[i] != null && HeroController.IsPlaying(i) && HeroController.players[i].character != null)
				{
					this.characters.Add(HeroController.players[i].character);
					HeroController.players[i].character.beingControlledByTriggerAction = true;
					HeroController.players[i].character.controllingTriggerAction = this;
				}
			}
		}
		else
		{
			List<GameObject> doodadsByTag = Map.GetDoodadsByTag(this.info.characterTag);
			if (doodadsByTag == null || doodadsByTag.Count == 0)
			{
				if (LevelEditorGUI.levelEditorActive)
				{
					LevelTitle.ShowText("COULD NOT FIND CHARACTER TAGGED AS " + this.info.characterTag, 0f);
				}
				this.state = TriggerActionState.Done;
			}
			else
			{
				for (int j = 0; j < doodadsByTag.Count; j++)
				{
					Unit component = doodadsByTag[j].GetComponent<Unit>();
					if (component != null)
					{
						this.characters.Add(component);
						component.beingControlledByTriggerAction = true;
						component.controllingTriggerAction = this;
						if (component.GetComponent<PolymorphicAI>() != null)
						{
							component.GetComponent<PolymorphicAI>().ClearActionQueue();
						}
					}
				}
			}
		}
		this.currentCommandIndices = new int[this.characters.Count];
		for (int k = 0; k < this.currentCommandIndices.Length; k++)
		{
			this.currentCommandIndices[k] = 0;
		}
		for (int l = 0; l < this.characters.Count; l++)
		{
			if (this.info.commands.Count > 0 && this.info.commands[0].type == CharacterCommandType.AICommand)
			{
				this.AddAICommand(l);
			}
		}
		if (this.info.commands.Count == 0)
		{
			this.state = TriggerActionState.Done;
		}
		UnityEngine.Debug.Log("Start " + this.characters.Count);
	}

	private void AddAICommand(int characterIndex)
	{
		if (this.characters[characterIndex].GetComponent<PolymorphicAI>() != null)
		{
			this.characters[characterIndex].GetComponent<PolymorphicAI>().AddAction(this.GetCurrentCommand(characterIndex).aiCommandType, this.GetCurrentCommand(characterIndex).target, this.GetCurrentCommand(characterIndex).maxTime);
		}
	}

	public CharacterCommand GetCurrentCommand(int characterIndex)
	{
		if (this.currentCommandIndices[characterIndex] >= 0 && this.currentCommandIndices[characterIndex] < this.info.commands.Count)
		{
			return this.info.commands[this.currentCommandIndices[characterIndex]];
		}
		return null;
	}

	internal void CompleteCurrentCommand(Unit character)
	{
		int num = this.characters.IndexOf(character);
		this.currentCommandIndices[num]++;
		if (this.currentCommandIndices[num] >= this.info.commands.Count)
		{
			if (character.GetComponent<PolymorphicAI>() != null)
			{
				character.GetComponent<PolymorphicAI>().Reassess();
			}
			character.beingControlledByTriggerAction = false;
			character.controllingTriggerAction = null;
		}
		else if (this.GetCurrentCommand(num).type == CharacterCommandType.AICommand)
		{
			this.AddAICommand(num);
		}
		bool flag = true;
		for (int i = 0; i < this.characters.Count; i++)
		{
			flag = (flag && this.currentCommandIndices[i] >= this.info.commands.Count);
		}
		if (flag)
		{
			this.state = TriggerActionState.Done;
		}
	}

	internal CharacterCommand GetCurrentCommand(Unit unit)
	{
		if (this.characters.Contains(unit))
		{
			return this.GetCurrentCommand(this.characters.IndexOf(unit));
		}
		UnityEngine.Debug.Log("characters did not contain this char");
		return null;
	}

	private CharacterActionInfo info;

	private List<Unit> characters;

	private float timeOnCurrentCommand;

	private int[] currentCommandIndices;
}
