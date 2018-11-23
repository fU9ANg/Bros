// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TimeController : MonoBehaviour
{
	public static void StopTime(float duration, float toTimeScale, float audioFilterAmount, bool scaleDown, bool alterSound)
	{
		if (TimeController.instance != null)
		{
			TimeController.timeStopTime = duration;
			TimeController.timeStopped = true;
			TimeController.totaltimeStoppedSoFar = 0f;
			if (scaleDown)
			{
				TimeController.targetTimeScale = toTimeScale;
			}
			else
			{
				Time.timeScale = toTimeScale;
			}
			TimeController.waitOneFrame = true;
			TimeController.instance.scaleDown = scaleDown;
			if (alterSound)
			{
				Sound.SetPitchNearInstant(audioFilterAmount);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("No timecontroller instance in scene!");
		}
	}

	private void Start()
	{
		TimeController.timeStopped = false;
		TimeController.instance = this;
		Time.timeScale = 1f;
	}

	private void Update()
	{
		if (TimeController.timeStopped)
		{
			if (TimeController.waitOneFrame)
			{
				TimeController.waitOneFrame = false;
				return;
			}
			if (Time.timeScale > 0f)
			{
				if (TimeController.timeStopTime > 0f)
				{
					TimeController.timeStopTime -= Time.deltaTime / Time.timeScale;
					TimeController.totaltimeStoppedSoFar += Time.deltaTime / Time.timeScale;
					if (this.scaleDown)
					{
						Time.timeScale = Mathf.Lerp(1f, TimeController.targetTimeScale, TimeController.totaltimeStoppedSoFar * 4f);
						if (Time.timeScale < TimeController.targetTimeScale)
						{
							Time.timeScale = TimeController.targetTimeScale;
							this.scaleDown = false;
						}
					}
					if (TimeController.timeStopTime <= 0f)
					{
						Sound.SetPitchNearInstant(1f);
					}
				}
			}
			else
			{
				Time.timeScale = 0.001f;
			}
			if (TimeController.timeStopTime <= 0f)
			{
				Time.timeScale += Time.deltaTime / Time.timeScale * 5f;
			}
			if (Time.timeScale >= 1f)
			{
				Time.timeScale = 1f;
				TimeController.timeStopped = false;
			}
		}
	}

	protected static bool timeStopped;

	protected static float timeStopTime;

	protected static float totaltimeStoppedSoFar;

	private static TimeController instance;

	private static bool waitOneFrame;

	private bool scaleDown;

	private static float targetTimeScale;
}
