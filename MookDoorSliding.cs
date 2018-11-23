// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookDoorSliding : MookDoor
{
	protected override void Start()
	{
		base.Start();
		this.doorClosedX = this.door.position.x;
	}

	protected override void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (!Map.isEditing && !this.isDestroyed)
		{
			if (this.spawning && (this.mookCount < this.maxMookCount || this.alarmedMookCounter > 0))
			{
				if (this.door.transform.position.x > this.doorClosedX - this.maxDoorOffset)
				{
					this.door.transform.position = new Vector3(Mathf.Clamp(this.door.transform.position.x - this.doorSpeed * this.t, this.doorClosedX - this.maxDoorOffset, this.doorClosedX), this.door.transform.position.y, this.door.transform.position.z);
					this.spawningCounter -= this.t;
					this.doorCloseDelay = 1f;
				}
			}
			else if ((this.doorCloseDelay -= this.t) < 0f)
			{
				this.door.transform.position = new Vector3(Mathf.Clamp(this.door.transform.position.x + this.doorSpeed * this.t, this.doorClosedX - this.maxDoorOffset, this.doorClosedX), this.door.transform.position.y, this.door.transform.position.z);
			}
		}
		base.Update();
	}

	public override void Collapse()
	{
		EffectsController.CreateGibs(this.doorGibHolder, this.door.position.x, this.door.position.y, 20f, 20f, 0f, 0f);
		UnityEngine.Object.Destroy(this.door.gameObject);
		base.Collapse();
	}

	public Transform door;

	public GibHolder doorGibHolder;

	private float maxDoorOffset = 18f;

	private float doorSpeed = 50f;

	private float doorClosedX;

	private float doorCloseDelay = 1f;
}
