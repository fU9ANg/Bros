// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookGeneral : TestVanDammeAnim
{
	protected override void UseFire()
	{
	}

	public override bool IsEvil()
	{
		return true;
	}

	protected override void UseSpecial()
	{
		if (Time.time - this.lastSpawnTime > 3f)
		{
			this.PlaySpecial3Sound(0.4f);
			this.lastSpawnTime = Time.time;
			for (int i = 0; i < 3; i++)
			{
				UnityEngine.Object @object = (UnityEngine.Random.value >= Map.MapData.suicideMookSpawnProbability) ? Resources.Load("Mooks/ZMook") : Resources.Load("Mooks/ZMookSuicide");
				Mook component = (@object as GameObject).GetComponent<Mook>();
				this.SpawnMook(component, this.x + (float)UnityEngine.Random.Range(-64, 64), SortOfFollow.GetScreenMaxY() + 32f);
			}
		}
	}

	protected void SpawnMook(Mook prefab, float x, float y)
	{
		if (prefab != null)
		{
			MapController.SpawnMook_Networked(prefab, x, y, 0f, 0f, false, false, true, false, true);
		}
		else
		{
			UnityEngine.Debug.LogError("Could not instantiate resource");
		}
	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void AnimateSpecial()
	{
		this.SetSpriteOffset(0f, 0f);
		this.DeactivateGun();
		this.frameRate = 0.0667f;
		int num = Mathf.Clamp(this.frame % 4, 0, 4);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), (float)(this.spritePixelHeight * 7));
		if (this.frame % 4 == 2)
		{
			this.UseSpecial();
		}
	}

	protected override void NotifyDeathType()
	{
		if (!this.hasNotifiedDeathType)
		{
			this.hasNotifiedDeathType = true;
			StatisticsController.NotifyDeathType(MookType.General, this.deathType, this.xI, this.yI);
		}
	}

	private float lastSpawnTime;
}
