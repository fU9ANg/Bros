// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public struct ParentedPosition
{
	public ParentedPosition(float worldX, float worldY, Transform Parent)
	{
		this.parent = Parent;
		if (Parent == null)
		{
			this.localX = worldX;
			this.localY = worldY;
		}
		else
		{
			this.localX = worldX - Parent.position.x;
			this.localY = worldY - Parent.position.y;
		}
	}

	public float WorldX
	{
		get
		{
			if (this.parent == null)
			{
				return this.localX;
			}
			return this.parent.position.x + this.localX;
		}
	}

	public float WorldY
	{
		get
		{
			if (this.parent == null)
			{
				return this.localY;
			}
			return this.parent.position.y + this.localY;
		}
	}

	public static ParentedPosition Lerp(ParentedPosition from, ParentedPosition to, float t)
	{
		Vector3 start = new Vector3(from.WorldX, from.WorldY);
		Vector3 start2 = new Vector3(to.WorldX, to.WorldY);
		UnityEngine.Debug.DrawRay(start, UnityEngine.Random.onUnitSphere * 3f, Color.magenta);
		UnityEngine.Debug.DrawRay(start2, UnityEngine.Random.onUnitSphere * 3f, Color.magenta);
		float worldX = Mathf.Lerp(from.WorldX, to.WorldX, t);
		float worldY = Mathf.Lerp(from.WorldY, to.WorldY, t);
		Transform transform = from.parent;
		if (t > 0.5f)
		{
			transform = to.parent;
		}
		return new ParentedPosition(worldX, worldY, transform);
	}

	public float localX;

	public float localY;

	public Transform parent;
}
