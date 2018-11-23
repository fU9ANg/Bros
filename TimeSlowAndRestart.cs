// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TimeSlowAndRestart : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			if (Time.timeScale < 1f)
			{
				Time.timeScale = 1f;
				MonoBehaviour.print("------ Resetting Time Scale ------");
			}
			else
			{
				Time.timeScale = 0.1f;
				MonoBehaviour.print("------ Slowing Time ------");
			}
		}
		if (Input.GetKeyDown(KeyCode.F6))
		{
			MonoBehaviour.print("------ reloading level ------");
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
