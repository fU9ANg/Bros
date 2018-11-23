// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
	private void Start()
	{
		this.startTime = Time.time;
		LevelTimer.instance = this;
	}

	public static float GetLevelTime()
	{
		return Time.time - LevelTimer.instance.startTime;
	}

	private void Update()
	{
		if (!Map.IsFinished() && HeroController.GetPlayersAliveCount() > 0)
		{
			float num = Time.time - this.startTime;
			float num2 = (float)((int)(num / 60f));
			float num3 = (float)((int)(num - num2 * 60f));
			float num4 = (float)((int)((num - num2 * 60f - num3) * 100f));
			this.textMesh.text = string.Concat(new string[]
			{
				string.Empty,
				num2.ToString("00"),
				":",
				num3.ToString("00"),
				".",
				num4.ToString("00")
			});
		}
	}

	protected float startTime;

	public TextMesh textMesh;

	public static LevelTimer instance;
}
