// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BossFollowGameObject : MonoBehaviour
{
	protected void Update()
	{
		if (Map.isEditing)
		{
			return;
		}
		if (this.followCounter > 0f)
		{
			this.followCounter -= Time.deltaTime;
			if (this.followCounter <= 0f)
			{
				HeroController.TryFollow(base.transform);
			}
		}
	}

	protected float followCounter = 1f;
}
