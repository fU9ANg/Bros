// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HelicopterMookLauncher : TankWeapon
{
	protected override void Update()
	{
		base.Update();
	}

	public override void SetTargetPlayerNum(int pN, Vector3 TargetPosition)
	{
		MonoBehaviour.print(string.Concat(new object[]
		{
			"SetTargetPlayerNum ",
			TargetPosition,
			" ",
			base.transform.position
		}));
		this.targetPlayerNum = pN;
	}

	protected override void RunFiring()
	{
		this.fireDelay -= this.t;
		if (this.fire && this.health > 0 && this.tank.CanFire())
		{
			if (this.fireDelay <= 0f)
			{
				this.fireCounter += this.t;
				if (this.fireCounter > 0f)
				{
					this.fireFrameCounter += this.t;
					if (this.fireFrameCounter > 0.0334f)
					{
						this.fireFrameCounter -= 0.0334f;
						this.fireFrame++;
						if (this.fireFrame == 15)
						{
							this.FireWeapon(ref this.fireIndex);
						}
						if (this.fireFrame >= 17)
						{
							this.fireFrame = 0;
							this.fireCounter -= this.fireRate;
							if (this.fireIndex >= this.mookWaveCount)
							{
								this.fire = false;
								this.fireDelay = 2.1f;
								this.fireIndex = 0;
							}
						}
					}
				}
			}
			else
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.nextTargetRotation, 30f * Time.deltaTime);
			}
		}
		else
		{
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.identity, 45f * Time.deltaTime);
		}
		this.wasFire = this.fire;
	}

	protected override void FireWeapon(ref int index)
	{
		float num = 1f + UnityEngine.Random.value * 0.5f;
		float num2 = 0f;
		float num3 = 0f;
		if (HeroController.PlayerIsAlive(this.targetPlayerNum))
		{
			HeroController.GetPlayerPos(this.targetPlayerNum, ref num2, ref num3);
		}
		else
		{
			this.targetPlayerNum = -1;
		}
		if (num2 > 0f)
		{
			float f = num2 - this.tank.x;
			float num4 = num3 - this.tank.y;
			num = 0.2f + Mathf.Abs(f) / 200f + num4 / 300f;
			if (num < 0.5f)
			{
				num = 0.5f;
			}
		}
		Vector3 position = base.transform.position;
		position.z = this.launchPoint.position.z;
		Vector3 normalized = (this.launchPoint.position - position).normalized;
		float z = base.transform.rotation.eulerAngles.z;
		normalized.x = Mathf.Cos(0.0174532924f * z);
		normalized.y = Mathf.Sin(0.0174532924f * z);
		if (this.IsLocalMook)
		{
			ProjectileController.SpawnGrenadeOverNetwork(this.specialGrenade, this, base.transform.position.x + normalized.x * 50f, base.transform.position.y + normalized.y * 50f, 0.001f, 0.011f, normalized.x * 320f * num * (0.8f + UnityEngine.Random.value * 0.4f), normalized.y * 320f * num * UnityEngine.Random.Range(0.8f, 1.3f), this.playerNum);
			this.nextTargetRotation = Quaternion.Euler(new Vector3(0f, 0f, (float)UnityEngine.Random.Range(15, 45)));
		}
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.6f, new Vector3(base.transform.position.x + normalized.x * 50f, base.transform.position.y + normalized.y * 50f, 0f));
		index++;
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
	}

	internal new void StopFiring()
	{
		this.fire = false;
	}

	protected bool hidingHoles;

	protected int targetPlayerNum = -1;

	public int mookWaveCount = 3;

	public Mook[] mookProjectiles;

	public Grenade specialGrenade;

	protected int fireFrame;

	protected float fireFrameCounter;

	private Quaternion nextTargetRotation = Quaternion.Euler(new Vector3(0f, 0f, 45f));

	public Transform launchPoint;
}
