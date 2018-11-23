// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FallingBlock : Block
{
	public double beingPushedByPlayerTimeStamp { get; set; }

	public double lastSettleTime { get; set; }

	protected override void Start()
	{
		base.Start();
		global::Math.SetupLookupTables();
		this.halfHeight = this.pixelHeight / 2f;
		this.startY = this.y;
		this.beingPushedByPlayerTimeStamp = double.PositiveInfinity;
		this.lastSettleTime = 0.0;
		this.zAngle = (this.zAngleTarget = Mathf.Round(base.transform.eulerAngles.z));
		if (this.zAngle > 180f)
		{
			this.zAngle -= 360f;
			this.zAngleTarget -= 360f;
		}
		this.originalShakeTime = this.shakeTime;
	}

	protected override void Awake()
	{
		if (this.soundHolder == null)
		{
			MonoBehaviour.print("soundholder is null on awake");
		}
		base.Awake();
	}

	public override bool IsSolid()
	{
		return false;
	}

	public override void Collapse(float xI, float yI, float chance)
	{
		if (chance > 0f && chance < this.collapseChance)
		{
			this.collapseChance = chance;
		}
		if (this.health > this.collapseHealthPoint)
		{
			base.DisturbNetworked();
		}
		else
		{
			base.Collapse(xI, yI, chance);
		}
	}

	public override void DestroyBlockInternal(bool CollapseBlocksAround)
	{
		base.DestroyBlockInternal(CollapseBlocksAround);
		if (!this.hasCollapsedSurroundingBlocks)
		{
			this.CollapseSurroundingBlocksOnFall();
		}
	}

	public override void DamageInternal(int damage, float xI, float yI)
	{
		if (damage > this.health || (Mathf.Abs(yI) > Mathf.Abs(xI) && yI < 0f))
		{
			base.DisturbNetworked();
		}
		if (this.health > 0 && this.health - damage > this.collapseHealthPoint)
		{
			this.health -= damage;
		}
		else if (this.health > this.collapseHealthPoint || !this.destroyed)
		{
			this.health -= damage;
			if (this.health <= this.collapseHealthPoint)
			{
				this.Collapse(xI, yI, this.collapseChance);
			}
			else if (!this.disturbed)
			{
				if (xI > 0f)
				{
					this.Push(0.11f / Time.deltaTime * (float)damage);
				}
				else if (xI < 0f)
				{
					this.Push(-0.11f / Time.deltaTime * (float)damage);
				}
			}
		}
	}

	public override bool NotBroken()
	{
		return (!this.disturbed || (!(this.belowBlock == null) && this.belowBlock.NotBroken())) && Mathf.Abs(this.y - this.startY) < 8f && !this.destroyed;
	}

	public override void CollapseForced(float xI, float yI, float chance)
	{
		base.CollapseForced(xI, yI, chance);
	}

	protected override void Update()
	{
		base.Update();
		if (!this.destroyed)
		{
			this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			if (this.rollingLeft)
			{
				if (Mathf.Abs(this.zAngleTarget - this.zAngle) > 55f)
				{
					this.zAngle += this.zAngleI * this.t;
					base.transform.RotateAround(this.rollingPivot, Vector3.forward, this.zAngleI * this.t);
					this.zAngleI += 120f * this.t;
					if (Mathf.Abs(this.zAngleTarget - this.zAngle) <= 55f)
					{
						this.RollOnToUnits();
					}
				}
				else
				{
					this.zAngle += this.zAngleI * this.t;
					base.transform.RotateAround(this.rollingPivot, Vector3.forward, this.zAngleI * this.t);
					this.zAngleI += 1200f * this.t;
				}
				if (this.zAngle > this.zAngleTarget)
				{
					this.FinishRoll(this.zAngleTarget);
				}
			}
			else if (this.rollingRight)
			{
				if (Mathf.Abs(this.zAngleTarget - this.zAngle) > 55f)
				{
					this.zAngle += this.zAngleI * this.t;
					base.transform.RotateAround(this.rollingPivot, Vector3.forward, this.zAngleI * this.t);
					this.zAngleI -= 120f * this.t;
					if (Mathf.Abs(this.zAngleTarget - this.zAngle) <= 55f)
					{
						this.RollOnToUnits();
					}
				}
				else
				{
					this.zAngle += this.zAngleI * this.t;
					base.transform.RotateAround(this.rollingPivot, Vector3.forward, this.zAngleI * this.t);
					this.zAngleI -= 1200f * this.t;
				}
				if (this.zAngle < this.zAngleTarget)
				{
					this.FinishRoll(this.zAngleTarget);
				}
			}
			else if (this.disturbed)
			{
				if (this.shakeTime > 0f)
				{
					this.shakeCounter += this.t * 60f;
					this.shakeTime -= this.t;
					if (this.shakeTime > 0.15f)
					{
						this.SetPosition(global::Math.Sin(this.shakeCounter) * 1f);
					}
					else
					{
						this.SetPosition(0f);
					}
					if (this.shakeTime <= 0f)
					{
						this.StartFalling();
					}
				}
				else
				{
					if (this.yI < -40f)
					{
						this.HitUnits();
					}
					this.yI -= 1000f * this.t;
					float num = this.yI * this.t;
					if (this.ClampToGround(ref num))
					{
						if (this.yI < -200f)
						{
							this.y += num;
							this.Bounce();
						}
						else
						{
							this.y += num;
							this.Land();
						}
					}
					else
					{
						this.y += num;
					}
					if (this.y < -44f)
					{
						this.health = 0;
						this.Collapse(0f, 0f, 1f);
					}
					this.SetPosition(0f);
				}
			}
			else if (this.invulnerable)
			{
				this.invulnerableTime -= this.t;
				if (this.invulnerableTime <= 0f)
				{
					this.invulnerable = false;
				}
			}
		}
	}

	protected virtual void StartFalling()
	{
		base.HideAbove();
		this.ClearBlock();
		this.CollapseSurroundingBlocksOnFall();
	}

	protected virtual void ClearBlock()
	{
		Map.SetBlockEmpty(this, this.collumn, this.row);
	}

	protected virtual void RollOnToUnits()
	{
		if (this.rollingLeft)
		{
			Map.RollOntoUnits(this.collumn - 1, this.row, -1);
		}
		if (this.rollingRight)
		{
			Map.RollOntoUnits(this.collumn + 1, this.row, 1);
		}
	}

	protected virtual void CollapseSurroundingBackgroundBlocks()
	{
		Block backgroundBlock = Map.GetBackgroundBlock(this.collumn, this.row);
		if (backgroundBlock != null)
		{
			backgroundBlock.Collapse(0f, 0f, 0f);
		}
		backgroundBlock = Map.GetBackgroundBlock(this.collumn, this.row + 1);
		if (backgroundBlock != null)
		{
			backgroundBlock.Collapse(0f, 0f, 0f);
		}
		backgroundBlock = Map.GetBackgroundBlock(this.collumn - 1, this.row);
		if (backgroundBlock != null)
		{
			backgroundBlock.Collapse(0f, 0f, 0f);
		}
		backgroundBlock = Map.GetBackgroundBlock(this.collumn + 1, this.row);
		if (backgroundBlock != null)
		{
			backgroundBlock.Collapse(0f, 0f, 0f);
		}
	}

	protected virtual void CollapseSurroundingBlocksOnFall()
	{
		this.hasCollapsedSurroundingBlocks = true;
		Block block = Map.GetBlock(this.collumn, this.row + 1);
		if (block != null)
		{
			block.Collapse(0f, 0f, 0f);
		}
		if (Physics.Raycast(base.transform.position, Vector3.up, out this.groundHit, 24f, this.groundLayer))
		{
			Cage component = this.groundHit.collider.gameObject.GetComponent<Cage>();
			if (component != null)
			{
				component.Collapse(0f, 0f, 1f);
			}
			CageBar component2 = this.groundHit.collider.gameObject.GetComponent<CageBar>();
			if (component2 != null)
			{
				component2.Collapse(0f, 0f, 1f);
			}
		}
		if (this.collapseBackgroundBlocks)
		{
			this.CollapseSurroundingBackgroundBlocks();
		}
		if (Physics.Raycast(base.transform.position, Vector3.right, out this.groundHit, 24f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("CrumbleBridge", this.collapseChance * 0.5f, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			Block block2 = Map.GetBlock(this.collumn + 1, this.row);
			if (block2 != null)
			{
				block2.CrumbleBridge(this.collapseChance * 0.5f);
			}
		}
		if (Physics.Raycast(base.transform.position, Vector3.left, out this.groundHit, 24f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("CrumbleBridge", this.collapseChance * 0.5f, SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			Block block3 = Map.GetBlock(this.collumn - 1, this.row);
			if (block3 != null)
			{
				block3.CrumbleBridge(this.collapseChance * 0.5f);
			}
		}
	}

	protected override void DelayedCollapseAbove()
	{
		this.collapseChance = 0f;
		base.DelayedCollapseAbove();
	}

	protected virtual void HitUnits()
	{
		Map.HitUnits(this, 20, DamageType.Crush, 6f, this.x, this.y - this.halfHeight - 4f, 0f, this.yI, true, false);
		if (Map.DamageDoodads(5, this.x, this.y, 0f, this.yI, 4f, 5))
		{
			this.yI *= 0.5f;
		}
	}

	protected virtual void Bounce()
	{
		SortOfFollow.Shake(0.2f, base.transform.position);
		this.yI *= -0.25f;
		this.PlayHitSound();
	}

	protected virtual void PlayHitSound()
	{
		Sound instance = Sound.GetInstance();
		instance.PlaySoundEffectAt(this.soundHolder.hitSounds, 0.3f, base.transform.position);
	}

	protected virtual void Land()
	{
		this.yI = 0f;
		this.startY = this.y;
		this.row = Map.GetRow(this.y);
		this.collumn = Map.GetCollumn(this.x);
		Map.AssignBlock(this, this.collumn, this.row);
		this.disturbed = false;
		this.shakeTime = 0.1f;
		if (this.landDamage > 0)
		{
			Block block = Map.GetBlock(this.collumn, this.row - 1);
			if (block != null)
			{
				block.Damage(new DamageObject(this.landDamage, DamageType.Crush, 0f, 0f, null));
			}
		}
	}

	protected virtual bool ClampToGround(ref float yIT)
	{
		if (Physics.Raycast(new Vector3(this.x, this.y + 4f, 0f), Vector3.down, out this.groundHit, this.pixelHeight, this.groundLayer) && this.y + yIT - this.halfHeight < this.groundHit.point.y)
		{
			yIT = this.groundHit.point.y - (this.y - this.halfHeight);
			return true;
		}
		return false;
	}

	public override void CrumbleBridge(float chance)
	{
		if (chance > 0.4f)
		{
			this.collapseChance = chance;
			this.Collapse(0f, 0f, this.collapseChance * 0.8f);
		}
	}

	public override void StepOn(TestVanDammeAnim unit)
	{
		if (((unit.IsMine && !unit.IsEnemy) || unit.IsEnemy) && !this.disturbed && !this.invulnerable && !Map.isEditing)
		{
			this.StepOnBlock();
		}
	}

	public override void StepOn(Grenade grenade)
	{
		if (grenade.IsMine && !this.disturbed && !this.invulnerable && !Map.isEditing)
		{
			this.StepOnBlock();
		}
	}

	public override void StepOnBlock()
	{
		if (!this.disturbed)
		{
			this.shakeTime = this.originalShakeTime * 1.425f;
		}
		base.DisturbNetworked();
	}

	public override void StepOnBlockForced()
	{
		this.invulnerable = false;
		this.invulnerableTime = 0f;
		if (!this.disturbed)
		{
			this.shakeTime = this.originalShakeTime * 1.425f;
		}
		base.DisturbNetworked();
	}

	public override void Disturb()
	{
		if (!this.disturbed && !this.invulnerable && !this.IsBelowBlockSolid())
		{
			this.shakeTime *= 0.7f;
			this.disturbed = true;
			Sound instance = Sound.GetInstance();
			instance.PlaySoundEffectAt(this.soundHolder.effortSounds, 0.3f, base.transform.position);
		}
	}

	public override bool NotRolling()
	{
		return !this.rollingLeft && !this.rollingRight;
	}

	protected bool IsBelowBlockSolid()
	{
		if (!Physics.Raycast(new Vector3(this.x, this.y + 4f, 0f), Vector3.down, out this.groundHit, 16f, this.groundLayer))
		{
			return false;
		}
		Block component = this.groundHit.collider.GetComponent<Block>();
		if (component == null)
		{
			DoodadDestroyable component2 = this.groundHit.collider.GetComponent<DoodadDestroyable>();
			return !(component2 != null) || !component2.Falling;
		}
		return component.groundType != GroundType.Ladder && component.NotBroken() && component.NotRolling();
	}

	protected void SetPosition(float xOffset)
	{
		base.transform.position = new Vector3(Mathf.Round(this.x), Mathf.Round(this.y), 0f);
		if (this.sprite != null)
		{
			this.sprite.SetOffset(this.spriteOffset + new Vector3(Mathf.Round(xOffset), 0f, 0f));
		}
	}

	protected void SetRotation()
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, this.zAngle);
	}

	public void SetRotation(float newAngle)
	{
		this.zAngle = newAngle;
		this.SetRotation();
	}

	public int Rotation
	{
		get
		{
			return (int)this.zAngle;
		}
	}

	public override void Push(float xINormalized)
	{
		if (!this.rollingLeft && !this.rollingRight)
		{
			this.pushForce += xINormalized * this.t;
			if (this.pushForce > 0.1f)
			{
				if (!Map.IsBlockSolid(this.collumn + 1, this.row))
				{
					if (Map.CanRollOntoUnits(this.collumn + 1, this.row, 1))
					{
						this.RollOver(1);
					}
				}
				else
				{
					this.pushForce = 0f;
				}
			}
			else if (this.pushForce < -0.1f)
			{
				if (!Map.IsBlockSolid(this.collumn - 1, this.row))
				{
					if (Map.CanRollOntoUnits(this.collumn - 1, this.row, -1))
					{
						this.RollOver(-1);
					}
				}
				else
				{
					this.pushForce = 0f;
				}
			}
		}
	}

	public override void Rotate(int direction)
	{
		this.zAngle += (float)(direction * 90);
		this.zAngleTarget = this.zAngle;
		this.SetRotation();
	}

	protected virtual void RollOver(int direction)
	{
		if (this.beingPushedByPlayer == PID.NoID)
		{
			MapController.SendRollBlockRPC(this, direction);
		}
		else
		{
			MonoBehaviour.print("Cannot push block becaues another player is currently pushing it " + this.beingPushedByPlayer);
		}
	}

	protected virtual void ClearBlockOnRoll()
	{
		this.ClearBlock();
		Block block = Map.GetBlock(this.collumn, this.row + 1);
		if (block != null)
		{
			UnityEngine.Debug.Log("Collapse Above block");
			block.Collapse(0f, 0f, 0f);
		}
	}

	[RPC]
	public void RollOverRPC(int direction)
	{
		this.pushForce = 0f;
		if (direction > 0)
		{
			this.rollingLeft = false;
			this.rollingRight = true;
			this.zAngleI = -420f;
			this.zAngleTarget -= 90f;
			this.rollingPivot = new Vector3(this.x + 8f, this.y - 8f, 0f);
			this.ClearBlockOnRoll();
		}
		else if (direction < 0)
		{
			this.rollingLeft = true;
			this.rollingRight = false;
			this.zAngleI = 420f;
			this.zAngleTarget += 90f;
			this.rollingPivot = new Vector3(this.x - 8f, this.y - 8f, 0f);
			this.ClearBlockOnRoll();
		}
		this.SetPosition(0f);
	}

	private void Settle(float X, float Y, int colOffset, float final_zAngle)
	{
		this.rollingRight = false;
		this.rollingLeft = false;
		if (Map.GetBlock(this.collumn, this.row) == this)
		{
			Map.SetBlockEmpty(this, this.collumn, this.row);
		}
		this.x = X;
		this.y = Y;
		this.zAngle = final_zAngle;
		this.SetPosition(0f);
		this.SetRotation();
		Map.GetRowCollumn(this.x, this.y, ref this.row, ref this.collumn);
		if (this.IsBelowBlockSolid() || (this.height == 2 && Map.IsBlockSolid(this.collumn + colOffset, this.row - 1)))
		{
			SortOfFollow.Shake(0.1f, base.transform.position);
			Map.AssignBlock(this, this.collumn, this.row);
			Sound instance = Sound.GetInstance();
			instance.PlaySoundEffectAt(this.soundHolder.hitSounds, 0.15f, base.transform.position);
			if (this.height == 2 && Map.IsBlockSolid(this.collumn - colOffset, this.row))
			{
				Map.PushBlock(this.collumn - colOffset, this.row, -1000f);
			}
		}
		else
		{
			this.yI = -100f;
			this.disturbed = true;
			this.shakeTime = -1f;
		}
	}

	public void SettleBlockAfterRoll(float X, float Y, int colOffset, float final_zAngle, double photonTimeStamp, PID sender)
	{
		if (photonTimeStamp < this.beingPushedByPlayerTimeStamp)
		{
			return;
		}
		this.lastSettleTime = photonTimeStamp;
		this.Settle(X, Y, colOffset, final_zAngle);
		if (this.beingPushedByPlayer == sender)
		{
			this.beingPushedByPlayer = PID.NoID;
			this.beingPushedByPlayerTimeStamp = double.PositiveInfinity;
		}
	}

	protected virtual void FinishRoll(float final_zAngle)
	{
		this.zAngle = final_zAngle;
		if (this.rollingLeft)
		{
			this.x += -16f;
			if (this.beingPushedByPlayer == PID.MyID)
			{
				Networking.RPC<FallingBlock, float, float, int, float>(PID.TargetOthers, new RpcSignature<FallingBlock, float, float, int, float>(MapController.SettleBlockRPC), this, this.x, this.y, -1, final_zAngle, false);
				this.beingPushedByPlayer = PID.NoID;
				this.beingPushedByPlayerTimeStamp = double.PositiveInfinity;
			}
			this.Settle(this.x, this.y, -1, final_zAngle);
		}
		else if (this.rollingRight)
		{
			this.x += 16f;
			if (this.beingPushedByPlayer == PID.MyID)
			{
				Networking.RPC<FallingBlock, float, float, int, float>(PID.TargetOthers, new RpcSignature<FallingBlock, float, float, int, float>(MapController.SettleBlockRPC), this, this.x, this.y, 1, final_zAngle, false);
				this.beingPushedByPlayer = PID.NoID;
				this.beingPushedByPlayerTimeStamp = double.PositiveInfinity;
			}
			this.Settle(this.x, this.y, 1, final_zAngle);
		}
	}

	public Shrapnel otherShrpnelPrefab;

	protected float startY;

	protected float shakeCounter;

	public float shakeTime = 0.5f;

	protected float originalShakeTime = 0.5f;

	public float pixelHeight = 16f;

	protected float halfHeight = 8f;

	protected float invulnerableTime = 0.5f;

	protected bool invulnerable = true;

	public int collapseHealthPoint = -15;

	public int landDamage = 1;

	protected float t = 0.01f;

	public PID beingPushedByPlayer = PID.NoID;

	public bool collapseBackgroundBlocks = true;

	protected bool hasCollapsedSurroundingBlocks;

	protected float zAngle;

	protected float zAngleI;

	protected float zAngleTarget;

	protected bool rollingLeft;

	protected bool rollingRight;

	protected Vector3 rollingPivot = Vector3.zero;
}
