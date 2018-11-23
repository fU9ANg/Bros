// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using Badumna;
using Badumna.Match;
using UnityEngine;

public class MatchController
{
	public event EndMatchHandler OnMatchEnded;

	public event StateChangedHandler OnStateChanged;

	public IEnumerable<MemberIdentity> Members
	{
		get
		{
			return this.members;
		}
	}

	[Replicable]
	public void MigrateHostRPC(PID newServer, bool announce)
	{
		Connect.OnHostMigration(newServer, announce);
	}

	private static BadumnaLayer badumnaLayer
	{
		get
		{
			return (BadumnaLayer)Connect.Layer;
		}
	}

	public void RemoveMember(Match match, MatchMembershipEventArgs e)
	{
		UnityEngine.Debug.Log("> RemoveMember " + e.Member);
		Analytics.PrintKbpsLog();
		this.members.Remove(e.Member);
		Connect.ClearDCPlayers();
		ChatSystem.AddMessage(e.Member + " has Left", PID.NoID, Connect.Timef);
	}

	[Replicable]
	public void EndMatch()
	{
		UnityEngine.Debug.Log("EndMatch");
		EndMatchHandler onMatchEnded = this.OnMatchEnded;
		if (onMatchEnded != null)
		{
			onMatchEnded();
		}
	}

	public static void AddMember(Match match, MatchMembershipEventArgs e)
	{
		BadumnaIDWrapper badumnaIDWrapper = new BadumnaIDWrapper(e.Member);
		MatchController.memberIDs[badumnaIDWrapper] = e.Member;
		ConnectionLayer.PlayerHasJoinedMatch(badumnaIDWrapper);
	}

	public static MemberIdentity GetMemberID(PID pid)
	{
		if (MatchController.badumnaLayer.playerIdPairs.ContainsKey(pid))
		{
			IDWrapper key = MatchController.badumnaLayer.playerIdPairs[pid];
			if (MatchController.memberIDs.ContainsKey(key))
			{
				return MatchController.memberIDs[key];
			}
		}
		return null;
	}

	[Replicable]
	public void Recieve(byte[] bytes)
	{
		ConnectionLayer.RecieveBytes(bytes);
	}

	public const MemberIdentity NullIdentity = null;

	private List<MemberIdentity> members = new List<MemberIdentity>();

	public static Dictionary<IDWrapper, MemberIdentity> memberIDs = new Dictionary<IDWrapper, MemberIdentity>();
}
