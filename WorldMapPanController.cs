// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapPanController : MonoBehaviour
{
	private void Awake()
	{
		WorldMapPanController.newMissions = new List<MissionButton>();
		MonoBehaviour.print("WorldMapPanController Start" + UnityEngine.Random.value);
	}

	public static void AddNewMission(MissionButton newMission)
	{
		WorldMapPanController.newMissions.Add(newMission);
		newMission.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (this.delay > 0f)
		{
			this.delay -= Time.deltaTime;
			if (this.delay <= 0f && WorldMapPanController.newMissions.Count > 0)
			{
				this.panToTarget = WorldMapPanController.newMissions[0].transform.position;
			}
		}
	}

	protected static List<MissionButton> newMissions = new List<MissionButton>();

	protected float delay = 1f;

	protected float panToTime = 1f;

	public float panningPause;

	public float panDuration = 2f;

	protected float panStart;

	protected Vector3 panToTarget;
}
