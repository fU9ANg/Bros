// dnSpy decompiler from Assembly-CSharp.dll
using System;
using Badumna.Match;

public class BadumnaGameInfo : GameInfo
{
	public BadumnaGameInfo(MatchmakingResult Room)
	{
		this.room = Room;
		base.DecodeGameInfo(Room.Criteria.MatchName);
	}

	public override int Capacity
	{
		get
		{
			return this.room.Capacity.NumSlots;
		}
	}

	public override int EmptySlots
	{
		get
		{
			return this.room.Capacity.EmptySlots;
		}
	}

	public MatchmakingResult room;
}
