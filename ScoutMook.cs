// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ScoutMook : Mook
{
	protected override void Awake()
	{
		base.Awake();
		this.normalMaterial = base.GetComponent<Renderer>().material;
	}

	protected override void Update()
	{
		base.Update();
		this.waveTime -= this.t;
		if (this.enemyAI.GetThinkState() == MentalState.Idle || this.enemyAI.GetThinkState() == MentalState.Suspicious)
		{
			if (base.GetComponent<Renderer>().material != this.normalMaterial)
			{
				base.GetComponent<Renderer>().material = this.normalMaterial;
			}
		}
		else if (base.GetComponent<Renderer>().material != this.disarmedMaterial)
		{
			base.GetComponent<Renderer>().material = this.disarmedMaterial;
		}
	}

	protected override void FireWeapon(float x, float y, float xSpeed, float ySpeed)
	{
		this.waveTime = 0.4f;
	}

	protected override void AnimateRunning()
	{
		if (this.burnTime > 0f || this.blindTime > 0f || this.scaredTime > 0f || this.waveTime > 0f)
		{
			this.DeactivateGun();
			this.frameRate = 0.044455f;
			int num = 21 + this.frame % 4;
			this.sprite.SetLowerLeftPixel((float)(num * this.spritePixelWidth), 32f);
		}
		else
		{
			base.AnimateRunning();
		}
	}

	protected override void PressSpecial()
	{
	}

	private float waveTime;

	public float runSpeed = 100f;

	public Material disarmedMaterial;

	private Material normalMaterial;
}
