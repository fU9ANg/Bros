// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BronnarJensen : TestVanDammeAnim
{
	protected override void Awake()
	{
		this.isHero = true;
		base.Awake();
	}

	protected override void PressSpecial()
	{
		if (this.actionState != ActionState.Melee && base.SpecialAmmo > 0 && this.health > 0)
		{
			this.usingSpecial = true;
			this.frame = 0;
			this.gunFrame = 4;
			this.ChangeFrame();
		}
	}

	protected override void UseSpecial()
	{
		if (base.SpecialAmmo > 0 && this.remoteVehicle == null)
		{
			base.SpecialAmmo--;
			bool flag;
			if (base.IsEnemy)
			{
				flag = this.IsLocalMook;
			}
			else
			{
				flag = base.IsMine;
			}
			if (flag)
			{
				this.projectileTime = Time.time;
				this.remoteVehicle = Networking.Instantiate<RemoteControlExplosiveCar>(this.remoteControlVehiclePrefab, base.transform.position, Quaternion.identity, false);
				this.remoteVehicle.playerNum = this.playerNum;
				this.remoteVehicle.Knock(DamageType.Bounce, base.transform.localScale.x * 100f, 100f, false);
			}
		}
		else
		{
			HeroController.FlashSpecialAmmo(this.playerNum);
			this.ActivateGun();
		}
	}

	protected override void AnimateSpecial()
	{
		this.SetSpriteOffset(0f, 0f);
		this.ActivateGun();
		this.frameRate = 0.0334f;
		this.SetGunSprite(5 - this.frame, 0);
		if (this.frame == 0)
		{
			this.UseSpecial();
		}
		if (this.frame >= 5)
		{
			this.gunFrame = 0;
			this.frame = 0;
			this.usingSpecial = false;
		}
	}

	protected override void UseFire()
	{
		if (base.IsMine)
		{
			this.FireWeapon(this.x + base.transform.localScale.x * 6f, this.y + 10f, base.transform.localScale.x * this.shootGrenadeSpeedX + this.xI * 0.45f, this.shootGrenadeSpeedY + ((this.yI <= 0f) ? 0f : (this.yI * 0.3f)));
		}
		this.PlayAttackSound(0.4f);
		Map.DisturbWildLife(this.x, this.y, 60f, this.playerNum);
		this.fireDelay = 0.6f;
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.gunFrame = 3;
		this.SetGunSprite(this.gunFrame, 1);
		if (base.IsEnemy)
		{
			bool flag = this.IsLocalMook;
		}
		else
		{
			bool flag = base.IsMine;
		}
		EffectsController.CreateMuzzleFlashRoundEffect(x + 1f, y + 1f, -25f, xSpeed * 0.2f, ySpeed * 0.2f, base.transform);
		this.SetGunSprite(this.gunFrame, 0);
		this.gunCounter = 0f;
		if (base.IsMine)
		{
			ProjectileController.SpawnProjectileOverNetwork(this.projectile, this, x, y, xSpeed, ySpeed, true, this.playerNum, false, false);
		}
	}

	protected override void CheckInput()
	{
		base.CheckInput();
		LevelEditorGUI.DebugText = this.left.ToString() + ", " + this.right.ToString();
		if (this.remoteVehicle != null && this.remoteVehicle.gameObject.activeInHierarchy)
		{
			this.remoteVehicle.wasUp = this.remoteVehicle.up;
			this.remoteVehicle.wasDown = this.remoteVehicle.down;
			this.remoteVehicle.wasLeft = this.remoteVehicle.left;
			this.remoteVehicle.wasRight = this.remoteVehicle.right;
			this.remoteVehicle.wasButtonJump = this.remoteVehicle.buttonJump;
			this.remoteVehicle.up = this.up;
			this.remoteVehicle.down = this.down;
			this.remoteVehicle.left = this.left;
			this.remoteVehicle.right = this.right;
			this.remoteVehicle.buttonJump = this.buttonJump;
			this.up = false;
			this.left = false;
			this.right = false;
			this.down = false;
			this.buttonJump = false;
			if (Time.time - this.projectileTime > 0.56f && ((this.special && !this.wasSpecial) || (this.fire && !this.wasFire)))
			{
				UnityEngine.Debug.Log("Death ");
				this.controllingProjectile = false;
				this.usingSpecial = false;
				this.remoteVehicle.Explode();
			}
			this.usingSpecial = false;
			this.fire = false;
			this.controllingRCVDelay = 0.25f;
		}
		else
		{
			if (this.controllingRCVDelay > 0f)
			{
				this.controllingRCVDelay -= this.t;
				this.up = false;
				this.left = false;
				this.right = false;
				this.down = false;
				this.usingSpecial = false;
				this.fire = false;
			}
			this.controllingProjectile = false;
		}
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		if (this.remoteVehicle != null)
		{
			this.remoteVehicle.Explode();
		}
		base.Death(xI, yI, damage);
	}

	public Grenade primaryGrenade;

	public RemoteControlExplosiveCar remoteControlVehiclePrefab;

	private RemoteControlExplosiveCar remoteVehicle;

	public float shootGrenadeSpeedX = 300f;

	public float shootGrenadeSpeedY = 60f;

	private bool controllingVehicle;

	private float controllingRCVDelay;
}
