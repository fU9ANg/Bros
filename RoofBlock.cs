// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RoofBlock : CrateBlock
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		int num = UnityEngine.Random.Range(0, 3);
		foreach (GameObject gameObject in this.attachedObjects)
		{
			SpriteSM component = gameObject.GetComponent<SpriteSM>();
			if (component != null)
			{
				component.SetLowerLeftPixel(new Vector2((float)(num * 32), component.lowerLeftPixel.y));
			}
		}
	}

	protected override void Land()
	{
		base.Land();
		if (this.shakeAmount > 0f)
		{
			SortOfFollow.Shake(this.shakeAmount, base.transform.position);
		}
		if (Physics.Raycast(new Vector3(this.x, this.y + 4f, 0f), Vector3.down, out this.groundHit, 18f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("Disturb");
		}
	}

	protected override void Bounce()
	{
		base.Bounce();
		if (Physics.Raycast(new Vector3(this.x, this.y + 4f, 0f), Vector3.down, out this.groundHit, 18f, this.groundLayer))
		{
			this.groundHit.collider.gameObject.SendMessage("Disturb");
		}
	}

	public override void Push(float xINormalized)
	{
	}

	protected override void MakeEffects()
	{
		if (!this.exploded)
		{
			EffectsController.CreateShrapnel(this.shrapnelPrefab, this.x, this.y, 2f, 150f, (float)(this.shrapnelCount / 3), 0f, this.explodeYForce);
			EffectsController.CreateShrapnel(this.shrapnelBitPrefab, this.x, this.y, 2f, 150f, (float)(this.shrapnelCount / 3), 0f, this.explodeYForce);
			EffectsController.CreateShrapnel(this.shrapnelBitPrefab3, this.x, this.y, 2f, 150f, (float)(this.shrapnelCount / 2), 0f, this.explodeYForce);
			if (this.yI < -120f && UnityEngine.Random.value > 0.5f)
			{
				EffectsController.CreateShrapnel(this.otherShrpnelPrefab, this.x, this.y, 2f, 150f, (float)(this.shrapnelCount / 5 + 1), 0f, -this.yI * 0.6f + this.explodeYForce);
			}
			this.exploded = true;
		}
	}

	protected override void CrumbleBridges(float chance)
	{
		chance = this.collapseChance;
		base.CrumbleBridges(chance);
	}

	public override void CrumbleBridge(float chance)
	{
		if (chance <= 0.7f)
		{
			if (!this.IsSupported())
			{
				chance = 1f;
			}
			else
			{
				chance = 0f;
			}
		}
		if (chance > 0.7f)
		{
			this.health = 0;
			this.collapseChance = chance;
			this.Collapse(0f, 0f, this.collapseChance);
		}
	}

	public override bool IsSupported()
	{
		int num = 3;
		if (!this.canSupportOthers)
		{
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			Block block = Map.GetBlock(this.collumn + i, this.row);
			if (block == null)
			{
				break;
			}
			if (block.health <= 0)
			{
				break;
			}
			if (block.supportedBy != null || block.IsSolid())
			{
				return true;
			}
		}
		for (int j = 0; j < num; j++)
		{
			Block block2 = Map.GetBlock(this.collumn - j, this.row);
			if (block2 == null)
			{
				break;
			}
			if (block2.health <= 0)
			{
				break;
			}
			if (block2.supportedBy != null)
			{
				return true;
			}
		}
		return false;
	}

	public override void StepOn(TestVanDammeAnim unit)
	{
	}

	public override void StepOn(Grenade grenade)
	{
	}

	private bool CanSupporMe(Block block)
	{
		return block.groundType != GroundType.Roof && block.groundType != GroundType.WoodenBlock && block.groundType != GroundType.Bridge && block.groundType != GroundType.Bridge2;
	}

	public bool canSupportOthers = true;

	public float shakeAmount;

	public bool makeDeathSound = true;

	public int shrapnelCount = 15;

	public float explodeYForce;
}
