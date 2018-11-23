// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TankHatch : SimpleSpriteWrapper
{
	protected override void Awake()
	{
		base.Awake();
	}

	public void Open()
	{
		if (!this.opening)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.effortSounds, 0.84f, (base.transform.position + SortOfFollow.GetInstance().transform.position) / 2f);
		}
		this.opening = true;
		this.closing = false;
		base.enabled = true;
	}

	public void Close()
	{
		if (!this.closing)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.defendSounds, 0.84f, (base.transform.position + SortOfFollow.GetInstance().transform.position) / 2f);
		}
		this.opening = false;
		this.closing = true;
		base.enabled = true;
	}

	private void Update()
	{
		this.frameCounter += Time.deltaTime;
		if (this.frameCounter > 0.033f)
		{
			this.frameCounter -= 0.033f;
			if (this.opening)
			{
				if (base.Frame < 3)
				{
					base.Frame++;
				}
				else
				{
					base.enabled = false;
					this.opening = false;
				}
			}
			else if (this.closing)
			{
				if (base.Frame > 0)
				{
					base.Frame--;
				}
				else
				{
					base.enabled = false;
					this.closing = false;
				}
			}
		}
	}

	protected bool opening;

	protected bool closing;

	public SoundHolder soundHolder;
}
