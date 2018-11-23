// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class LevelStats
{
	public void Add(LevelStats stats)
	{
		this.destruction += stats.destruction;
		this.mooksKilled += stats.mooksKilled;
		this.rescues += stats.rescues;
		this.mooksHeardSound += stats.mooksHeardSound;
		this.mooksHalfAlerted += stats.mooksHalfAlerted;
		this.mooksFullyAlerted += stats.mooksFullyAlerted;
		this.mooksKnifed += stats.mooksKnifed;
		this.mooksKilledUnawares += stats.mooksKilledUnawares;
		this.totalTime += stats.totalTime;
		this.deathList.AddRange(stats.deathList);
		this.totalBrotality += stats.totalBrotality;
		this.totalBrotalityPenalty += stats.totalBrotalityPenalty;
	}

	public int destruction;

	public int mooksKilled;

	public int rescues;

	public int mooksHeardSound;

	public int mooksHalfAlerted;

	public int mooksFullyAlerted;

	public int mooksKnifed;

	public int mooksKilledUnawares;

	public int totalCages;

	public int mooksAtStart;

	public List<DeathObject> deathList = new List<DeathObject>();

	public float totalTime;

	public float totalBrotality;

	public float totalBrotalityPenalty;
}
