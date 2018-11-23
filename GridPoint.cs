// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GridPoint
{
	public GridPoint()
	{
		this.row = 0; this.collumn = (this.row );
	}

	public GridPoint(int c, int r)
	{
		this.collumn = c;
		this.row = r;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"C:",
			this.collumn,
			" R:",
			this.row
		});
	}

	public int CollumnAdjusted
	{
		get
		{
			if (Map.startFromSuperCheckPoint)
			{
				return this.collumn - Map.lastXLoadOffset;
			}
			return this.collumn;
		}
	}

	public int RowAdjusted
	{
		get
		{
			if (Map.startFromSuperCheckPoint)
			{
				return this.row - Map.lastYLoadOffset;
			}
			return this.row;
		}
	}

	public override bool Equals(object o)
	{
		GridPoint gridPoint = o as GridPoint;
		return !(gridPoint == null) && this.collumn == gridPoint.collumn && this.row == gridPoint.row;
	}

	public override int GetHashCode()
	{
		return this.row * this.collumn + 17 % (this.row * 12 + this.collumn * 13 + 10);
	}

	public static GridPoint FromString(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return null;
		}
		try
		{
			int num = str.IndexOf(':');
			int num2 = str.IndexOf(' ');
			int num3 = str.LastIndexOf(':');
			int c = int.Parse(str.Substring(num + 1, num2 - num - 1));
			int r = int.Parse(str.Substring(num3 + 1));
			return new GridPoint(c, r);
		}
		catch (Exception)
		{
			UnityEngine.Debug.LogError("could not parse GridPoint: " + str);
		}
		return null;
	}

	public static bool operator ==(GridPoint a, GridPoint b)
	{
		return (object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null)) || (!object.ReferenceEquals(a, null) && !object.ReferenceEquals(b, null) && (a.row == b.row && a.collumn == b.collumn));
	}

	public static bool operator !=(GridPoint a, GridPoint b)
	{
		return (object.ReferenceEquals(a, null) && !object.ReferenceEquals(b, null)) || (object.ReferenceEquals(b, null) && !object.ReferenceEquals(a, null)) || ((!object.ReferenceEquals(b, null) || !object.ReferenceEquals(a, null)) && (a.row != b.row || a.collumn != b.collumn));
	}

	public int collumn;

	public int row;
}
