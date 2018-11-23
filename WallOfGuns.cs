// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WallOfGuns : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.weapons.Length; i++)
		{
		}
		if (this.openingGrindClip != null)
		{
			this.grindSource = base.gameObject.AddComponent<AudioSource>();
			this.grindSource.rolloffMode = AudioRolloffMode.Linear;
			this.grindSource.minDistance = 200f;
			this.grindSource.dopplerLevel = 0.1f;
			this.grindSource.maxDistance = 500f;
			this.grindSource.volume = 0.33f;
			this.grindSource.loop = true;
			this.grindSource.clip = this.openingGrindClip;
		}
	}

	protected virtual void Update()
	{
		if (Map.isEditing)
		{
			return;
		}
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.autonomous)
		{
			this.thinkCounter += num;
			if (this.thinkCounter > 1f)
			{
				this.thinkCounter -= this.thinkRate;
				this.Think();
			}
		}
		if (this.opening)
		{
			if (this.shakeDelay > 0f)
			{
				this.shakeDelay -= num;
				this.wallTransform.localPosition = new Vector3(this.localWallX + global::Math.Sin(this.shakeDelay * 80f) * this.shakeDelay * 3.33f, this.wallY, this.wallTransform.localPosition.z);
			}
			else
			{
				this.openSpeed = Mathf.Lerp(this.openSpeed, this.openSpeed * 2f, num);
				if (this.screenShakeGrind > 0f)
				{
					this.grindCounter += num;
					if (this.grindCounter > 0.0333f)
					{
						this.grindCounter -= 0.0333f;
						SortOfFollow.Shake(this.screenShakeGrind, 1f, base.transform.position);
					}
				}
				this.wallY += this.openSpeed * num;
				if (this.openSpeed > 0f && this.wallY > this.openYOffset)
				{
					this.FinishOpening();
				}
				else if (this.openSpeed < 0f && this.wallY < this.openYOffset)
				{
					this.FinishOpening();
				}
				this.wallTransform.localPosition = new Vector3(this.localWallX, this.wallY, this.wallTransform.localPosition.z);
			}
			if (this.motor != null && this.motor.health <= 0)
			{
				this.opened = true;
				this.opening = false;
			}
		}
		if (this.motor != null && this.motor.health <= 0 && this.wallY > 0f)
		{
			this.openSpeed -= 1000f * num;
			this.wallY += this.openSpeed * num;
			if (this.wallY <= 0f)
			{
				this.wallY = 0f;
				if (this.openSpeed < -300f)
				{
					if (this.heartBlock != null)
					{
						this.heartBlock.DelayedDestroy();
					}
					SortOfFollow.Shake(0.5f, 1f, base.transform.position);
				}
				else if (this.openSpeed < -100f)
				{
					SortOfFollow.Shake(0.3f, 1f, base.transform.position);
				}
			}
			this.wallTransform.localPosition = new Vector3(this.localWallX, this.wallY, this.wallTransform.localPosition.z);
		}
	}

	public virtual void Activate()
	{
		if (!this.opening && !this.opened)
		{
			this.shakeDelay = 0.3f;
			this.opening = true;
			if (this.grindSource != null)
			{
				this.grindSource.Play();
			}
		}
	}

	protected virtual void FinishOpening()
	{
		if (!this.opened && this.screenShakeOnOpen > 0f)
		{
			SortOfFollow.Shake(this.screenShakeOnOpen, this.screenShakeSpeedM);
		}
		if (this.grindSource != null && this.grindSource.isPlaying)
		{
			this.grindSource.Stop();
		}
		if (this.finishOpeningClip != null)
		{
			Sound.GetInstance().PlaySoundEffect(this.finishOpeningClip, 0.4f);
		}
		this.opening = false;
		this.opened = true;
		this.wallY = this.openYOffset;
		this.wallTransform.localPosition = new Vector3(this.localWallX, this.wallY, this.wallTransform.localPosition.z);
		for (int i = 0; i < this.weapons.Length; i++)
		{
			this.weapons[i].enabled = true;
		}
	}

	protected virtual void Think()
	{
		if (!this.opened && !this.opening && SortOfFollow.IsItSortOfVisible(base.transform.position.x, base.transform.position.y + this.openYOffset, 10f, 20f) && HeroController.IsPlayerNearby(base.transform.position.x + this.lookForPlayerXOffset, base.transform.position.y + this.lookForPlayerYOffset, this.lookForPlayerXDirection, this.lookForPlayerXRange, this.lookForPlayerYRange))
		{
			this.shakeDelay = 0.3f;
			this.opening = true;
		}
	}

	private float thinkCounter;

	public float lookForPlayerXOffset = -64f;

	public float lookForPlayerYOffset = 64f;

	public float lookForPlayerXRange = 128f;

	public float lookForPlayerYRange = 128f;

	public int lookForPlayerXDirection = -1;

	public float openSpeed = 200f;

	protected bool opening;

	protected bool opened;

	public float openYOffset = 48f;

	protected float shakeDelay;

	public Transform wallTransform;

	public BossBlockPiece motor;

	public float localWallX = 8f;

	public BossBlockWeapon[] weapons;

	public BossBlockPiece heartBlock;

	protected float wallY;

	public float screenShakeOnOpen;

	public float screenShakeSpeedM = 1f;

	public float screenShakeGrind;

	protected float grindCounter;

	[HideInInspector]
	public bool autonomous = true;

	public AudioClip finishOpeningClip;

	public AudioClip slamBackClip;

	public AudioClip openingGrindClip;

	private AudioSource grindSource;

	public float thinkRate = 1f;
}
