// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookBazooka : MookTrooper
{
	protected override void UseFire()
	{
		if (SetResolutionCamera.IsItVisible(base.transform.position))
		{
			this.FireWeapon(this.x + base.transform.localScale.x * 12f, this.y + 8f, base.transform.localScale.x * 100f, (float)(UnityEngine.Random.Range(0, 4) - 2) * ((!Demonstration.bulletsAreFast) ? 0.2f : 1f));
			EffectsController.CreateSmoke(this.x - base.transform.localScale.x * 10f, this.y + 7f, 0f, new Vector3(base.transform.localScale.x * -60f, 0f, 0f));
			EffectsController.CreateSmoke(this.x - base.transform.localScale.x * 9f, this.y + 7f, 0f, new Vector3(base.transform.localScale.x * -50f, 0f, 0f));
			EffectsController.CreateSmoke(this.x - base.transform.localScale.x * 9f, this.y + 7f, 0f, new Vector3(base.transform.localScale.x * -40f, 0f, 0f));
			Map.DisturbWildLife(this.x, this.y, 40f, this.playerNum);
		}
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.fireDelay = 1.4f;
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(this.gunSpritePixelWidth * this.gunFrame), 32f);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		Projectile projectile = ProjectileController.SpawnProjectileOverNetwork(this.projectile, this, x, y, xSpeed, ySpeed, false, -1, false, true);
		this.PlayAttackSound();
		int seenPlayerNum = this.enemyAI.GetSeenPlayerNum();
		if (seenPlayerNum >= 0)
		{
			Networking.RPC<float, float, int>(PID.TargetAll, true, false, false, new RpcSignature<float, float, int>(projectile.Target), projectile.transform.position.x + base.transform.localScale.x * 800f, projectile.transform.position.y, seenPlayerNum);
		}
		this.xIBlast = -base.transform.localScale.x * 40f;
	}

	protected override void PlayAttackSound()
	{
		this.sound.PlaySoundEffectAt(this.soundHolder.attackSounds, 0.4f, base.transform.position, 0.8f);
	}
}
