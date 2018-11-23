// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldLocation : MonoBehaviour
{
	public virtual void Activate()
	{
		WorldMapController.TransportAriveAt(this);
	}

	protected virtual void Start()
	{
		if (this.territory == null)
		{
			this.territory = base.GetComponent<WorldMapTerritory>();
		}
	}

	protected virtual void Update()
	{
	}

	public virtual void SetMissionText()
	{
		if (this.isBase)
		{
			WorldMapController.SetMissionDetailsText(((!this.isForwardBase) ? "HOME BASE" : "FORWARD BASE") + "\n" + base.name);
		}
		else
		{
			WorldMapController.SetMissionDetailsText(string.Concat(new object[]
			{
				"TERRORIST BASE\n",
				base.name,
				"\nDIFFICULTY: ",
				this.missionDifficulty,
				"\nLENGTH: ",
				this.missionLength
			}));
		}
	}

	public WorldMapTerritory territory;

	public bool isBase;

	public bool isForwardBase;

	public int missionDifficulty = 1;

	public int missionLength = 1;

	public bool isCompleted;

	public bool hasTerroristAirDefence;
}
