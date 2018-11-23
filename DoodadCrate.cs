// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class DoodadCrate : DoodadDestroyable
{
	protected override void Start()
	{
		base.Start();
		this.block = base.GetComponent<Block>();
		if (this.canSpawnAmmo && Map.MapData.spawnAmmoCrates)
		{
			Map.woodBlockCount++;
			if (Map.woodBlockCount % (GameModeController.IsDeathMatchMode ? 8 : 11) == 0)
			{
				this.sprite.SetLowerLeftPixel((float)((int)this.sprite.lowerLeftPixel.x), (float)this.ammoCrateLowerLeftY);
				this.containsPresent = true;
			}
		}
		if (this.containsPresent)
		{
			this.pickup = PickupableController.CreateAmmoBox(this.x, this.y);
			Registry.RegisterDeterminsiticGameObject(this.pickup.gameObject);
			this.pickup.gameObject.SetActive(false);
		}
		if (this.blockWidth > 0 && this.blockHeight > 0)
		{
			for (int i = 0; i < this.blockWidth; i++)
			{
				for (int j = 0; j < this.blockHeight; j++)
				{
					Map.blocks[this.collumn + i, this.row + j] = base.GetComponent<Block>();
				}
			}
		}
	}

	protected override void MakeEffectsDeath()
	{
		if (base.GetComponent<UnityEngine.Collider>() != null)
		{
			base.GetComponent<UnityEngine.Collider>().enabled = false;
		}
		base.MakeEffectsDeath();
		if (this.containsPresent && !Map.isEditing)
		{
			this.pickup.Launch(this.x + this.ammoXOffset, this.y, 0f, 200f);
			EffectsController.CreatePuffDisappearRingEffect(this.x + this.ammoXOffset, this.y, 0f, 0f);
		}
	}

	protected override void Update()
	{
		if (this.block != null && !this.collapsed && this.block.health <= 0)
		{
			this.Collapse();
		}
		base.Update();
	}

	public override void Death()
	{
		if (base.GetComponent<Block>() != null)
		{
			base.GetComponent<Block>().CollapseForced();
		}
		base.Death();
	}

	public bool canSpawnAmmo;

	protected bool containsPresent;

	public int ammoCrateLowerLeftY = 64;

	public float ammoXOffset = 8f;

	private Pickupable pickup;

	public int blockWidth;

	public int blockHeight;

	private Block block;
}
