// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BossBlockArtillery : BossBlockWeapon
{
	protected override void FireWeapon()
	{
		base.GetComponent<Collider>().enabled = false;
		float num = base.transform.position.x + this.fireOffset.x;
		float num2 = base.transform.position.y + this.fireOffset.y;
		float x = this.fireDirection.x;
		float y = this.fireDirection.y;
		Vector3 playerPos = HeroController.GetPlayerPos(this.seenPlayerNum);
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			" Seenplayernum ",
			this.seenPlayerNum,
			" pos ",
			playerPos
		}));
		if (Mathf.Sign(playerPos.x - num) == Mathf.Sign(x) && playerPos.x > 0f)
		{
			this.fireCount++;
			float num3 = 1f;
			if (playerPos.x > 0f)
			{
				float f = playerPos.x - num;
				float num4 = playerPos.y - num2;
				num3 = 0.2f + Mathf.Abs(f) / 180f + num4 / 300f;
				if (num3 < 0.5f)
				{
					num3 = 0.5f;
				}
			}
			Projectile x2 = ProjectileController.SpawnProjectileOverNetwork(this.projectile, this, num, num2, Mathf.Sign(x) * 150f * num3 * (0.85f + UnityEngine.Random.value * 0.3f), 150f * num3, true, -1, false, true);
			UnityEngine.Debug.Log("Fire artillery ! " + (x2 != null));
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.attackSounds, 0.5f, new Vector3(num, num2, 0f));
			if (this.fireCount % 3 == 2)
			{
				this.fireDelay = this.longFireDelay;
			}
			else
			{
				this.fireDelay = this.shortFireDelay;
			}
		}
		else
		{
			this.frame = this.returnToRestFrame - 1;
		}
		base.GetComponent<Collider>().enabled = true;
	}

	protected int fireCount;

	public float longFireDelay = 3f;

	public float shortFireDelay = 0.5f;
}
