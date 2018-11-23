// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
	private void Awake()
	{
		Cutscene.allCutscenesLoaded.Add(this);
	}

	private void Start()
	{
		MonoBehaviour.print("Resetting timeScale");
		Time.timeScale = 1f;
	}

	public static Cutscene GetMostRecentlyLoadedCutscene()
	{
		for (int i = Cutscene.allCutscenesLoaded.Count - 1; i >= 0; i--)
		{
			if (Cutscene.allCutscenesLoaded[i] != null)
			{
				return Cutscene.allCutscenesLoaded[i];
			}
			Cutscene.allCutscenesLoaded.RemoveAt(i);
		}
		return null;
	}

	public bool isFinished;

	public static List<Cutscene> allCutscenesLoaded = new List<Cutscene>();
}
