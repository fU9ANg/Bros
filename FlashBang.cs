// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FlashBang : Grenade
{
	public override void Death()
	{
		this.MakeEffects();
		this.DestroyGrenade();
	}

	protected override void MakeEffects()
	{
		EffectsController.CreateShrapnel(this.shrapnel, this.x, this.y, 3f, 200f, 40f, 0f, 250f);
		FlashBangExplosion flashBangExplosion = UnityEngine.Object.Instantiate(this.flashBangExplosion, base.transform.position + Vector3.up, Quaternion.identity) as FlashBangExplosion;
		flashBangExplosion.Setup(this.playerNum, this.firedBy, DirectionEnum.Any);
		MonoBehaviour.print("firedBy " + this.firedBy);
		SortOfFollow.Shake(0.4f);
		this.PlayDeathSound();
		Map.DisturbWildLife(this.x, this.y, 120f, 5);
	}

	public FlashBangExplosion flashBangExplosion;
}
