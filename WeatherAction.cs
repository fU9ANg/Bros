// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WeatherAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (WeatherActionInfo)value;
		}
	}

	public override void Start()
	{
		this.state = TriggerActionState.Done;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Action Weather Run ",
			this.info.weatherType,
			" ",
			this.info.rainType
		}));
		WeatherController.SwitchWeather(this.info.rainType, false);
		WeatherController.SwitchWeather(this.info.weatherType, false);
	}

	public override void Update()
	{
	}

	protected WeatherActionInfo info;
}
