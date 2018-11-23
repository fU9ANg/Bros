// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MobileSwitch : Switch
{
	protected override void Start()
	{
		Map.RegisterSwitch(this);
	}

	public override void Activate(Unit activatingUnit)
	{
		if (!Map.isEditing && this.affectedGameObject != null && this.methodName.Length > 0 && activatingUnit.IsMine)
		{
			this.used = true;
			if (this.delay > 0f && this.activationDelay <= 0f)
			{
				this.activationDelay = this.delay;
				this.activatingUnit = activatingUnit;
			}
			else
			{
				this.affectedGameObject.SendMessage(this.methodName, activatingUnit, SendMessageOptions.RequireReceiver);
			}
			Sound.GetInstance().PlaySoundEffectAt(this.soundEffect, this.volume, base.transform.position);
			EffectsController.CreatePuffDisappearRingEffect(base.transform.position.x, base.transform.position.y, 0f, 0f);
			if (this.useOnce)
			{
				this.bubble.GoAway();
				Map.RemoveSwitch(this);
			}
		}
	}

	private void Update()
	{
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		if ((!this.useOnce || (!this.used && this.activationDelay <= 0f)) && HeroController.IsPlayerNearby(this.x, this.y, this.switchRange, this.switchRange))
		{
			this.bubble.ShowBubble();
		}
		if (this.activationDelay > 0f)
		{
			this.activationDelay -= Time.deltaTime;
			if (this.activationDelay <= 0f)
			{
				if (this.activateDelayedSoundEffect != null)
				{
					Sound.GetInstance().PlaySoundEffectAt(this.activateDelayedSoundEffect, this.volume, base.transform.position);
				}
				this.affectedGameObject.SendMessage(this.methodName, this.activatingUnit, SendMessageOptions.RequireReceiver);
			}
		}
	}

	public ReactionBubble bubble;

	public float switchRange = 24f;

	public GameObject affectedGameObject;

	public string methodName = string.Empty;

	public float delay;

	protected float activationDelay;

	public bool useOnce = true;

	protected bool used;

	public AudioClip activateDelayedSoundEffect;

	public float volume = 0.4f;

	protected Unit activatingUnit;
}
