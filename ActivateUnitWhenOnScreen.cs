// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ActivateUnitWhenOnScreen : MonoBehaviour
{
	private void Start()
	{
		this.unit = base.GetComponent<Unit>();
	}

	public void SetExtents(float minX, float minY, float maxX, float maxY)
	{
		this.hasSetExtents = true;
		this.minX = minX;
		this.minY = minY;
		this.maxX = maxX;
		this.maxY = maxY;
	}

	private void Update()
	{
		if (!Map.isEditing)
		{
			if (!this.hasSetExtents)
			{
				this.startX = base.transform.position.x;
				this.startY = base.transform.position.y;
				this.hasSetExtents = true;
				this.minX = base.transform.position.x - 16f;
				this.maxX = base.transform.position.x + 16f;
				this.minY = base.transform.position.y - 16f;
				this.maxY = base.transform.position.y + 16f;
			}
			if (this.followUnit)
			{
				this.offsetX = base.transform.position.x - this.startX;
				this.offsetY = base.transform.position.y - this.startY;
			}
			if (this.CanActivate() && this.unit.Activate())
			{
				base.enabled = false;
			}
		}
	}

	protected bool CanActivate()
	{
		return this.minX + this.offsetX < SortOfFollow.maxX && this.maxX + this.offsetX > SortOfFollow.minX && this.minY + this.offsetY < SortOfFollow.maxY && this.maxY + this.offsetY > SortOfFollow.minY;
	}

	protected float minX;

	protected float minY;

	protected float maxX;

	protected float maxY;

	protected float offsetX;

	protected float offsetY;

	protected float startX;

	protected float startY;

	protected bool hasSetExtents;

	public bool followUnit;

	protected Unit unit;
}
