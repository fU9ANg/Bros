// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Brononymous : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void UseFire()
	{
		this.FireWeapon(this.x + base.transform.localScale.x * 14f, this.y + 9.5f, base.transform.localScale.x * 700f, 0f);
		this.fireDelay = 0.2f;
		this.PlayAttackSound(0.45f);
		Map.DisturbWildLife(this.x, this.y, 130f, this.playerNum);
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.gunSprite.SetLowerLeftPixel((float)(32 * this.gunFrame), 32f);
		EffectsController.CreateMuzzleFlashEffect(x, y, -25f, xSpeed * 0.01f, ySpeed * 0.01f, base.transform);
		ProjectileController.SpawnProjectileLocally(this.projectile, this, x, y, xSpeed, ySpeed, this.playerNum);
		this.yI += 150f;
		this.xIBlast = -base.transform.localScale.x * 150f;
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0)
		{
			base.SpecialAmmo--;
			HeroController.SetSpecialAmmo(this.playerNum, base.SpecialAmmo);
			this.PlaySpecialAttackSound(0.3f);
			Neuraliser neuraliser = UnityEngine.Object.Instantiate(this.neuraliserPrefab, base.transform.position + Vector3.up * (this.height + 3f) + Vector3.right * base.transform.localScale.x * 4f, Quaternion.identity) as Neuraliser;
			neuraliser.direction = (int)base.transform.localScale.x;
			neuraliser.playerNum = this.playerNum;
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.ActivateGun();
		}
	}

	public Neuraliser neuraliserPrefab;
}
