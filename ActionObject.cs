// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class ActionObject
{
	public ActionObject(EnemyActionType type, GridPoint gridPoint, float duration)
	{
		this.gridPoint = gridPoint;
		this.duration = duration;
		this.type = type;
	}

	public ActionObject(EnemyActionType type, float duration)
	{
		this.type = type;
		this.duration = duration;
	}

	public ActionObject(EnemyActionType type, GridPoint gridPoint)
	{
		this.type = type;
		this.gridPoint = gridPoint;
	}

	public ActionObject(EnemyActionType type)
	{
		this.type = type;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			this.type.ToString(),
			", ",
			(!(this.gridPoint == null)) ? this.gridPoint.ToString() : "no gridpoint",
			", ",
			this.duration
		});
	}

	public GridPoint gridPoint;

	public float duration;

	public EnemyActionType type;
}
