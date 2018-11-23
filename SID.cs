// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class SID
{
	public SID(byte id)
	{
		this.ID = id;
	}

	public SID(int id)
	{
		this.ID = (byte)id;
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

	public override string ToString()
	{
		if (this == SID.NoID)
		{
			return "NoID";
		}
		return this.ID + string.Empty;
	}

	public void Increment()
	{
		for (int i = 0; i < 5; i++)
		{
			this.ID += 1;
			if (this.ID != 255 && this.ID != 254)
			{
				break;
			}
		}
	}

	public void Set(byte newID)
	{
		this.ID = newID;
	}

	public override int GetHashCode()
	{
		return this.ID.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return obj is SID && (SID)obj == this;
	}

	public static explicit operator SID(int id)
	{
		return new SID(id);
	}

	public static explicit operator int(SID sid)
	{
		return (int)sid.ID;
	}

	public static explicit operator SID(byte id)
	{
		return new SID(id);
	}

	public static explicit operator byte(SID sid)
	{
		return sid.ID;
	}

	public static bool operator ==(SID pid1, byte pid2)
	{
		return pid1.ID == pid2;
	}

	public static bool operator !=(SID pid1, byte pid2)
	{
		return !(pid1 == pid2);
	}

	public static bool operator ==(byte pid1, SID pid2)
	{
		return pid1 == pid2.ID;
	}

	public static bool operator !=(byte pid1, SID pid2)
	{
		return !(pid1 == pid2);
	}

	public static bool operator ==(SID pid1, SID pid2)
	{
		pid1 = (pid1 ?? SID.NoID);
		pid2 = (pid2 ?? SID.NoID);
		return (pid1.ID == SID.NoID.ID && pid2.ID == SID.NoID.ID) || (pid1.ID != SID.NoID.ID && pid2.ID != SID.NoID.ID && pid1.ID == pid2.ID);
	}

	public static bool operator !=(SID pid1, SID pid2)
	{
		return !(pid1 == pid2);
	}

	public const byte noid = 255;

	public const byte ignoreID = 254;

	public static SID NoID = new SID(byte.MaxValue);

	public static SID IgnoreID = new SID(254);

	private byte ID = byte.MaxValue;
}
