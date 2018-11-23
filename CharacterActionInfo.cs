// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterActionInfo : TriggerActionInfo
{
	public override void ShowGUI(LevelEditorGUI gui)
	{
		this.commandingBros = GUILayout.Toggle(this.commandingBros, "Command Bros", new GUILayoutOption[0]);
		if (!this.commandingBros)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Character Tag:", new GUILayoutOption[0]);
			this.characterTag = GUILayout.TextField(this.characterTag, new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
		}
		for (int i = 0; i < this.commands.Count; i++)
		{
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label((i + 1).ToString() + ":" + this.commands[i].ToString(), new GUILayoutOption[0]);
			if (this.editingCommand == this.commands[i])
			{
				GUILayout.Label("EDITING", new GUILayoutOption[0]);
			}
			else if (GUILayout.Button("Edit", new GUILayoutOption[0]))
			{
				this.editingCommand = this.commands[i];
			}
			if (GUILayout.Button("▲", new GUILayoutOption[0]) && i - 1 >= 0)
			{
				CharacterCommand value = this.commands[i];
				this.commands[i] = this.commands[i - 1];
				this.commands[i - 1] = value;
			}
			if (GUILayout.Button("▼", new GUILayoutOption[0]) && i + 1 < this.commands.Count)
			{
				CharacterCommand value2 = this.commands[i];
				this.commands[i] = this.commands[i + 1];
				this.commands[i + 1] = value2;
			}
			if (GUILayout.Button("Del", new GUILayoutOption[0]))
			{
				this.commands.RemoveAt(i);
			}
			GUILayout.EndHorizontal();
		}
		if (this.editingCommand != null)
		{
			GUILayout.Label("Editing Command " + (this.commands.IndexOf(this.editingCommand) + 1), new GUILayoutOption[0]);
			GUILayout.Label("Type:", new GUILayoutOption[0]);
			if (this.commandingBros)
			{
				this.editingCommand.type = CharacterCommandType.Move;
			}
			else
			{
				this.editingCommand.type = (CharacterCommandType)((int)LevelEditorGUI.SelectList(gui.characterCommandTypes, this.editingCommand.type, gui.skin.label, gui.skin.button));
			}
			switch (this.editingCommand.type)
			{
			case CharacterCommandType.Move:
				if (this.editingCommand.target == null)
				{
					this.editingCommand.target = new GridPoint();
				}
				if (GUILayout.Button("Set Target (currently " + this.editingCommand.target.ToString() + ")", new GUILayoutOption[0]))
				{
					gui.settingWaypoint = true;
					gui.waypointToSet = this.editingCommand.target;
				}
				gui.MarkTargetPoint(this.editingCommand.target);
				break;
			case CharacterCommandType.AICommand:
				if (GUILayout.Button("Set Target (currently " + this.editingCommand.target.ToString() + ")", new GUILayoutOption[0]))
				{
					gui.settingWaypoint = true;
					gui.waypointToSet = this.editingCommand.target;
				}
				gui.MarkTargetPoint(this.editingCommand.target);
				this.editingCommand.aiCommandType = (EnemyActionType)((int)LevelEditorGUI.SelectList(gui.enemyActionTypes, this.editingCommand.aiCommandType, gui.skin.label, gui.skin.button));
				break;
			}
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.Label("Max Time: ", new GUILayoutOption[0]);
			float.TryParse(GUILayout.TextField(this.editingCommand.maxTime.ToString("#.00"), new GUILayoutOption[0]), out this.editingCommand.maxTime);
			GUILayout.EndHorizontal();
		}
		if (GUILayout.Button("Add Command", new GUILayoutOption[0]))
		{
			CharacterCommand item = new CharacterCommand();
			this.editingCommand = item;
			this.commands.Add(item);
		}
	}

	public string characterTag = string.Empty;

	public List<CharacterCommand> commands = new List<CharacterCommand>();

	private CharacterCommand editingCommand;

	public bool commandingBros;
}
