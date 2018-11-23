// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ReviveBlast : NetworkObject
{
	private void Awake()
	{
		this.lifeStart = this.life;
	}

	public void Setup(int PlayerNum, TestVanDammeAnim ReviveSource)
	{
		this.playerNum = PlayerNum;
		this.reviveSource = ReviveSource;
		this.TryRevive();
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.delay > 0f)
		{
			this.delay -= num;
		}
		else
		{
			this.life -= num;
			if (this.life <= 0f)
			{
				this.TryRevive();
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				float num2 = this.life / this.lifeStart;
				this.range += this.rangeGrowRate * (0.1f + num2 * 0.9f) * num;
				int num3 = (int)Mathf.Clamp((this.range - 20f) / 2f, 0f, 15f);
				this.reviveBlastSprite.SetLowerLeftPixel((float)(num3 * (int)this.reviveBlastSprite.pixelDimensions.x), (float)((int)this.reviveBlastSprite.lowerLeftPixel.y));
				this.reviveCounter += num;
				if (this.reviveCounter >= 0.0334f)
				{
					this.reviveCounter -= 0.0334f;
					this.TryRevive();
				}
			}
		}
	}

	private void TryRevive()
	{
		if (base.IsMine && Map.ReviveDeadUnits(base.transform.position.x, base.transform.position.y, this.range + 4f, this.playerNum, 1, this.playerNum >= 0, this.reviveSource))
		{
			Sound.GetInstance().PlaySoundEffectAt(this.reviveClips, 0.4f, base.transform.position, 0.9f + Mathf.Clamp((float)this.reviveCount * 0.06f, 0f, 0.5f));
			this.delay = 0.15f;
			this.reviveCounter = 0.0334f;
			this.reviveCount++;
		}
	}

	public int playerNum = -1;

	public AudioClip[] reviveClips;

	protected float range = 20f;

	public float rangeGrowRate = 32f;

	public float life = 2f;

	protected float lifeStart = 0.3f;

	protected int reviveCount;

	protected float reviveCounter;

	public TestVanDammeAnim reviveSource;

	public SpriteSM reviveBlastSprite;

	protected float delay;
}
