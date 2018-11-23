// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TankBig : Tank
{
	protected override void Start()
	{
		base.Start();
		this.width = 24f;
		this.height = 20f;
		int playerAliveNum = HeroController.GetPlayerAliveNum();
		if (playerAliveNum > 1)
		{
			this.health = (int)((float)this.health * (1f + (float)(playerAliveNum - 1) * 0.33f));
		}
	}

	protected override void HitUnits()
	{
		this.invulnerable = true;
		if (Map.HitUnits(this, 20, DamageType.Crush, 30f, 2f, this.x - 8f, this.y - 8f, 0f, this.yI, true, false))
		{
		}
		this.invulnerable = false;
	}

	public override void Death(float xI, float yI, DamageObject damage)
	{
		base.Death(xI, yI, damage);
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void GetGroundHeight()
	{
		this.groundHeight = -200f;
		this.leftGround = false;
		this.rightGround = false;
		this.midLeftGround = false;
		this.midRightGround = false;
		this.farLeftGround = false;
		this.farRightGround = false;
		if (Physics.Raycast(new Vector3(this.x + 40f, this.y + 6f, 0f), Vector3.down, out this.raycastHitFarRight, 64f, this.groundLayer))
		{
			if (this.raycastHitFarRight.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitFarRight.point.y;
			}
			if (this.raycastHitFarRight.point.y > this.y - 9f)
			{
				this.farRightGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x - 40f, this.y + 6f, 0f), Vector3.down, out this.raycastHitFarLeft, 64f, this.groundLayer))
		{
			if (this.raycastHitFarLeft.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitFarLeft.point.y;
			}
			if (this.raycastHitFarLeft.point.y > this.y - 9f)
			{
				this.farLeftGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x - 8f, this.y + 6f, 0f), Vector3.down, out this.raycastHitMidLeft, 64f, this.groundLayer))
		{
			if (this.raycastHitMidLeft.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMidLeft.point.y;
			}
			if (this.raycastHitMidLeft.point.y > this.y - 9f)
			{
				this.midLeftGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x + 8f, this.y + 6f, 0f), Vector3.down, out this.raycastHitMidRight, 64f, this.groundLayer))
		{
			if (this.raycastHitMidRight.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitMidRight.point.y;
			}
			if (this.raycastHitMidRight.point.y > this.y - 9f)
			{
				this.midRightGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x + 24f, this.y + 6f, 0f), Vector3.down, out this.raycastHitRight, 64f, this.groundLayer))
		{
			if (this.raycastHitRight.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitRight.point.y;
			}
			if (this.raycastHitRight.point.y > this.y - 9f)
			{
				this.rightGround = true;
			}
		}
		if (Physics.Raycast(new Vector3(this.x - 24f, this.y + 6f, 0f), Vector3.down, out this.raycastHitLeft, 64f, this.groundLayer))
		{
			if (this.raycastHitLeft.point.y > this.groundHeight)
			{
				this.groundHeight = this.raycastHitLeft.point.y;
			}
			if (this.raycastHitLeft.point.y > this.y - 9f)
			{
				this.leftGround = true;
			}
		}
		if (!Map.isEditing && ((!this.leftGround) ? 1 : 0) + ((!this.midLeftGround) ? 1 : 0) + ((!this.midRightGround) ? 1 : 0) + ((!this.rightGround) ? 1 : 0) + ((!this.farRightGround) ? 1 : 0) + ((!this.farLeftGround) ? 1 : 0) >= 3)
		{
			this.DamageGroundBelow(true);
		}
	}

	public override void Damage(int damage, DamageType damageType, float xI, float yI, int direction, MonoBehaviour damageSender, float hitX, float hitY)
	{
		if (this.health > 0)
		{
			this.damageBrotalityCount += damage;
			StatisticsController.AddBrotality(this.damageBrotalityCount / 5);
			this.damageBrotalityCount -= this.damageBrotalityCount / 5;
		}
		base.Damage(damage, damageType, xI, yI, direction, damageSender, hitX, hitY);
		if (this.health > 0 && this.weapon.health > this.health)
		{
			this.weapon.health = this.health;
		}
	}

	protected override void DamageGroundBelow(bool forced)
	{
		base.DamageGroundBelow(forced);
		if (forced && this.farRightGround && this.raycastHitFarRight.collider != null)
		{
			this.raycastHitFarRight.collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
		}
		if (forced && this.farLeftGround && this.raycastHitFarLeft.collider != null)
		{
			this.raycastHitFarLeft.collider.gameObject.SendMessage("Damage", new DamageObject(5, DamageType.Crush, 0f, 0f, null), SendMessageOptions.DontRequireReceiver);
		}
	}

	protected override void ConstrainToBlocks(int direction, ref float xIT)
	{
		if (direction != 0)
		{
			if (base.ConstrainToBlock(ref xIT, this.x - (float)(8 * direction), this.y + 86f, (direction <= 0) ? Vector3.left : Vector3.right, 40f, 36f, true))
			{
			}
			if (base.ConstrainToBlock(ref xIT, this.x - (float)(8 * direction), this.y + 70f, (direction <= 0) ? Vector3.left : Vector3.right, 54f, 48f, true))
			{
			}
			if (base.ConstrainToBlock(ref xIT, this.x - (float)(8 * direction), this.y + 54f, (direction <= 0) ? Vector3.left : Vector3.right, 54f, 48f, true))
			{
			}
			if (base.ConstrainToBlock(ref xIT, this.x - (float)(8 * direction), this.y + 38f, (direction <= 0) ? Vector3.left : Vector3.right, 54f, 48f, true))
			{
			}
			if (base.ConstrainToBlock(ref xIT, this.x - (float)(8 * direction), this.y + 22f, (direction <= 0) ? Vector3.left : Vector3.right, 54f, 48f, true))
			{
			}
			if (base.ConstrainToBlock(ref xIT, this.x - (float)(8 * direction), this.y + 8f, (direction <= 0) ? Vector3.left : Vector3.right, 54f, 48f, true))
			{
			}
		}
	}

	protected RaycastHit raycastHitFarRight;

	protected RaycastHit raycastHitFarLeft;

	protected bool farLeftGround;

	protected bool farRightGround;

	protected int damageBrotalityCount;
}
