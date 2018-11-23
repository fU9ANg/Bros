// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class PathPoint
{
	public PathPoint(GridPoint point, float gValue, PathMoveMode moveMode)
	{
		this.point = point;
		this.gValue = gValue;
		this.moveMode = moveMode;
	}

	public PathPoint(GridPoint point, float gValue)
	{
		this.point = point;
		this.gValue = gValue;
		this.moveMode = PathMoveMode.Normal;
	}

	public PathPoint(int collumn, int row, float gValue)
	{
		this.point = new GridPoint(collumn, row);
		this.gValue = gValue;
	}

	public int row
	{
		get
		{
			return this.point.row;
		}
	}

	public int collumn
	{
		get
		{
			return this.point.collumn;
		}
	}

	public float gValue;

	public float fScore;

	public float fScoreHeuristic;

	public GridPoint point;

	public PathPoint previous;

	public PathMoveMode moveMode;
}
