// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class IntermissionScreen : MonoBehaviour
{
	protected static void RegisterIntermissionScreen(IntermissionScreen screen)
	{
		if (IntermissionScreen.ActiveIntermissionScreens == null)
		{
			IntermissionScreen.ActiveIntermissionScreens = new List<IntermissionScreen>();
		}
		if (IntermissionScreen.ActiveIntermissionScreens.Contains(screen))
		{
			UnityEngine.Debug.LogError("Tried to register intermission screen twice.");
			return;
		}
		IntermissionScreen.ActiveIntermissionScreens.Add(screen);
	}

	protected virtual void Awake()
	{
		IntermissionScreen.RegisterIntermissionScreen(this);
		this.intermissionCamera = base.GetComponentInChildren<Camera>();
	}

	public static IList<IntermissionScreen> ActiveIntermissionScreens;

	protected Camera intermissionCamera;

	public float minX = -140f;

	public float maxX = 140f;

	public float minY = -90f;

	public float maxY = 90f;
}
