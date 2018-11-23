// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SachelPack : Projectile
{
	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 point)
	{
		if (this.hasMadeEffects)
		{
			return;
		}
		if (base.IsMine)
		{
			MapController.DamageGround(this.firedBy, this.damage, this.damageType, this.range, x, y, null);
			Map.ExplodeUnits(this.firedBy, this.damage, DamageType.Explosion, this.range, this.range * 0.7f, x, y, this.blastForce * 40f, 300f, this.playerNum, false, true);
			if (this.stuckToUnit != null)
			{
				this.stuckToUnit.Damage(this.damage, DamageType.Fire, 0f, 200f, (int)Mathf.Sign(this.stuckToUnit.x - x), this.firedBy, x, y);
			}
			Map.HitProjectiles(this.playerNum, this.damage, this.damageType, this.range, x, y, 0f, 0f, 0.25f);
			Map.ShakeTrees(x, y, 128f, 48f, 80f);
		}
		MapController.BurnUnitsAround_NotNetworked(this.firedBy, this.playerNum, 5, this.range * 1.1f, x, y, true, true);
		EffectsController.CreateShrapnel(this.shrapnel, x, y, 10f, 110f, 20f, 0f, 50f);
		Vector3 a = UnityEngine.Random.insideUnitCircle;
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke1, x + a.x * this.range * 0.6f, y + a.y * this.range * 0.6f, UnityEngine.Random.value * 0.2f, UnityEngine.Random.insideUnitSphere * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.smoke2, x + a.x * this.range * 0.6f, y + a.y * this.range * 0.6f, UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosionSmall, x + a.x * this.range * 0.6f, y + a.y * this.range * 0.6f, 0.1f + UnityEngine.Random.value * 0.5f, a * this.range * 3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.explosion, x, y, 0f, a * 0f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, x + a.x * this.range * 0.6f, y + a.y * this.range * 0.6f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, x + a.x * this.range * 0.6f, y + a.y * this.range * 0.6f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire3, x + a.x * this.range * 0.6f, y + a.y * this.range * 0.6f, 0.1f + UnityEngine.Random.value * 0.2f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire1, x + a.x * this.range * 0.6f, y + a.y * this.range * 0.6f, 0.1f + UnityEngine.Random.value * 0.5f, a * this.range * 0.5f * 3f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(this.fire2, x + a.x * this.range * 0.6f, y + a.y * this.range * 0.6f, 0.1f + UnityEngine.Random.value * 0.5f, a * this.range * 0.5f * 3f);
		SortOfFollow.Shake(0.7f);
		this.PlayDeathSound();
		Map.DisturbWildLife(x, y, 80f, 5);
		Map.DamageDoodads(this.damageInternal, x, y, 0f, 0f, this.range * 0.6f, this.playerNum);
	}

	protected override void CheckSpawnPoint()
	{
		this.TryStickToUnit();
		if (!this.sticky)
		{
			base.CheckSpawnPoint();
		}
		else if (this.stuckToUnit == null)
		{
			if (this.xI < 0f)
			{
				if (Physics.Raycast(new Vector3(this.x + 6f, this.y, 0f), Vector3.left, out this.raycastHit, 16f, this.groundLayer))
				{
					this.stuckLeft = true;
					this.x = this.raycastHit.point.x + this.heightOffGround;
					this.yI = 0f; this.xI = (this.yI );
					this.stickyToUnits = false;
					this.PlayStuckSound(0.7f);
				}
			}
			else if (this.xI > 0f && Physics.Raycast(new Vector3(this.x - 6f, this.y, 0f), Vector3.right, out this.raycastHit, 12f, this.groundLayer))
			{
				this.stuckRight = true;
				this.x = this.raycastHit.point.x - this.heightOffGround;
				this.yI = 0f; this.xI = (this.yI );
				this.stickyToUnits = false;
				this.PlayStuckSound(0.7f);
			}
			if (!this.stuckLeft && !this.stuckRight && Physics.Raycast(new Vector3(this.x, this.y - 6f, 0f), Vector3.up, out this.raycastHit, 26f, this.groundLayer))
			{
				this.stuckUp = true;
				this.y = this.raycastHit.point.y - this.heightOffGround;
				this.yI = 0f; this.xI = (this.yI );
				this.stickyToUnits = false;
				this.PlayStuckSound(0.7f);
			}
			this.RegisterProjectile();
		}
	}

	protected override void HitProjectiles()
	{
	}

	protected virtual void TryStickToUnit()
	{
		if (this.stickyToUnits && this.stuckToUnit == null && base.IsMine)
		{
			this.stuckToUnit = Map.GeLivingtUnit(this.playerNum, 0f, 0f, this.x, this.y);
			if (this.stuckToUnit != null)
			{
				this.PlayStuckSound(0.7f);
				Vector3 arg = this.stuckToUnit.transform.InverseTransformPoint(base.transform.position);
				Networking.RPC<Unit, Vector3>(PID.TargetAll, new RpcSignature<Unit, Vector3>(this.StickToUnit), this.stuckToUnit, arg, false);
			}
		}
	}

	protected virtual void PlayStuckSound(float v)
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialAttackSounds, v, base.transform.position);
	}

	public void StickToUnit(Unit unit, Vector3 stucklocalPos)
	{
		this.stuckToUnit = unit;
		this.stuckTolocalPos = stucklocalPos;
		if (unit.enemyAI != null)
		{
			if (unit.enemyAI.IsAlerted())
			{
				this.stuckToUnit.Panic((int)Mathf.Sign(this.xI), this.life + 0.2f, false);
			}
			else
			{
				this.stuckToUnit.enemyAI.ShowQuestionBubble();
			}
		}
	}

	protected override void HitUnits()
	{
		this.TryStickToUnit();
	}

	protected override void RunDamageBackground(float t)
	{
	}

	protected override bool HitWalls()
	{
		if (this.stuckToUnit == null && (this.stuckUp || this.stuckLeft || this.stuckRight))
		{
			if (this.stuckUp)
			{
				if (Physics.Raycast(new Vector3(this.x, this.y - 6f, 0f), Vector3.up, out this.raycastHit, 14f + this.heightOffGround, this.groundLayer))
				{
					this.y = this.raycastHit.point.y - this.heightOffGround;
					this.yI = 0f; this.xI = (this.yI );
				}
				else
				{
					this.stuckUp = false;
				}
			}
			if (this.stuckLeft)
			{
				if (Physics.Raycast(new Vector3(this.x + 6f, this.y, 0f), Vector3.left, out this.raycastHit, 10f + this.heightOffGround, this.groundLayer))
				{
					this.x = this.raycastHit.point.x + this.heightOffGround;
					this.yI = 0f; this.xI = (this.yI );
				}
				else
				{
					this.stuckLeft = false;
				}
			}
			if (this.stuckRight)
			{
				if (Physics.Raycast(new Vector3(this.x - 6f, this.y, 0f), Vector3.right, out this.raycastHit, 10f + this.heightOffGround, this.groundLayer))
				{
					this.x = this.raycastHit.point.x - this.heightOffGround;
					this.yI = 0f; this.xI = (this.yI );
				}
				else
				{
					this.stuckRight = false;
				}
			}
		}
		else
		{
			if (this.xI < 0f)
			{
				if (Physics.Raycast(new Vector3(this.x + 4f, this.y, 0f), Vector3.left, out this.raycastHit, 6f + this.heightOffGround, this.groundLayer))
				{
					this.stickyToUnits = false;
					this.xI *= -this.bounceXM;
					this.PlayBounceSound();
				}
			}
			else if (this.xI > 0f && Physics.Raycast(new Vector3(this.x - 4f, this.y, 0f), Vector3.right, out this.raycastHit, 4f + this.heightOffGround, this.groundLayer))
			{
				this.stickyToUnits = false;
				this.xI *= -this.bounceXM;
				this.PlayBounceSound();
			}
			if (this.yI < 0f)
			{
				if (Physics.Raycast(new Vector3(this.x, this.y + 6f, 0f), Vector3.down, out this.raycastHit, 6f + this.heightOffGround - this.yI * this.t, this.groundLayer))
				{
					this.stickyToUnits = false;
					this.xI *= this.frictionM;
					if (this.yI < -40f)
					{
						this.yI *= -this.bounceYM;
					}
					else
					{
						this.yI = 0f;
						this.y = this.raycastHit.point.y + this.heightOffGround;
					}
					this.PlayBounceSound();
				}
			}
			else if (this.yI > 0f && Physics.Raycast(new Vector3(this.x, this.y - 6f, 0f), Vector3.up, out this.raycastHit, 6f + this.heightOffGround + this.yI * this.t, this.groundLayer))
			{
				this.stickyToUnits = false;
				this.yI *= -(this.bounceYM + 0.1f);
				this.PlayBounceSound();
			}
		}
		if (this.stuckToUnit != null)
		{
			Vector3 vector = this.stuckToUnit.transform.TransformPoint(this.stuckTolocalPos);
			this.x = vector.x;
			this.y = vector.y;
			this.xI = this.stuckToUnit.xI;
			this.yI = this.stuckToUnit.yI;
			if (this.stuckToUnit.health <= 0 && Mathf.Abs(this.stuckToUnit.xI) + Mathf.Abs(this.stuckToUnit.yI) < 100f)
			{
				if (base.IsMine)
				{
					Networking.RPC(PID.TargetAll, new RpcSignature(this.DetachFromUnit), false);
				}
				else
				{
					this.DetachFromUnit();
				}
			}
			return false;
		}
		return true;
	}

	public void DetachFromUnit()
	{
		this.stuckToUnit = null;
		this.stickyToUnits = false;
	}

	protected override void HitFragile()
	{
		if (Physics.Raycast(new Vector3(this.x - Mathf.Sign(this.xI) * this.projectileSize, this.y, 0f), new Vector3(this.xI, this.yI, 0f), out this.raycastHit, this.projectileSize * 2f, this.fragileLayer))
		{
			EffectsController.CreateProjectilePuff(this.raycastHit.point.x, this.raycastHit.point.y);
			if (this.raycastHit.collider.GetComponent<DoorDoodad>() != null)
			{
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.6f, base.transform.position);
				if (Mathf.Abs(this.raycastHit.normal.x) >= Mathf.Abs(this.raycastHit.normal.y))
				{
					this.xI *= -0.7f;
				}
				else
				{
					this.yI *= -0.7f;
				}
			}
			else
			{
				this.ProjectileApplyDamageToBlock(this.raycastHit.collider.gameObject, this.damageInternal, this.damageType, this.xI, this.yI);
			}
		}
	}

	protected void PlayBounceSound()
	{
		float num = Mathf.Abs(this.xI) + Mathf.Abs(this.yI);
		if (num > 33f)
		{
			float num2 = num / 210f;
			float num3 = 0.05f + Mathf.Clamp(num2 * num2, 0f, 0.25f);
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, num3 * this.bounceVolumeM, base.transform.position);
		}
	}

	protected override void Update()
	{
		if (!this.stuckUp && !this.stuckLeft && !this.stuckRight)
		{
			this.ApplyGravity();
		}
		base.Update();
		this.SetRotation();
	}

	protected virtual void ApplyGravity()
	{
		this.yI -= 600f * this.t;
	}

	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		this.HitWalls();
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		Map.RegisterFetchObject(x, y, 112f, 48f, base.transform);
	}

	protected override void RunProjectile(float t)
	{
		base.RunProjectile(t);
	}

	public float range = 28f;

	public float blastForce = 20f;

	public FlickerFader fire1;

	public FlickerFader fire2;

	public FlickerFader fire3;

	public Puff smoke1;

	public Puff smoke2;

	public Puff explosion;

	public Puff explosionSmall;

	public float heightOffGround = 2f;

	protected Unit stuckToUnit;

	protected Vector3 stuckTolocalPos;

	public float bounceYM = 0.3f;

	public float bounceXM = 0.7f;

	public float frictionM = 0.4f;

	public float bounceVolumeM = 1f;

	public bool sticky;

	public bool stickyToUnits;

	protected bool stuckLeft;

	protected bool stuckRight;

	protected bool stuckUp;
}
