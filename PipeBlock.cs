// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PipeBlock : BrickBlock
{
	protected bool IsVolatile
	{
		get
		{
			return this._isVolatile;
		}
		set
		{
			this._isVolatile = value;
		}
	}

	public override void ShowForeground(bool isEarth, bool isBrick, bool onlyEdges)
	{
		base.ShowForeground(isEarth, isBrick, onlyEdges);
		if (!Map.IsBlockSolid(this.collumn, this.row + 1) && UnityEngine.Random.value > 0.85f && this.aboveDecorationPrefabs.Length > 0)
		{
			this.addedObjects.Add(this.AddForegroundDecorationPiece(this.aboveDecorationPrefabs, 3f));
		}
		if (((!this.bottomSolidAtStart) ? 0 : 1) + ((!this.topSolidAtStart) ? 0 : 1) + ((!this.rightSolidAtStart) ? 0 : 1) + ((!this.leftSolidAtStart) ? 0 : 1) == 2 && ((this.topRightSolidAtStart && this.topSolidAtStart && this.rightSolidAtStart) || (this.topLeftSolidAtStart && this.topSolidAtStart && this.leftSolidAtStart) || (this.bottomLeftSolidAtStart && this.bottomSolidAtStart && this.leftSolidAtStart) || (this.bottomRightSolidAtStart && this.bottomSolidAtStart && this.rightSolidAtStart)) && UnityEngine.Random.value > 0.25f && this.machinePartsPrefabs.Length > 0)
		{
			this.addedObjects.Add(this.AddForegroundDecorationPiece(this.machinePartsPrefabs, -2f));
		}
	}

	public void SetVolatile(GameObject volatileAgent)
	{
		if (volatileAgent != null && this.volatileAgent != null)
		{
			float sqrMagnitude = (this.volatileAgent.transform.position - base.transform.position).sqrMagnitude;
			float sqrMagnitude2 = (volatileAgent.transform.position - base.transform.position).sqrMagnitude;
			if (sqrMagnitude2 < sqrMagnitude)
			{
				this.volatileAgent = volatileAgent;
			}
		}
		if (this.volatileAgent == null)
		{
			this.volatileAgent = volatileAgent;
		}
		if (this.volatileAgent != null)
		{
			this._isVolatile = true;
		}
	}

	public override void DamageInternal(int damage, float xI, float yI)
	{
		if (this.health <= 0)
		{
			this.Collapse(xI, yI, 1f);
		}
		base.DamageInternal(damage, xI, yI);
	}

	public override void Collapse(float xI, float yI, float chance)
	{
		if (chance < 1f)
		{
			return;
		}
		if ((this.hasBeenDamaged || !this.IsVolatile) && !this.exploded)
		{
			base.Collapse(xI, yI, chance);
		}
		else
		{
			this.hasBeenDamaged = true;
			this.explodeDelay = this.explodeDelayTime;
		}
	}

	protected virtual void PlayEffortSound()
	{
		if (this.sound == null)
		{
			this.sound = Sound.GetInstance();
		}
		if (this.sound != null)
		{
			this.sound.PlaySoundEffectAt(this.soundHolder.effortSounds, this.soundVolume * 0.6f, base.transform.position);
		}
	}

	protected override void EffectsCollapse(float xI, float yI)
	{
		if (!this.IsVolatile)
		{
			EffectsController.CreateDirtParticles(this.x, this.y, 20, 5f, 50f, 0f, 0f);
			this.PlayEffortSound();
		}
		else
		{
			this.Explode();
		}
	}

	protected override void EffectsDestroyed(float xI, float yI, float force)
	{
		if (!this.IsVolatile)
		{
			EffectsController.CreateDirtParticles(this.x, this.y, 25, 5f, force, 0f, 40f);
			this.PlayEffortSound();
		}
		else
		{
			this.Explode();
		}
	}

	protected void Explode()
	{
		if (!this.exploded)
		{
			this.exploded = true;
			EffectsController.CreateExplosion(this.x, this.y, 2f, 2f, 50f, 0f, 20f, 0.33f, 0f, false, false);
			MapController.BurnUnitsAround_NotNetworked(this, 15, 1, 20f, this.x, this.y, true, true);
			Map.ExplodeUnits(this, 12, DamageType.Explosion, this.explosionRange * 2f, this.explosionRange * 1.5f, this.x, this.y - 4f, 200f, 300f, 15, false, false);
			Map.ExplodeUnits(this, 1, DamageType.Explosion, this.explosionRange * 2f, this.explosionRange, this.x, this.y - 4f, 200f, 300f, -1, false, false);
			MapController.DamageGround(this, this.explosionDamage, DamageType.Explosion, this.explosionRange, this.x, this.y, null);
			Map.ShakeTrees(this.x, this.y, 80f, 32f, 64f);
			this.PlayDeathSound();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.explodeDelay > 0f)
		{
			this.explodeDelay -= Time.deltaTime;
			if (this.explodeDelay <= 0f)
			{
				this.Collapse(0f, 0f, 1f);
			}
		}
	}

	public override void ShowTopEdge()
	{
	}

	public override void ShowLeftEdge()
	{
	}

	public override void ShowRightEdge()
	{
	}

	public override void ShowBottomEdge()
	{
	}

	protected bool _isVolatile = true;

	protected GameObject volatileAgent;

	protected float explodeDelay;

	public float explodeDelayTime = 0.2f;

	protected bool hasBeenDamaged;

	protected bool exploded;

	public BlockPiece[] machinePartsPrefabs;

	public BlockPiece[] aboveDecorationPrefabs;

	public int explosionDamage = 10;

	public float explosionRange = 25f;
}
