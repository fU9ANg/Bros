// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GrenadeStickyPrimary : SachelPack
{
	private bool Stuck
	{
		get
		{
			return this.stuckLeft || this.stuckRight || this.stuckToUnit;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.Stuck)
		{
			this.Animate();
		}
	}

	public override void Fire(float x, float y, float xI, float yI, int playerNum, MonoBehaviour FiredBy)
	{
		base.Fire(x, y, xI, yI, playerNum, FiredBy);
		this.rI = -xI * 2.2f;
	}

	protected override void ApplyGravity()
	{
		this.yI -= 500f * this.t;
	}

	protected override void PlayStuckSound(float v)
	{
		Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.specialAttackSounds, v * 0.6f, base.transform.position, 0.8f);
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
					if (this.raycastHit.collider.GetComponent<SawBlade>() != null)
					{
						this.Death();
					}
					else
					{
						this.stuckLeft = true;
						this.x = this.raycastHit.point.x + this.heightOffGround;
						this.yI = 0f; this.xI = (this.yI );
						this.stickyToUnits = false;
						this.PlayStuckSound(0.3f);
					}
				}
			}
			else if (this.xI > 0f && Physics.Raycast(new Vector3(this.x - 4f, this.y, 0f), Vector3.right, out this.raycastHit, 4f + this.heightOffGround, this.groundLayer))
			{
				if (this.raycastHit.collider.GetComponent<SawBlade>() != null)
				{
					this.Death();
				}
				else
				{
					this.stuckRight = true;
					this.x = this.raycastHit.point.x - this.heightOffGround;
					this.yI = 0f; this.xI = (this.yI );
					this.stickyToUnits = false;
					this.PlayStuckSound(0.3f);
				}
			}
			if (this.yI < 0f)
			{
				if (Physics.Raycast(new Vector3(this.x, this.y + 6f, 0f), Vector3.down, out this.raycastHit, 6f + this.heightOffGround - this.yI * this.t, this.groundLayer))
				{
					this.rI = -this.xI * 3f;
					this.xI *= this.frictionM;
					if (this.yI < -40f)
					{
						this.yI *= -this.bounceYM;
					}
					else
					{
						this.yI = 0f;
						this.y = this.raycastHit.point.y + this.heightOffGround;
						this.rI = 0f;
					}
					base.PlayBounceSound();
				}
			}
			else if (this.yI > 0f && Physics.Raycast(new Vector3(this.x, this.y - 6f, 0f), Vector3.up, out this.raycastHit, 6f + this.heightOffGround + this.yI * this.t, this.groundLayer))
			{
				this.yI *= -(this.bounceYM + 0.1f);
				base.PlayBounceSound();
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
					Networking.RPC(PID.TargetAll, new RpcSignature(base.DetachFromUnit), false);
				}
				else
				{
					base.DetachFromUnit();
				}
			}
			return false;
		}
		return true;
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
					this.PlayStuckSound(0.3f);
				}
			}
			else if (this.xI > 0f && Physics.Raycast(new Vector3(this.x - 6f, this.y, 0f), Vector3.right, out this.raycastHit, 12f, this.groundLayer))
			{
				this.stuckRight = true;
				this.x = this.raycastHit.point.x - this.heightOffGround;
				this.yI = 0f; this.xI = (this.yI );
				this.stickyToUnits = false;
				this.PlayStuckSound(0.3f);
			}
			this.RegisterProjectile();
		}
	}

	protected override void SetRotation()
	{
		if (!this.Stuck)
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, base.transform.eulerAngles.z + this.rI * this.t);
		}
	}

	private void Animate()
	{
		if ((this.animationDelay -= this.t) < 0f)
		{
			this.frame = (this.frame + 1) % 3;
			base.GetComponent<SpriteSM>().SetLowerLeftPixel((float)(16 + 16 * this.frame), 16f);
			this.animationDelay = this.life / 4f + 0.03f;
		}
	}

	protected override void HitProjectiles()
	{
		if (Map.HitProjectiles(this.playerNum, this.damageInternal, this.damageType, this.projectileSize, this.x, this.y, this.xI, this.yI, 0.1f))
		{
		}
	}

	protected float rI = 700f;

	private float animationDelay;

	private int frame;
}
