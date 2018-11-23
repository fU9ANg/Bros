// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public struct NID
{
	public NID(SID _sid, PID _OwnerID, uint _GoID)
	{
		this.sid = _sid;
		this.ownerID = _OwnerID;
		this.gameObjectID = _GoID;
		this.nestedID = 0;
		this.componentID = 0;
		this.obj = null;
		this.netObject = null;
		this.objName = "Not Set";
	}

	public NID(SID _sid, PID _OwnerID, uint _GoID, byte _NestedID, byte _ComponentID)
	{
		this.sid = _sid;
		this.ownerID = _OwnerID;
		this.gameObjectID = _GoID;
		this.nestedID = _NestedID;
		this.componentID = _ComponentID;
		this.obj = null;
		this.netObject = null;
		this.objName = string.Empty;
	}

	public NID(NID copy)
	{
		this.sid = copy.sid;
		this.ownerID = copy.ownerID;
		this.gameObjectID = copy.gameObjectID;
		this.nestedID = copy.nestedID;
		this.componentID = copy.componentID;
		this.obj = copy.obj;
		this.netObject = copy.netObject;
		this.objName = copy.objName + "  Copy of key";
	}

	public SID Sid
	{
		get
		{
			return this.sid;
		}
	}

	public PID OwnerID
	{
		get
		{
			return this.ownerID;
		}
	}

	public uint GameObjectID
	{
		get
		{
			return this.gameObjectID;
		}
	}

	public byte NestedID
	{
		get
		{
			return this.nestedID;
		}
	}

	public byte ComponentID
	{
		get
		{
			return this.componentID;
		}
	}

	public object Obj
	{
		get
		{
			if (this.obj == null)
			{
				this.obj = Registry.GetObject(this);
			}
			return this.obj;
		}
	}

	public void OverrideSID(SID newSid)
	{
		this.sid = newSid;
	}

	public NetworkObject NetObject
	{
		get
		{
			if (this.netObject == null)
			{
				this.netObject = (this.Obj as NetworkObject);
			}
			return this.netObject;
		}
	}

	public NID BaseID
	{
		get
		{
			return new NID(this.sid, this.ownerID, this.gameObjectID);
		}
	}

	public NID AllocateComponentID(byte _nestedID, byte _ComponentID)
	{
		NID result = new NID(this);
		if (_nestedID > 250 || _ComponentID > 250)
		{
			UnityEngine.Debug.LogError("Overflow. nesting is too deep " + this.ToString());
		}
		result.nestedID = _nestedID;
		result.componentID = _ComponentID;
		return result;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"[",
			this.sid,
			" ",
			this.ownerID,
			" ",
			this.gameObjectID,
			" ",
			this.nestedID,
			" ",
			this.componentID,
			" ",
			this.objName,
			"]"
		});
	}

	public override bool Equals(object obj)
	{
		return obj is NID && (NID)obj == this;
	}

	public override int GetHashCode()
	{
		return this.ownerID.GetHashCode() ^ this.gameObjectID.GetHashCode() ^ this.nestedID.GetHashCode() ^ this.componentID.GetHashCode() ^ this.sid.AsByte.GetHashCode();
	}

	public static bool operator ==(NID nid1, NID nid2)
	{
		return nid1.sid.AsByte == nid2.sid.AsByte && !(nid1.ownerID != nid2.ownerID) && nid1.gameObjectID == nid2.gameObjectID && nid1.nestedID == nid2.nestedID && nid1.componentID == nid2.componentID;
	}

	public static bool operator !=(NID pid1, NID pid2)
	{
		return !(pid1 == pid2);
	}

	private SID sid;

	private PID ownerID;

	private uint gameObjectID;

	private byte nestedID;

	private byte componentID;

	public static byte allocatedIDs = 0;

	public static NID NoID = new NID(SID.NoID, PID.NoID, 0u);

	public string objName;

	private object obj;

	private NetworkObject netObject;
}
