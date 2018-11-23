// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;

public class NavPath
{
	public GridPoint TargetPoint
	{
		get
		{
			if (this.points.Count > 0)
			{
				return new GridPoint(this.points[this.points.Count - 1].collumn, this.points[this.points.Count - 1].row);
			}
			return null;
		}
	}

	public void AddPointToStart(PathPoint point)
	{
		this.points.Insert(0, point);
	}

	public List<PathPoint> points = new List<PathPoint>();
}
