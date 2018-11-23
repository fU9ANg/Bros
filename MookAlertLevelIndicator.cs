// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookAlertLevelIndicator : MonoBehaviour
{
	private void Start()
	{
		if (GameModeController.IsDeathMatchMode || GameModeController.GameMode == GameMode.BroDown || GameModeController.GameMode == GameMode.ExplosionRun || GameModeController.GameMode == GameMode.Race)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (!GameModeController.IsLevelFinished())
		{
			for (int i = 0; i < this.icons.Length; i++)
			{
				int brotalityLevel = StatisticsController.GetBrotalityLevel();
				if (StatisticsController.GetBrotalityLevel() > i)
				{
					if (brotalityLevel >= 1)
					{
						this.icons[i].SetPosition(((float)(-(float)(brotalityLevel - 1)) / 2f + (float)i) * 12f);
					}
				}
				else
				{
					this.icons[i].SetPosition(((float)(-(float)(brotalityLevel - 1)) / 2f + (float)i) * 12f);
					this.icons[i].Disappear();
				}
			}
		}
		else
		{
			for (int j = 0; j < this.icons.Length; j++)
			{
				this.icons[j].Disappear();
			}
		}
	}

	public Camera uiCamera;

	public MookAlertIcon[] icons;
}
