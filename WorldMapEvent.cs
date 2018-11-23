// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class WorldMapEvent
{
	public WorldMapEvent(WorldMapEventType type, int money, float delay)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Make event ",
			money,
			"  type ",
			type
		}));
		if (type == WorldMapEventType.GainMoney)
		{
			this.money = money;
		}
		this.eventType = type;
		this.requireRadar = false;
		this.delay = delay;
	}

	public WorldMapEvent(WorldMapEventType type, WorldTerritory3D targetTerritory, WorldTerritory3D otherTerritory, float delay)
	{
		this.territory = targetTerritory;
		this.otherTerritory = otherTerritory;
		this.eventType = type;
		this.requireRadar = false;
		this.delay = delay;
	}

	public WorldTerritory3D territory;

	public WorldTerritory3D otherTerritory;

	public WorldMapEventType eventType;

	public float zoomLevel = 1f;

	public int delayUntilTurn;

	public bool requireRadar = true;

	public int missionDifficulty;

	public int missionLength = 2;

	public int money = 1000;

	public float delay = 1f;
}
