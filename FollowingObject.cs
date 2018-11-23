// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FollowingObject : BroforceObject
{
	public void Follow(Transform trans, Vector3 offset)
	{
		this.followTransform = trans;
		this.offset = offset;
		base.transform.position = trans.position + offset;
		this.mook = trans.gameObject.GetComponent<Mook>();
		this.icon = base.GetComponent<AnimatedIcon>();
	}

	private void Start()
	{
	}

	protected virtual void Update()
	{
		if (this.icon != null && this.mook != null && this.mook.actionState == ActionState.Dead && !this.icon.goingAway)
		{
			this.icon.GoAway();
		}
	}

	protected override void LateUpdate()
	{
		if (this.followTransform != null)
		{
			base.transform.position = this.followTransform.position + this.offset;
		}
		base.LateUpdate();
	}

	private Vector3 offset;

	private Transform followTransform;

	private Mook mook;

	private AnimatedIcon icon;
}
