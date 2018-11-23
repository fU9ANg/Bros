// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventController : MonoBehaviour
{
	private void Awake()
	{
		WorldEventController.eventCounter = 1f;
		this.events = new List<WorldMapEvent>();
		for (int i = 0; i < this.testEvents.Count; i++)
		{
			if (i >= WorldEventController.completedEventCount)
			{
				this.events.Add(this.testEvents[i]);
			}
		}
		WorldEventController.instance = this;
	}

	private void Start()
	{
	}

	public static void AddCurrentEvent(WorldMapEvent e)
	{
		WorldEventController.currentEvents.Add(e);
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Added current Event ",
			e.eventType,
			"just added events  ",
			WorldEventController.currentEvents.Count
		}));
	}

	protected bool ProcessEvent(WorldMapEvent e)
	{
		UnityEngine.Debug.Log("Process Event " + e.eventType);
		if (e.eventType == WorldMapEventType.RunTerritories)
		{
			WorldMapTerritoriesController.RunTerritories();
			return true;
		}
		if (e.eventType == WorldMapEventType.GainMoney)
		{
			Helicopter3D.ShowText("$" + e.money);
			return true;
		}
		if (e.eventType == WorldMapEventType.Dialogue)
		{
			WorldMapCutsceneController.DeactivateWorld();
			return true;
		}
		switch (e.eventType)
		{
		case WorldMapEventType.Dialogue:
			if (e.territory != null)
			{
				WorldMapController.CameraPanTo(e.territory.transform, (e.zoomLevel <= 1f) ? 1f : e.zoomLevel, WorldEventController.eventCounter);
			}
			break;
		case WorldMapEventType.NewMission:
			if (e.territory != null)
			{
				UnityEngine.Debug.Log("New mission should spawn at " + e.territory.name);
				e.territory.BecomeTerroristBase();
				UnityEngine.Debug.Log("CameraPanTo " + WorldEventController.eventCounter);
				WorldMapController.CameraPanTo(e.territory.icon.transform, 2f, WorldEventController.eventCounter);
			}
			break;
		case WorldMapEventType.NewAirBase:
			if (e.territory != null)
			{
				UnityEngine.Debug.LogError("New Air Base should spawn at " + e.territory.name);
			}
			break;
		case WorldMapEventType.ReenforceTerritory:
			if (e.otherTerritory == null)
			{
				e.territory.AddTerrorVisual(1);
			}
			else
			{
				WorldMapEffectsController.CreateTerroristLine(e.otherTerritory, e.territory);
			}
			break;
		case WorldMapEventType.Liberate:
			UnityEngine.Debug.Log("Liberate Event ");
			if (!(e.territory != null) || e.territory.HasHospital())
			{
			}
			break;
		}
		return true;
	}

	public static void AddDelay(float time)
	{
		WorldEventController.eventCounter += time;
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Add Delay ! ",
			WorldEventController.eventCounter,
			"  currentEvents.Count ",
			WorldEventController.currentEvents.Count
		}));
	}

	private void Update()
	{
		if (WorldEventController.currentEvents.Count > 0)
		{
			WorldEventController.eventCounter -= Time.deltaTime;
			if (WorldEventController.eventCounter <= 0f && WorldMapController.IsNotBusy())
			{
				WorldEventController.eventCounter = WorldEventController.currentEvents[0].delay;
				this.ProcessEvent(WorldEventController.currentEvents[0]);
				WorldEventController.currentEvents.RemoveAt(0);
			}
		}
		else if (this.events.Count > 0)
		{
			this.counter -= Time.deltaTime;
			if (this.counter <= 0f && WorldMapController.IsNotBusy())
			{
				this.counter = this.delay;
				UnityEngine.Debug.Log("Set Counter2  " + this.counter);
				if (this.ProcessEvent(this.events[0]))
				{
					this.events.RemoveAt(0);
					WorldEventController.completedEventCount++;
				}
			}
		}
	}

	protected List<WorldMapEvent> events;

	public List<WorldMapEvent> testEvents;

	public List<WorldMapEvent> firstTurnEvents;

	protected static List<WorldMapEvent> currentEvents = new List<WorldMapEvent>();

	protected float counter = 1f;

	protected static float eventCounter = 5f;

	protected float delay = 1f;

	protected static int completedEventCount = 0;

	protected static WorldEventController instance;
}
