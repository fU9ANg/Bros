// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BlockBuriedRemoteRocket : BarrelBlock
{
	public void Launch(Unit hero)
	{
		UnityEngine.Debug.Log("LAunch!");
		this.heroPilot = hero;
		this.health = -20;
		this.Collapse(0f, 30f, 1f);
	}

	protected override void MakeEffects()
	{
		UnityEngine.Debug.Log("Make effecst!");
		int playerNum = -1;
		if (this.heroPilot != null)
		{
			playerNum = this.heroPilot.playerNum;
		}
		TestVanDammeAnim testVanDammeAnim = this.heroPilot as TestVanDammeAnim;
		Projectile projectile = ProjectileController.SpawnProjectileOverNetwork(this.rocketPrefab, this, this.x + 6f * base.transform.localScale.x, this.y + 11f, 0f, 90f, true, playerNum, true, false);
		if (projectile != null && testVanDammeAnim != null)
		{
			UnityEngine.Debug.Log("Set Off Rocket!");
			testVanDammeAnim.SetRemoteProjectile(projectile);
		}
	}

	protected Unit heroPilot;

	public RemoteRocket rocketPrefab;
}
