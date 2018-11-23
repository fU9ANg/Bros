// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class KickPlayerMenu : Menu
{
	public static KickPlayerMenu Instance
	{
		get
		{
			if (KickPlayerMenu.instance == null)
			{
				KickPlayerMenu.instance = (UnityEngine.Object.FindObjectsOfTypeAll(typeof(KickPlayerMenu))[0] as KickPlayerMenu);
			}
			return KickPlayerMenu.instance;
		}
	}

	protected override void Awake()
	{
		this.template = this.itemNames[0];
		base.Awake();
	}

	protected override void SetupItems()
	{
		base.SetupItems();
		List<MenuBarItem> list = new List<MenuBarItem>(this.itemNames);
		for (int i = 0; i < 12; i++)
		{
			if (list[0].name != "BACK")
			{
				list.RemoveAt(0);
			}
		}
		MonoBehaviour.print(list.Count);
		foreach (PID pid in Connect.playerIDList)
		{
			if (!pid.IsMine)
			{
				MenuBarItem item = new MenuBarItem(this.template);
				this.template.name = pid.PlayerName;
				this.template.storage = pid;
				list.Insert(0, item);
			}
		}
		this.itemNames = list.ToArray();
	}

	private void ConfirmKickPlayer()
	{
		PID pid = this.itemNames[base.Index].storage as PID;
		MonoBehaviour.print("ConfirmKickPlayer " + pid);
		base.gameObject.SetActive(false);
		ConfirmKick.Open(pid, this.controlledByControllerID);
	}

	private void BackToPauseMenu()
	{
		base.gameObject.SetActive(false);
		PauseMenu.instance.gameObject.SetActive(true);
		KickPlayerMenu.Instance.controlledByControllerID = -1;
	}

	public static void OpenMenu(int ControlledByControllerID)
	{
		KickPlayerMenu.Instance.gameObject.SetActive(true);
		KickPlayerMenu.Instance.controlledByControllerID = ControlledByControllerID;
		KickPlayerMenu.Instance.InstantiateItems();
	}

	private MenuBarItem template;

	private static KickPlayerMenu instance;
}
