// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CheckPointExplosionRun : CheckPoint
{
	public override void ActivateInternal()
	{
		if (!this.activated)
		{
			SortOfFollow.SlowTimeDown(0.5f);
			SortOfFollow.ResetSpeed();
		}
		base.ActivateInternal();
	}

	protected override void Start()
	{
		base.Start();
		SortOfFollow.RegisterExplosionRunCheckPoint(this);
	}

	public CameraFollowMode cameraFollowMode;

	[HideInInspector]
	public bool explosionRunDirecitonUsed;

	public float speedM = 1f;
}
