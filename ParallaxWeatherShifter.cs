// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ParallaxWeatherShifter : MonoBehaviour
{
	private void Start()
	{
		WeatherController.RegisterWeatherAffectedParallax(this);
		if (Map.isEditing)
		{
			WeatherType currentWeather = WeatherController.GetCurrentWeather();
			if (currentWeather != WeatherType.Day)
			{
				this.TransitionToWeather(currentWeather, 0.3f);
			}
		}
	}

	public void TransitionToWeather(WeatherType type, float duration)
	{
		if (this.currentWeatherType != type)
		{
			this.transitionDuration = duration;
			if (this.transitioning)
			{
				this.FinishTransition();
			}
			switch (type)
			{
			case WeatherType.Day:
				if (this.normalMaterial == null)
				{
					return;
				}
				this.transitionToMaterial = this.normalMaterial;
				goto IL_EE;
			case WeatherType.Night:
				if (this.nightMaterial == null)
				{
					return;
				}
				this.transitionToMaterial = this.nightMaterial;
				goto IL_EE;
			case WeatherType.Burning:
				if (this.burningMaterial == null)
				{
					return;
				}
				this.transitionToMaterial = this.burningMaterial;
				goto IL_EE;
			case WeatherType.Overcast:
				if (this.overcastMaterial == null)
				{
					this.transitionToMaterial = this.normalMaterial;
				}
				else
				{
					this.transitionToMaterial = this.overcastMaterial;
				}
				goto IL_EE;
			}
			UnityEngine.Debug.LogError("Weather Type Not Implemented");
			return;
			IL_EE:
			this.transitioning = true;
			this.transitionCounter = 0f;
			this.transitionToType = type;
			Material sharedMaterial = base.GetComponent<Renderer>().sharedMaterial;
			base.GetComponent<Renderer>().material = this.transitionMaterial;
			base.GetComponent<Renderer>().material.SetTexture("_MainTex", sharedMaterial.mainTexture);
			base.GetComponent<Renderer>().material.SetTexture("_BlendTex", this.transitionToMaterial.mainTexture);
			base.GetComponent<Renderer>().material.SetFloat("_BlendAmount", 0f);
		}
	}

	protected void FinishTransition()
	{
		this.transitioning = false;
		base.GetComponent<Renderer>().sharedMaterial = this.transitionToMaterial;
	}

	private void Update()
	{
		if (this.transitioning)
		{
			this.transitionCounter += Time.deltaTime;
			float num = this.transitionCounter / this.transitionDuration;
			if (num >= 1f)
			{
				this.FinishTransition();
			}
			else
			{
				base.GetComponent<Renderer>().material.SetFloat("_BlendAmount", num);
			}
		}
	}

	public Material normalMaterial;

	public Material nightMaterial;

	public Material burningMaterial;

	public Material overcastMaterial;

	public Material transitionMaterial;

	protected bool transitioning;

	protected float transitionCounter;

	protected WeatherType currentWeatherType;

	protected WeatherType transitionToType;

	protected Material transitionToMaterial;

	protected float transitionDuration = 1.3f;
}
