// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookCaptainCutscene : Mook
{
	protected override void Start()
	{
		base.Start();
		this.actionState = ActionState.Taunting;
		this.counter -= 3f;
		this.sprite.SetLowerLeftPixel((float)(25 * this.spritePixelWidth), 32f);
		base.transform.localScale = new Vector3(-1f, 1f, 1f);
		this.enemyAI.enabled = false;
		this.right = false; this.left = (this.right );
	}

	protected override void Update()
	{
		if (this.actionState == ActionState.Taunting)
		{
			this.t = Mathf.Clamp(Time.deltaTime * this.highFiveBoostM, 0f, 0.04f);
			this.counter += this.t;
			if (this.counter > this.frameRate)
			{
				this.counter -= this.frameRate;
				this.ChangeFrame();
				if (base.transform.localScale.x > 0f)
				{
					base.transform.localScale = new Vector3(-1f, 1f, 1f);
					this.enemyAI.enabled = false;
				}
			}
		}
		else
		{
			base.Update();
		}
	}

	protected override void ChangeFrame()
	{
		if (this.actionState == ActionState.Taunting)
		{
			this.frame++;
			this.AnimateTaunt();
		}
		else
		{
			base.ChangeFrame();
		}
	}

	protected void AnimateTaunt()
	{
		this.frameRate = 0.1337f;
		int num = 25 + Mathf.Clamp(this.frame, 0, 32);
		this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 32f);
		if (this.frame == 11)
		{
			this.counter -= 0.5f;
		}
		if (this.frame == 12)
		{
			this.counter -= 0.066f;
		}
		if (this.frame == 15)
		{
			Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.greeting, 0.7f, base.transform.position);
		}
		if (this.frame == 18)
		{
			this.counter -= 0.2f;
		}
		if (this.frame >= 33)
		{
			base.gameObject.SetActive(false);
			this.hatchController.SendMessage("EndCutscene");
		}
	}

	public GameObject hatchController;
}
