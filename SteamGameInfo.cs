// dnSpy decompiler from Assembly-CSharp.dll
using System;
using ManagedSteam.SteamTypes;

public class SteamGameInfo : GameInfo
{
	public SteamGameInfo(SteamID lobbyID)
	{
		this.Match = lobbyID;
		string lobbyData = SteamLayer.matchMaking.GetLobbyData(lobbyID, "EncodedData");
		base.DecodeGameInfo(lobbyData);
	}

	public SteamID Match;
}
