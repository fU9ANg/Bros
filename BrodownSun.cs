// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BrodownSun : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Debug.Log("Go GameMode " + GameModeController.GameMode);
		if (GameModeController.GameMode != GameMode.BroDown)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		this.sun.Translate(0f, -2f * Time.deltaTime, 0f);
	}

	public Transform sun;
}
