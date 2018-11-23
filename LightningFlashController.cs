// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class LightningFlashController : MonoBehaviour
{
	private void Awake()
	{
		LightningFlashController.lightningObjects = new List<LightningObject>();
	}

	private void Start()
	{
		if (Map.MapData != null && Map.MapData.theme == LevelTheme.Forest && Map.MapData.weatherType == WeatherType.Night)
		{
			this.lightningOn = true;
		}
	}

	public static void RegisterLighntingObject(LightningObject l)
	{
		LightningFlashController.lightningObjects.Add(l);
	}

	public static void FlashLightning()
	{
		FullScreenFlashEffect.FlashLightning(1f);
		foreach (LightningObject lightningObject in LightningFlashController.lightningObjects)
		{
			lightningObject.Flash();
		}
		UnityEngine.Debug.Log("Flas!!!");
	}

	private void Update()
	{
		if (this.lightningOn)
		{
			this.lightningCounter += Time.deltaTime;
			if (this.lightningCounter > 2f)
			{
				if (UnityEngine.Random.value < 0.2f)
				{
					this.lightningCounter -= 0.4f + UnityEngine.Random.value;
				}
				else
				{
					this.lightningCounter -= 2f + UnityEngine.Random.value * 6f;
				}
				LightningFlashController.FlashLightning();
			}
		}
	}

	protected bool lightningOn;

	protected float lightningCounter;

	protected static List<LightningObject> lightningObjects = new List<LightningObject>();
}
