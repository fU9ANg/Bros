// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PID
{
	public PID(byte id)
	{
		this.ID = id;
	}

	public PID(int id)
	{
		this.ID = (byte)id;
	}

	public static bool ServerHasBeenSet
	{
		get
		{
			return PID.serverHasBeenSet;
		}
		set
		{
			if (value && !PID.serverHasBeenSet && PID.myIdHasBeenSet)
			{
				Connect.IDsAreSetup();
			}
			PID.serverHasBeenSet = value;
		}
	}

	public static bool MyIdHasBeenSet
	{
		get
		{
			return PID.myIdHasBeenSet;
		}
		set
		{
			if (value && !PID.myIdHasBeenSet && PID.serverHasBeenSet)
			{
				Connect.IDsAreSetup();
			}
			PID.myIdHasBeenSet = value;
		}
	}

	public static PID MyID
	{
		get
		{
			return PID.myID;
		}
	}

	public static PID ServerID
	{
		get
		{
			return PID.serverID;
		}
	}

	public string PlayerName
	{
		get
		{
			return Connect.GetPlayerName(this);
		}
	}

	public float Ping
	{
		get
		{
			return PingController.GetPing(this);
		}
	}

	public float RawPing
	{
		get
		{
			return PingController.GetRawPing(this);
		}
	}

	public byte AsByte
	{
		get
		{
			return this.ID;
		}
	}

	public int AsInt
	{
		get
		{
			return (int)this.ID;
		}
	}

	public bool IsMine
	{
		get
		{
			return this == PID.MyID;
		}
	}

	public static bool IamServer()
	{
		return PID.MyID == PID.ServerID;
	}

	public bool IsDefault()
	{
		return this.ID == PID.defaultID;
	}

	public static void Reset()
	{
		PID.allocatedIDs = 0;
		PID.SetMyID(PID.defaultID);
		PID.myIdHasBeenSet = false;
		PID.serverID = PID.MyID;
		PID.serverHasBeenSet = false;
	}

	public static void SetMyID(byte newID)
	{
		UnityEngine.Debug.Log("> [4] SetMyID " + newID);
		PID.myID.ID = newID;
		Registry.RecacheDictionaries();
		PID.MyIdHasBeenSet = true;
	}

	public static void SetServerID(PID newID)
	{
		UnityEngine.Debug.Log("> [4] SetServerID " + newID);
		PID.serverID = newID;
		PID.ServerHasBeenSet = true;
		Registry.RecacheDictionaries();
	}

	public override string ToString()
	{
		if (this == PID.NoID)
		{
			return "NoID";
		}
		if (this == PID.TargetAll)
		{
			return "TargetAll";
		}
		if (this == PID.TargetOthers)
		{
			return "TargetOthers";
		}
		string result = this.ID + " " + this.PlayerName;
		if (this == PID.ServerID)
		{
			return " (Server)";
		}
		return result;
	}

	public static PID Allocate()
	{
		if (PID.allocatedIDs == PID.TargetServer)
		{
			PID.allocatedIDs += 1;
		}
		if (PID.allocatedIDs == PID.TargetOthers)
		{
			PID.allocatedIDs += 1;
		}
		if (PID.allocatedIDs == PID.TargetAll)
		{
			PID.allocatedIDs += 1;
		}
		if (PID.allocatedIDs == PID.NoID)
		{
			PID.allocatedIDs += 1;
		}
		if (PID.allocatedIDs == PID.MyID)
		{
			PID.allocatedIDs += 1;
		}
		PID result = new PID(PID.allocatedIDs);
		PID.allocatedIDs += 1;
		PID.allocatedIDs %= byte.MaxValue;
		return result;
	}

	public override int GetHashCode()
	{
		return this.ID.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return obj is PID && (PID)obj == this;
	}

	public static explicit operator PID(int id)
	{
		return new PID(id);
	}

	public static explicit operator int(PID pid)
	{
		return (int)pid.ID;
	}

	public static explicit operator PID(byte id)
	{
		return new PID(id);
	}

	public static explicit operator byte(PID pid)
	{
		return pid.ID;
	}

	public static bool operator ==(PID pid1, byte pid2)
	{
		return pid1.ID == pid2;
	}

	public static bool operator !=(PID pid1, byte pid2)
	{
		return !(pid1 == pid2);
	}

	public static bool operator ==(byte pid1, PID pid2)
	{
		return pid1 == pid2.ID;
	}

	public static bool operator !=(byte pid1, PID pid2)
	{
		return !(pid1 == pid2);
	}

	public static bool operator ==(PID pid1, PID pid2)
	{
		pid1 = (pid1 ?? PID.NoID);
		pid2 = (pid2 ?? PID.NoID);
		return (pid1.ID == PID.NoID.ID && pid2.ID == PID.NoID.ID) || (pid1.ID != PID.NoID.ID && pid2.ID != PID.NoID.ID && pid1.ID == pid2.ID);
	}

	public static bool operator !=(PID pid1, PID pid2)
	{
		return !(pid1 == pid2);
	}

	private static bool serverHasBeenSet = false;

	private static bool myIdHasBeenSet = false;

	public static PID TargetServer = new PID(0);

	public static PID TargetOthers = new PID(1);

	public static PID TargetAll = new PID(2);

	public static PID NoID = new PID(3);

	private static byte defaultID = 4;

	private static PID myID = new PID(PID.defaultID);

	private static PID serverID = PID.MyID;

	private byte ID;

	public static byte allocatedIDs = 0;
}
