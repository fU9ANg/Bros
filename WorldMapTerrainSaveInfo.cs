// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapTerrainSaveInfo
{
	public WorldMapTerrainSaveInfo(bool completed, bool isBase, int collumn, int row)
	{
		this.isBase = isBase;
		this.collumn = collumn;
		this.row = row;
		this.completed = completed;
	}

	public WorldMapTerrainSaveInfo(bool completed, int collumn, int row, string name, Vector3 position, int difficulty, int length)
	{
		this.collumn = collumn;
		this.row = row;
		this.completed = completed;
	}

	public int collumn = -1;

	public int row = -1;

	public bool completed;

	public bool isBase;
}
