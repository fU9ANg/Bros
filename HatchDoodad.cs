// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HatchDoodad : DoodadDestroyable
{
	protected void Close()
	{
		this.platformColliders.SetActive(true);
		this.sprite.SetSize(this.startWidth, this.startHeight);
		this.sprite.SetOffset(this.startOffset);
		this.sprite.SetLowerLeftPixel(this.startLowerLeft);
		this.opened = false;
		base.gameObject.layer = LayerMask.NameToLayer("DirtyHippie");
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.special2Sounds, 0.6f, base.transform.position);
		Map.DisturbWildLife(base.centerX, base.centerY, 80f, 5);
	}

	protected void Open(int direction)
	{
		if (!this.opened)
		{
			this.platformColliders.SetActive(false);
			this.openTime = Time.time;
			base.gameObject.layer = LayerMask.NameToLayer("Movetivate");
			this.opened = true;
			this.sprite.SetLowerLeftPixel(32f, 16f);
			Map.RemoveDestroyableDoodad(this);
			if (this.lastDamageObject != null && this.lastDamageObject.damageType == DamageType.Knifed)
			{
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialSounds, 0.2f, base.transform.position);
			}
			else
			{
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialSounds, 0.77f, base.transform.position);
				Map.DisturbWildLife(base.centerX, base.centerY, 90f, 5);
			}
			this.openedDirection = 1;
		}
	}

	public void Use(DamageObject damageObject)
	{
	}

	public override bool DamageOptional(DamageObject damageObject, ref bool showBulletHit)
	{
		showBulletHit = true;
		return this.Damage(damageObject);
	}

	public override bool Damage(DamageObject damageObject)
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			"Damage !  x ",
			damageObject.xForce,
			"  y ",
			damageObject.yForce
		}));
		if (damageObject.damageType == DamageType.Melee || damageObject.damageType == DamageType.Knifed)
		{
			this.lastDamageObject = damageObject;
			if (!this.opened)
			{
				this.Open((int)Mathf.Sign(damageObject.yForce));
			}
			return true;
		}
		this.platformColliders.SetActive(false);
		return base.Damage(damageObject);
	}

	protected override void Start()
	{
		base.Start();
		int collumn = Map.GetCollumn(base.transform.position.x);
		int row = Map.GetRow(base.transform.position.y);
		GroundType backgroundGroundType = Map.GetBackgroundGroundType(collumn - 1, row, GroundType.Empty);
		GroundType backgroundGroundType2 = Map.GetBackgroundGroundType(collumn + 1, row, GroundType.Empty);
		bool flag = backgroundGroundType == GroundType.EarthBehind || backgroundGroundType == GroundType.BunkerBehind || backgroundGroundType == GroundType.BrickBehind || backgroundGroundType == GroundType.BrickBehindDoodads || backgroundGroundType == GroundType.WoodenBehind;
		bool flag2 = backgroundGroundType2 == GroundType.EarthBehind || backgroundGroundType2 == GroundType.BunkerBehind || backgroundGroundType2 == GroundType.BrickBehind || backgroundGroundType == GroundType.BrickBehindDoodads || backgroundGroundType2 == GroundType.WoodenBehind;
		if (flag && !flag2)
		{
			this.sprite.SetSize(-16f, 32f);
			base.transform.Translate(4f, 0f, 0f, Space.World);
			this.y -= 4f;
		}
		else if (!flag && flag2)
		{
			base.transform.Translate(-4f, 0f, 0f, Space.World);
			this.x -= 4f;
		}
		this.startOffset = this.sprite.offset;
		this.startWidth = this.sprite.width;
		this.startHeight = this.sprite.height;
		this.startLowerLeft = this.sprite.lowerLeftPixel;
	}

	protected override void MakeEffectsDeath()
	{
		float num = 0f + ((this.lastDamageObject == null) ? 0f : this.lastDamageObject.xForce);
		float num2 = 0f + ((this.lastDamageObject == null) ? 0f : this.lastDamageObject.yForce);
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.deathSounds, 0.45f, base.transform.position);
		if (this.willExplode)
		{
			EffectsController.CreateWoodParticles(base.centerX, base.centerY, 24, this.width / 2f, this.height / 2f, 300f, 0f + num * 0.3f, 50f + num2 * 0.3f, 250f);
			EffectsController.CreateExplosion(base.centerX, base.centerY, this.width / 2f, this.height / 2f, 120f, 1f, 120f, 0.5f, 0.45f, false);
			Map.BurnUnitsAround_Local(this, -1, 1, 64f, this.x, this.y, true, true);
			Map.ExplodeUnits(this, 12, DamageType.Explosion, 48f, 32f, this.x, this.y, 200f, 300f, 10, false, false);
		}
		else
		{
			EffectsController.CreateWoodParticles(base.centerX, base.centerY, 24, this.width / 2f, this.height / 2f, 120f, 0f + num * 0.3f, 50f + num2 * 0.3f, 160f);
		}
		this.sprite.SetOffset(new Vector3(this.sprite.offset.x, this.sprite.offset.y, 10f));
		base.GetComponent<Collider>().enabled = false;
	}

	private bool opened;

	protected float openTime;

	protected int openedDirection;

	protected Vector3 startOffset;

	protected Vector2 startLowerLeft;

	protected float startWidth = 16f;

	protected float startHeight = 32f;

	public GameObject platformColliders;
}
