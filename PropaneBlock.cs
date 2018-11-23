// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PropaneBlock : BarrelBlock
{
	protected override void Start()
	{
		base.Start();
		this.startZAngle = this.zAngle;
		if (this.startZAngle != 0f)
		{
			if (this.startZAngle > 0f && Map.GetBlock(this.collumn - 1, this.row) != this && !Map.AssignBlock(this, this.collumn - 1, this.row))
			{
				UnityEngine.Debug.LogError("Failed Assigning Top Propane Block Right " + Map.GetBlock(this.collumn - 1, this.row).name);
			}
			if (this.startZAngle < 0f && Map.GetBlock(this.collumn + 1, this.row) != this && !Map.AssignBlock(this, this.collumn + 1, this.row))
			{
				UnityEngine.Debug.LogError("Failed Assigning Top Propane Block Left " + Map.GetBlock(this.collumn + 1, this.row).name);
			}
		}
		else if (!Map.AssignBlock(this, this.collumn, this.row + 1))
		{
			UnityEngine.Debug.LogError("Failed Assigning Top Propane Block");
		}
	}

	public override void Collapse(float xI, float yI, float chance)
	{
		if (!this.flying)
		{
			if (this.health > this.collapseHealthPoint && chance < 1f)
			{
				base.Collapse(xI, yI, chance);
			}
			else
			{
				this.Launch();
			}
		}
		else if (chance >= 1f)
		{
			this.health = this.collapseHealthPoint;
			base.Collapse(xI, yI, chance);
		}
	}

	protected override void StartFalling()
	{
		base.StartFalling();
	}

	protected override void ClearBlock()
	{
		if (!this.clearedBlocks)
		{
			this.clearedBlocks = true;
			Map.SetBlockEmpty(this, this.collumn, this.row);
			if (base.transform.localEulerAngles.z == 0f)
			{
				Map.SetBlockEmpty(this, this.collumn, this.row + 1);
			}
			else if (this.startZAngle != 0f)
			{
				Block block = Map.GetBlock(this.collumn, this.row + 1);
				if (block != null)
				{
					block.DisturbNetworked();
				}
				if (this.startZAngle > 0f)
				{
					if (!Map.SetBlockEmpty(this, this.collumn - 1, this.row))
					{
						UnityEngine.Debug.LogError("Unable to clear block");
					}
					Block block2 = Map.GetBlock(this.collumn - 1, this.row + 1);
					if (block2 != null)
					{
						block2.DisturbNetworked();
					}
				}
				else
				{
					if (!Map.SetBlockEmpty(this, this.collumn + 1, this.row))
					{
						UnityEngine.Debug.LogError("Unable to clear block");
					}
					Block block3 = Map.GetBlock(this.collumn + 1, this.row + 1);
					if (block3 != null)
					{
						block3.DisturbNetworked();
					}
				}
			}
		}
	}

	protected override void ClearBlockOnRoll()
	{
		Map.SetBlockEmpty(this, this.collumn, this.row);
		Map.SetBlockEmpty(this, this.collumn, this.row + 1);
		this.clearedBlocks = true;
		Block block = Map.GetBlock(this.collumn, this.row + 2);
		if (block != null)
		{
			block.Collapse(0f, 0f, 0f);
		}
	}

	public override void Rotate(int direction)
	{
		if (this.zAngle == 0f)
		{
			bool flag = false;
			if (direction < 0)
			{
				if (Map.GetBlock(this.collumn + 1, this.row) == null && Map.AssignBlock(this, this.collumn + 1, this.row))
				{
					flag = true;
				}
			}
			else if (Map.GetBlock(this.collumn - 1, this.row) == null && Map.AssignBlock(this, this.collumn - 1, this.row))
			{
				flag = true;
			}
			if (flag)
			{
				if (Map.GetBlock(this.collumn, this.row + 1) == this)
				{
					Map.SetBlockEmpty(this, this.collumn, this.row + 1);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Block Rotation is not this advanced yet, please make room for the Propane Block");
			}
			base.Rotate(direction);
			this.startZAngle = this.zAngle;
		}
		else
		{
			UnityEngine.Debug.LogError("Block Rotation is not this advanced yet");
		}
	}

	public void Launch()
	{
		if (!this.flying)
		{
			Networking.RPC(PID.TargetOthers, new RpcSignature(this.Launch), false);
			this.flying = true;
			this.flyingTime = 1.25f;
			this.flyingCounter = 0f;
			this.flyingSpeed = 40f;
			this.ClearBlock();
			if (this.blastSound == null)
			{
				this.blastSound = base.gameObject.AddComponent<AudioSource>();
				this.blastSound.clip = this.soundHolder.specialSounds[UnityEngine.Random.Range(0, this.soundHolder.specialSounds.Length)];
				this.blastSound.loop = true;
				this.blastSound.volume = 0.01f;
				this.blastSound.rolloffMode = AudioRolloffMode.Linear;
				this.blastSound.maxDistance = 200f;
				this.blastSound.dopplerLevel = 0.12f;
				this.blastSound.pitch = 0.5f;
				this.blastSound.Play();
			}
		}
	}

	protected override void Land()
	{
		this.row = Map.GetRow(this.y);
		this.collumn = Map.GetCollumn(this.x);
		this.disturbed = false;
		SortOfFollow.Shake(0.2f, base.transform.position);
		this.DamageInternal(1, 0f, 0f);
	}

	protected override void FinishRoll(float final_zAngle)
	{
		base.FinishRoll(final_zAngle);
		if (!this.flying && this.burnTime <= 0f)
		{
			this.forceExplosions = true;
			this.DamageInternal(1, 0f, 0f);
		}
		else if (!this.flying && this.burnTime > 0f)
		{
			this.Collapse(0f, 0f, 1f);
		}
	}

	public override void Push(float xINormalized)
	{
		if (!this.flying && this.zAngle == 0f)
		{
			base.Push(xINormalized);
		}
	}

	protected override void Update()
	{
		if (this.disturbed && !this.flying && SetResolutionCamera.IsBelowScreen(base.transform.position))
		{
			this.Launch();
		}
		if (this.flying)
		{
			this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			this.flyingTime -= this.t;
			if (this.flyingTime > 0f)
			{
				this.flyingSpeed += 300f * this.t;
				if (this.blastSound != null)
				{
					this.blastSound.pitch = 0.2f + this.flyingSpeed / 200f;
					this.blastSound.volume = Mathf.Lerp(this.blastSound.volume, Mathf.Clamp(0.3f + this.flyingSpeed / 300f, 0f, 0.7f), Time.deltaTime * 4f);
				}
				Vector3 vector = base.transform.TransformDirection(0f, this.flyingSpeed, 0f);
				this.yI = vector.y;
				this.xI = vector.x;
				this.y += this.yI * this.t;
				this.x += this.xI * this.t;
				Vector3 direction = base.transform.TransformDirection(0f, 1f, 0f);
				if (MapController.DamageGround(this, 10, DamageType.Crush, 14f, this.x + direction.x * 20f, this.y + direction.y * 20f, new Collider[]
				{
					base.GetComponent<Collider>()
				}))
				{
					this.flyingSpeed *= 0.9f;
					if (this.flyingSpeed < 40f)
					{
						this.flyingSpeed = 40f;
					}
				}
				if (Physics.Raycast(new Vector3(this.x, this.y, 0f), direction, out this.groundHit, 90.5f, 1 << LayerMask.NameToLayer("MobileBarriers")))
				{
					MammothKopter component = this.groundHit.collider.GetComponent<MammothKopter>();
					if (component != null)
					{
						Networking.RPC(PID.TargetAll, new RpcSignature(component.PropaneHitWarning), true);
					}
				}
				if (Physics.Raycast(new Vector3(this.x, this.y, 0f), direction, out this.groundHit, 24.5f, 1 << LayerMask.NameToLayer("MobileBarriers")))
				{
					MammothKopter component2 = this.groundHit.collider.GetComponent<MammothKopter>();
					if (component2 != null)
					{
						Networking.RPC<PropaneBlock>(PID.TargetAll, new RpcSignature<PropaneBlock>(component2.PropaneHit), this, true);
						this.Collapse(0f, 0f, 1f);
					}
				}
				if (Physics.Raycast(new Vector3(this.x, this.y, 0f), direction, out this.groundHit, 24.5f, this.groundLayer) && this.groundHit.collider.tag == "Metal")
				{
					this.Collapse(0f, 0f, 1f);
				}
				Vector3 vector2 = base.transform.TransformDirection(0f, 18f, 0f);
				if (Mathf.Abs(this.xI) > Mathf.Abs(this.yI))
				{
					Map.CrushUnitsAgainstWalls(this, this.x + vector2.x, this.y + vector2.y, 7f, (int)Mathf.Sign(this.xI), 0);
				}
				else
				{
					Map.CrushUnitsAgainstWalls(this, this.x + vector2.x, this.y + vector2.y, 7f, 0, (int)Mathf.Sign(this.yI));
				}
				this.flyingCounter += this.t;
				this.RunWarning(this.t, this.flyingTime);
				if (this.flyingCounter > 0.045f)
				{
					this.flyingCounter -= 0.045f;
					Vector3 vector3 = UnityEngine.Random.insideUnitCircle;
					switch (UnityEngine.Random.Range(0, 3))
					{
					case 0:
						EffectsController.CreateEffect(this.fire1, this.x + vector3.x * 6f, this.y + vector3.y * 9f - 2f, UnityEngine.Random.value * 0.0434f, Vector3.zero);
						break;
					case 1:
						EffectsController.CreateEffect(this.fire2, this.x + vector3.x * 6f, this.y + vector3.y * 9f - 2f, UnityEngine.Random.value * 0.0434f, Vector3.zero);
						break;
					case 2:
						EffectsController.CreateEffect(this.fire3, this.x + vector3.x * 6f, this.y + vector3.y * 9f - 2f, UnityEngine.Random.value * 0.0434f, Vector3.zero);
						break;
					}
				}
				this.shakeCounter += this.t * 60f;
				base.SetPosition(global::Math.Sin(this.shakeCounter) * 1f);
			}
			else
			{
				this.Collapse(0f, 0f, 1f);
			}
		}
		else
		{
			base.Update();
		}
	}

	protected override void RollOnToUnits()
	{
		if (this.rollingLeft)
		{
			Map.HitUnits(this, 50, DamageType.Crush, 8f, this.x - 16f, this.y, 0f, -100f, true, false);
		}
		else if (this.rollingRight)
		{
			Map.HitUnits(this, 50, DamageType.Crush, 8f, this.x + 16f, this.y, 0f, -100f, true, false);
		}
	}

	protected override void RollOver(int direction)
	{
		if (!this.rolled)
		{
			this.rolled = true;
			base.RollOver(direction);
		}
	}

	protected override void RunWarning(float t, float explosionTime)
	{
		if (this.flying)
		{
			base.RunWarning(t, explosionTime);
		}
	}

	protected override bool ClampToGround(ref float yIT)
	{
		if (Physics.Raycast(new Vector3(this.x, this.y + 4f, 0f), Vector3.down, out this.groundHit, this.pixelHeight, this.groundLayer) && this.y + yIT - this.halfHeight < this.groundHit.point.y)
		{
			yIT = this.groundHit.point.y - (this.y - this.halfHeight);
			return true;
		}
		Vector3 b = base.transform.TransformDirection(0f, 12f, 0f);
		if (Physics.Raycast(base.transform.position + b, Vector3.down, out this.groundHit, this.pixelHeight, this.groundLayer) && this.y + yIT - this.halfHeight < this.groundHit.point.y)
		{
			yIT = this.groundHit.point.y - (this.y - this.halfHeight);
			return true;
		}
		return false;
	}

	protected bool flying;

	public float flyingTime;

	protected float flyingCounter;

	protected float flyingSpeed;

	protected AudioSource blastSound;

	protected bool rolled;

	protected float startZAngle;

	protected bool clearedBlocks;
}
