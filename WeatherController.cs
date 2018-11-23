// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
	private void Awake()
	{
		WeatherController.instance = this;
		WeatherController.parallaxAffected = new List<ParallaxWeatherShifter>();
		WeatherController.onlyAtNightObjects = new List<SwitchOnAtNight>();
		WeatherController.currentWeatherType = WeatherType.None;
		WeatherController.currentRainType = RainType.None;
		WeatherController.rainTransitionCounter = 0f;
		this.rainEmitters = new ParticleEmitter[2];
		this.rainEmitters[0] = this.rainEmitterWhite;
		this.rainEmitters[1] = this.rainEmitterBlue;
	}

	private void Start()
	{
	}

	public static WeatherType GetCurrentWeather()
	{
		return WeatherController.currentWeatherType;
	}

	protected void CheckInitialWeather()
	{
		UnityEngine.Debug.Log("CHECK INITIAL WEATHER " + (Map.MapData != null));
		if (Map.MapData != null)
		{
			UnityEngine.Debug.Log("CHECK INITIAL WEATHER " + Map.MapData.weatherType);
			switch (Map.MapData.weatherType)
			{
			case WeatherType.None:
			case WeatherType.NoChange:
				if (Map.MapData.theme == LevelTheme.City)
				{
					WeatherController.SwitchWeather(WeatherType.Night, true);
					WeatherController.SwitchWeather(RainType.Raining, true);
				}
				if (Map.MapData.theme == LevelTheme.BurningJungle)
				{
					UnityEngine.Debug.Log(" Burning Jungle Change !");
					WeatherController.SwitchWeather(WeatherType.Burning, true);
				}
				if (Map.MapData.theme == LevelTheme.Forest)
				{
					UnityEngine.Debug.Log("FOREST THEME ");
					WeatherController.currentWeatherType = WeatherType.Night;
					WeatherController.SwitchWeather(WeatherType.Day, true);
				}
				goto IL_18A;
			case WeatherType.Day:
				UnityEngine.Debug.Log("CHECK DAY " + Map.MapData.theme);
				if (Map.MapData.theme == LevelTheme.Forest)
				{
					UnityEngine.Debug.Log("FOREST THEME ");
					WeatherController.currentWeatherType = WeatherType.Night;
					WeatherController.SwitchWeather(WeatherType.Day, true);
				}
				goto IL_18A;
			case WeatherType.Night:
				WeatherController.SwitchWeather(WeatherType.Night, true);
				if (Map.MapData.theme == LevelTheme.City)
				{
					WeatherController.SwitchWeather(RainType.Raining, true);
				}
				goto IL_18A;
			case WeatherType.Burning:
				UnityEngine.Debug.Log(" Burning Jungle Change !");
				WeatherController.currentWeatherType = WeatherType.Night;
				WeatherController.SwitchWeather(WeatherType.Burning, true);
				goto IL_18A;
			case WeatherType.Overcast:
				WeatherController.SwitchWeather(WeatherType.Overcast, true);
				goto IL_18A;
			}
			UnityEngine.Debug.LogError("Not Implemented Weather Type!");
			IL_18A:;
		}
		else
		{
			WeatherType weatherType = MissionScreenController.GetWeatherType();
			if (weatherType != WeatherType.None)
			{
				WeatherController.SwitchWeather(weatherType, true);
			}
			RainType rainType = MissionScreenController.GetRainType();
			if (rainType != RainType.None)
			{
				WeatherController.SwitchWeather(rainType, true);
				ParticleAnimator component = this.rainEmitterWhite.GetComponent<ParticleAnimator>();
				ParticleAnimator component2 = this.rainEmitterBlue.GetComponent<ParticleAnimator>();
				ParticleAnimator component3 = this.rainEmitterDark.GetComponent<ParticleAnimator>();
				ParticleAnimator particleAnimator = component;
				Vector3 vector = new Vector3(-150f, -600f, 0f);
				component2.force = vector;
				vector = vector;
				component3.force = vector;
				particleAnimator.force = vector;
				ParticleEmitter particleEmitter = this.rainEmitterWhite;
				vector = new Vector3(-50f, -220f, 0f);
				this.rainEmitterDark.worldVelocity = vector;
				vector = vector;
				this.rainEmitterBlue.worldVelocity = vector;
				particleEmitter.worldVelocity = vector;
			}
		}
	}

	public static void RegisterWeatherAffectedParallax(ParallaxWeatherShifter parallax)
	{
		WeatherController.parallaxAffected.Add(parallax);
	}

	public static void RegisterSwitchOnAtNight(SwitchOnAtNight s)
	{
		WeatherController.onlyAtNightObjects.Add(s);
	}

	public static void SwitchWeather(WeatherType newWeatherType, bool instant)
	{
		UnityEngine.Debug.Log("switch WEATHER " + newWeatherType);
		if (instant || (newWeatherType != WeatherController.currentWeatherType && newWeatherType != WeatherType.NoChange))
		{
			WeatherController.instance.transitioning = true;
			WeatherController.instance.transitionCounter = 0f;
			WeatherController.currentWeatherType = newWeatherType;
			WeatherController.instance.oldLightingIntensity = WeatherController.instance.currentLightingIntensity;
			foreach (ParallaxWeatherShifter parallaxWeatherShifter in WeatherController.parallaxAffected)
			{
				if (parallaxWeatherShifter != null)
				{
					parallaxWeatherShifter.TransitionToWeather(newWeatherType, (!instant) ? 0.8f : 0.001f);
				}
			}
			switch (newWeatherType)
			{
			case WeatherType.Day:
			case WeatherType.Overcast:
				SunController.Appear(instant);
				WeatherController.instance.rainEmitters = new ParticleEmitter[2];
				WeatherController.instance.rainEmitters[0] = WeatherController.instance.rainEmitterWhite;
				WeatherController.instance.rainEmitters[1] = WeatherController.instance.rainEmitterBlue;
				WeatherController.instance.rainEmitterDark.emit = false;
				WeatherController.instance.targetLightingIntensity = 0f;
				goto IL_328;
			case WeatherType.Night:
				SunController.GoAway(instant);
				WeatherController.instance.rainEmitters = new ParticleEmitter[2];
				WeatherController.instance.rainEmitters[0] = WeatherController.instance.rainEmitterWhite;
				WeatherController.instance.rainEmitters[1] = WeatherController.instance.rainEmitterDark;
				WeatherController.instance.rainEmitterBlue.emit = false;
				if (Map.MapData != null && Map.MapData.theme == LevelTheme.City)
				{
					LightingCamera.SetBackgroundColor(new Color(0.56078434f, 0.7137255f, 0.831372559f, 1f));
					WeatherController.instance.targetLightingIntensity = 1f;
					WeatherController.rainEmission = 650f;
				}
				else if (Map.MapData != null && Map.MapData.theme == LevelTheme.Forest)
				{
					WeatherController.instance.targetLightingIntensity = 0f;
					WeatherController.instance.rainEmitters[0] = WeatherController.instance.rainEmitterStormFront;
					WeatherController.instance.rainEmitters[1] = WeatherController.instance.rainEmitterStormDark;
					WeatherController.instance.rainEmitterDark.emit = false;
					WeatherController.instance.rainEmitterWhite.emit = false;
					WeatherController.instance.rainEmitterBlue.emit = false;
					WeatherController.rainEmission = 400f;
				}
				else
				{
					LightingCamera.SetBackgroundColor(new Color(0.2509804f, 0.270588249f, 0.6117647f, 1f));
					WeatherController.instance.targetLightingIntensity = 1f;
					WeatherController.rainEmission = 650f;
				}
				goto IL_328;
			case WeatherType.Burning:
				WeatherController.instance.rainEmitters = new ParticleEmitter[1];
				WeatherController.instance.rainEmitters[0] = WeatherController.instance.rainEmitterWhite;
				WeatherController.instance.rainEmitterDark.emit = false;
				WeatherController.instance.rainEmitterBlue.emit = false;
				WeatherController.instance.targetLightingIntensity = 0f;
				goto IL_328;
			}
			UnityEngine.Debug.LogError("Not Implemented");
			IL_328:
			WeatherController.TransitionRain(WeatherController.currentRainType);
			WeatherController.rainTransitionCounter = ((!instant) ? 0f : 1.99f);
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Color shift switch WEATHER ",
				newWeatherType,
				" transitionDuration ",
				0.8f,
				" instance ? ",
				instant
			}));
			WeatherController.instance.colorShiftController.SwitchWeather(newWeatherType, (!instant) ? 0.8f : 0.001f, instant);
		}
	}

	public static void SwitchWeather(RainType newRainType, bool instant)
	{
		if (newRainType != WeatherController.currentRainType && newRainType != RainType.NoChange)
		{
			WeatherController.TransitionRain(newRainType);
			WeatherController.currentRainType = newRainType;
			WeatherController.rainTransitionCounter = ((!instant) ? 0f : 1f);
		}
	}

	protected static void TransitionRain(RainType newRainType)
	{
		WeatherController.oldRainM = WeatherController.currentRainM;
		switch (newRainType)
		{
		case RainType.None:
		case RainType.Clear:
			WeatherController.targetRainM = 0f;
			goto IL_67;
		case RainType.Raining:
			WeatherController.targetRainM = 1f;
			goto IL_67;
		case RainType.Drizzle:
			WeatherController.targetRainM = 0.15f;
			goto IL_67;
		}
		UnityEngine.Debug.LogError("Not Implemented");
		IL_67:
		WeatherController.rainTransitioning = true;
	}

	protected void SwitchOnNightObjects()
	{
		foreach (SwitchOnAtNight switchOnAtNight in WeatherController.onlyAtNightObjects)
		{
			switchOnAtNight.SwitchOn(WeatherController.currentWeatherType);
		}
	}

	private void LateUpdate()
	{
		if (this.firstFrame)
		{
			this.firstFrame = false;
			this.CheckInitialWeather();
		}
	}

	private void Update()
	{
		if (this.transitioning)
		{
			this.transitionCounter += Time.deltaTime;
			float num = Mathf.Clamp01(this.transitionCounter / 0.8f);
			if (num >= 1f)
			{
				this.transitioning = false;
				this.SwitchOnNightObjects();
			}
			this.currentLightingIntensity = this.oldLightingIntensity * (1f - num) + this.targetLightingIntensity * num;
			this.lightingCamera.SetIntensity(this.currentLightingIntensity);
			switch (WeatherController.currentWeatherType)
			{
			case WeatherType.Day:
				goto IL_D0;
			case WeatherType.Night:
				goto IL_D0;
			case WeatherType.Burning:
				goto IL_D0;
			case WeatherType.Overcast:
				goto IL_D0;
			}
			UnityEngine.Debug.LogError("Not Implemented Yet " + WeatherController.currentWeatherType);
		}
		IL_D0:
		if (WeatherController.rainTransitioning)
		{
			WeatherController.rainTransitionCounter += Time.deltaTime * (float)((WeatherController.currentWeatherType != WeatherType.Burning) ? 1 : 4);
			float num2 = Mathf.Clamp01(WeatherController.rainTransitionCounter / 2f);
			WeatherController.currentRainM = WeatherController.oldRainM * (1f - num2) + WeatherController.targetRainM * num2;
			if (num2 < 1f)
			{
				foreach (ParticleEmitter particleEmitter in this.rainEmitters)
				{
					particleEmitter.minEmission = WeatherController.rainEmission * WeatherController.currentRainM;
					particleEmitter.maxEmission = WeatherController.rainEmission * WeatherController.currentRainM;
					if (WeatherController.currentRainM > 0f)
					{
						particleEmitter.emit = true;
					}
				}
			}
			else
			{
				WeatherController.rainTransitioning = false;
				switch (WeatherController.currentRainType)
				{
				case RainType.None:
				case RainType.Clear:
					foreach (ParticleEmitter particleEmitter2 in this.rainEmitters)
					{
						particleEmitter2.emit = false;
					}
					return;
				case RainType.Raining:
					return;
				case RainType.Drizzle:
					return;
				}
				UnityEngine.Debug.LogError("Not Implemented Rain Type ");
			}
		}
	}

	private const float transitionDuration = 0.8f;

	private const float rainTransitionDuration = 2f;

	protected static List<ParallaxWeatherShifter> parallaxAffected;

	protected static WeatherType currentWeatherType;

	protected static List<SwitchOnAtNight> onlyAtNightObjects;

	protected float transitionCounter;

	protected bool transitioning;

	protected float oldLightingIntensity;

	protected float currentLightingIntensity;

	protected float targetLightingIntensity;

	public static RainType currentRainType;

	public ParticleEmitter rainEmitterWhite;

	public ParticleEmitter rainEmitterDark;

	public ParticleEmitter rainEmitterBlue;

	public ParticleEmitter rainEmitterStormFront;

	public ParticleEmitter rainEmitterStormDark;

	protected ParticleEmitter[] rainEmitters;

	private static float rainEmission = 650f;

	protected static float rainTransitionCounter;

	protected static bool rainTransitioning;

	protected static float targetRainM;

	protected static float oldRainM;

	protected static float currentRainM;

	protected bool firstFrame = true;

	public ColorShiftController colorShiftController;

	public LightingCamera lightingCamera;

	private static WeatherController instance;
}
