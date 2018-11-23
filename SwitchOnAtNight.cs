// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SwitchOnAtNight : MonoBehaviour
{
	private void Start()
	{
		WeatherController.RegisterSwitchOnAtNight(this);
		base.gameObject.SetActive(false);
	}

	public void SwitchOn(WeatherType weatherType)
	{
		if (weatherType == WeatherType.Night)
		{
			base.gameObject.SetActive(true);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}
}
