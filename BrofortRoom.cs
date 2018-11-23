// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class BrofortRoom
{
	public BrofortRoom(int roomC, int roomR, int worldC, int worldR, BrofortRoomType roomType)
	{
		this.roomCollumn = roomC;
		this.roomRow = roomR;
		this.worldCollumn = worldC;
		this.worldRow = worldR;
		this.type = roomType;
	}

	public void AddDoodad(Doodad doodad)
	{
		this.doodads.Add(doodad);
	}

	public BrofortRoomType type;

	public int roomCollumn;

	public int roomRow;

	public int size = 2;

	public int worldCollumn;

	public int worldRow;

	public bool hasLadder;

	public int ladderPos = 4;

	public List<Doodad> doodads = new List<Doodad>();
}
