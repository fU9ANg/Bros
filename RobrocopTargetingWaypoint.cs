// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class RobrocopTargetingWaypoint
{
	public RobrocopTargetingWaypoint(DirectionEnum direction)
	{
		this.direction = direction;
	}

	public RobrocopTargetingWaypoint(DirectionEnum direction, float distance)
	{
		this.direction = direction;
		this.distance = distance;
	}

	public DirectionEnum direction;

	public float distance;
}
