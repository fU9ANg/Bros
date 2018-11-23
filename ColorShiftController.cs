// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ColorShiftController : MonoBehaviour
{
	private void Awake()
	{
		ColorShiftController.instance = this;
	}

	public static void SlowTimeEffect(float time)
	{
		ColorShiftController.instance.flashEffectScript.enabled = false;
		ColorShiftController.slowTimeEffectTime = time;
		ColorShiftController.slowTimeEffectCounter = 0f;
		ColorShiftController.instance.specialEffectScript.LutBlendTexture = ColorShiftController.instance.timeSlowColorMap;
		ColorShiftController.instance.specialEffectScript.enabled = true;
	}

	public void SwitchWeather(WeatherType type, float duration, bool forced)
	{
		if (type != this.weatherType || forced)
		{
			this.oldIntensity = 0f;
			this.weatherType = type;
			this.transitionCounter = 0f;
			this.transitioning = true;
			this.transitionDuration = duration;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Sweitch weather TYuype ",
				type,
				" transitionDuration ",
				this.transitionDuration,
				" oldIntensity ",
				this.oldIntensity
			}));
			switch (type)
			{
			case WeatherType.Day:
				if (Map.MapData != null && Map.MapData.theme == LevelTheme.Forest)
				{
					this.targetIntensity = 1f;
					this.effectScript.LutTexture = this.effectScript.LutBlendTexture;
					this.effectScript.LutBlendTexture = this.weatherChernobylColorMap;
				}
				else
				{
					this.effectScript.LutTexture = this.effectScript.LutBlendTexture;
					this.effectScript.LutBlendTexture = null;
					this.targetIntensity = 1f;
				}
				return;
			case WeatherType.Night:
				if (Map.MapData != null && Map.MapData.theme == LevelTheme.City)
				{
					this.targetIntensity = 1f;
					this.effectScript.LutTexture = this.effectScript.LutBlendTexture;
					this.effectScript.LutBlendTexture = this.weatherCityNightColorMap;
				}
				else if (Map.MapData != null && Map.MapData.theme == LevelTheme.Forest)
				{
					this.targetIntensity = 1f;
					this.effectScript.LutTexture = this.effectScript.LutBlendTexture;
					this.effectScript.LutBlendTexture = this.weatherForestNightColorMap;
				}
				else
				{
					this.targetIntensity = 0f;
				}
				return;
			case WeatherType.Burning:
				this.targetIntensity = 1f;
				this.effectScript.LutTexture = this.effectScript.LutBlendTexture;
				if (Map.MapData != null && Map.MapData.theme == LevelTheme.Forest)
				{
					this.effectScript.LutBlendTexture = this.weatherBurningFactoryColorMap;
				}
				else
				{
					this.effectScript.LutBlendTexture = this.weatherBurningColorMap;
				}
				return;
			case WeatherType.Overcast:
				this.targetIntensity = 1f;
				this.effectScript.LutTexture = this.effectScript.LutBlendTexture;
				this.effectScript.LutBlendTexture = this.weatherRainyColorMap;
				return;
			}
			UnityEngine.Debug.LogError("Weather Type not implemented " + this.weatherType);
		}
	}

	public void SetIntensity(float m)
	{
		if (m > 0f)
		{
			this.effectScript.enabled = true;
			this.effectScript.BlendAmount = m;
		}
		else if (this.effectScript.LutBlendTexture == null)
		{
			this.effectScript.enabled = false;
		}
	}

	public static bool CanFlashHot()
	{
		return ColorShiftController.slowTimeEffectTime <= 0f;
	}

	private void Update()
	{
		if (this.transitioning)
		{
			this.transitionCounter += Time.deltaTime;
			float num = Mathf.Clamp01(this.transitionCounter / this.transitionDuration);
			this.SetIntensity(this.oldIntensity * (1f - num) + this.targetIntensity * num);
			if (num >= 1f)
			{
				this.transitioning = false;
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Stop transitioning ",
					this.oldIntensity,
					"  ",
					this.targetIntensity
				}));
			}
		}
		if (ColorShiftController.slowTimeEffectTime > 0f)
		{
			float deltaTime = Time.deltaTime;
			ColorShiftController.slowTimeEffectTime -= deltaTime;
			ColorShiftController.slowTimeEffectCounter += deltaTime;
			if (ColorShiftController.slowTimeEffectTime <= 0f)
			{
				this.flashEffectScript.enabled = true;
				this.specialEffectScript.enabled = false;
				FullScreenFlashEffect.Clear();
			}
			else
			{
				float blendAmount = Mathf.Clamp01(ColorShiftController.slowTimeEffectCounter * 8f) - (1f - Mathf.Clamp01(ColorShiftController.slowTimeEffectTime * 4f));
				this.specialEffectScript.BlendAmount = blendAmount;
			}
		}
	}

	public AmplifyColorEffect effectScript;

	public Texture2D weatherBurningColorMap;

	public Texture2D weatherBurningFactoryColorMap;

	public Texture2D weatherRainyColorMap;

	public Texture2D weatherChernobylColorMap;

	public Texture2D weatherCityNightColorMap;

	public Texture2D weatherForestNightColorMap;

	public Texture2D timeSlowColorMap;

	public AmplifyColorEffect specialEffectScript;

	protected WeatherType weatherType;

	public FullScreenFlashEffect flashEffectScript;

	protected float transitionDuration = 0.7f;

	protected float transitionCounter;

	protected float targetIntensity;

	protected float oldIntensity;

	protected bool transitioning;

	protected static ColorShiftController instance;

	protected static float slowTimeEffectCounter;

	protected static float slowTimeEffectTime;
}
