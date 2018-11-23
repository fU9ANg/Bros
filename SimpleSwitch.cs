// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SimpleSwitch : Switch
{
	protected override void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.deactivatedLowerLeft = this.sprite.lowerLeftPixel;
		this.activatedLowerLeft = new Vector2(this.deactivatedLowerLeft.x + this.sprite.pixelDimensions.x, this.deactivatedLowerLeft.y);
	}

	public override void Activate(Unit activatingUnit)
	{
		if (!Map.isEditing)
		{
			if (this.affectedGameObject != null && this.methodName.Length > 0)
			{
				if (this.delay > 0f && this.activationDelay <= 0f)
				{
					this.activationDelay = this.delay;
				}
				else
				{
					this.affectedGameObject.SendMessage(this.methodName, SendMessageOptions.RequireReceiver);
				}
				this.sprite.SetLowerLeftPixel(this.activatedLowerLeft);
				Sound.GetInstance().PlaySoundEffectAt(this.soundEffect, 0.6f, base.transform.position);
				EffectsController.CreatePuffDisappearRingEffect(base.transform.position.x, base.transform.position.y, 0f, 0f);
			}
			this.activateCounter = this.activateTime;
		}
	}

	protected override void AttachDoodad()
	{
	}

	protected virtual void Update()
	{
		if (this.activateCounter > 0f)
		{
			this.activateCounter -= Time.deltaTime;
			if (this.activateCounter <= 0f)
			{
				this.sprite.SetLowerLeftPixel(this.deactivatedLowerLeft);
			}
		}
		if (this.activationDelay > 0f)
		{
			this.activationDelay -= Time.deltaTime;
			if (this.activationDelay <= 0f)
			{
				this.affectedGameObject.SendMessage(this.methodName, SendMessageOptions.RequireReceiver);
			}
		}
	}

	protected SpriteSM sprite;

	protected Vector2 deactivatedLowerLeft;

	protected Vector2 activatedLowerLeft;

	public GameObject affectedGameObject;

	public string methodName = string.Empty;

	public float activateTime = 1f;

	protected float activateCounter;

	public float delay;

	protected float activationDelay;
}
