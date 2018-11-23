// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CamophlageHolder : MonoBehaviour
{
	private void Awake()
	{
		this.sprites = base.GetComponentsInChildren<SpriteSM>();
		for (int i = 0; i < this.sprites.Length; i++)
		{
			for (int j = 0; j < this.sprites.Length; j++)
			{
				if (this.sprites[j].transform.position.y < this.sprites[i].transform.position.y)
				{
					SpriteSM spriteSM = this.sprites[j];
					this.sprites[j] = this.sprites[i];
					this.sprites[i] = spriteSM;
				}
			}
		}
	}

	public void Reveal(float delay)
	{
		this.revealDelay = delay;
		this.revealing = true;
	}

	private void Update()
	{
		if (this.revealing)
		{
			if (this.revealDelay > 0f)
			{
				this.revealDelay -= Time.deltaTime;
			}
			else
			{
				this.leafCounter += Time.deltaTime;
				if (this.leafCounter > 0f)
				{
					this.leafCounter -= 0.06f;
					if (this.revealIndex < this.sprites.Length)
					{
						if (this.revealIndex % 2 == 0)
						{
							Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.hitSounds, 0.24f, this.sprites[this.revealIndex].transform.position);
						}
						this.sprites[this.revealIndex].gameObject.SetActive(false);
						EffectsController.CreateShrapnel(this.leafBit1, this.sprites[this.revealIndex].transform.position.x, this.sprites[this.revealIndex].transform.position.y, 16f, 24f, 12f, 0f, 50f);
						EffectsController.CreateShrapnel(this.leafBit2, this.sprites[this.revealIndex].transform.position.x, this.sprites[this.revealIndex].transform.position.y, 16f, 24f, 12f, 0f, 50f);
						this.revealIndex++;
					}
					else
					{
						base.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	protected SpriteSM[] sprites;

	public Shrapnel leafBit1;

	public Shrapnel leafBit2;

	protected float leafCounter;

	protected bool revealing;

	protected int revealIndex;

	protected float revealDelay;

	public SoundHolder soundHolder;
}
