// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Parachute : BroforceObject
{
	protected virtual void Awake()
	{
		this.mook = base.transform.parent.GetComponent<Mook>();
		this.sprite = base.GetComponent<SpriteSM>();
		if (this.mook == null)
		{
			UnityEngine.Debug.LogError("Parachute could not find parent mook...");
		}
	}

	private void Start()
	{
	}

	private void Damage(DamageObject damage)
	{
		if (damage.damageType != DamageType.Melee)
		{
			this.mook.IsParachuteActive = false;
			this.mook.Tumble();
		}
	}

	public void Deform()
	{
		this.deformed = true;
		if (!this.wasDeformed)
		{
			this.deformCounter = 0f;
			this.deformFrame = 1;
			this.sprite.SetLowerLeftPixel(32f, 32f);
			Sound.GetInstance().PlaySoundEffectAt(this.deformSounds.effortSounds, 0.2f, base.transform.position);
		}
	}

	public bool IsDeformed()
	{
		return this.deformFrame > 0;
	}

	private void Update()
	{
		if (this.deformed)
		{
			this.wasDeformed = true;
			this.deformed = false;
			if (this.deformFrame < 2)
			{
				this.deformCounter += Time.deltaTime;
				if (this.deformCounter > 0.045f)
				{
					this.deformFrame = 2;
					this.deformCounter = 0f;
					this.sprite.SetLowerLeftPixel(64f, 32f);
				}
			}
		}
		else if (this.wasDeformed)
		{
			if (this.sprite.lowerLeftPixel.x > 32f)
			{
				this.deformFrame = 1;
				this.sprite.SetLowerLeftPixel(32f, 32f);
				this.deformCounter = 0f;
			}
			if (this.deformFrame > 0)
			{
				this.deformCounter += Time.deltaTime;
				if (this.deformCounter > 0.045f)
				{
					this.deformFrame = 0;
					this.sprite.SetLowerLeftPixel(0f, 32f);
					this.wasDeformed = false;
				}
			}
		}
	}

	public Mook mook;

	private bool deformed;

	private bool wasDeformed;

	private SpriteSM sprite;

	private float deformCounter;

	private int deformFrame;

	public SoundHolder deformSounds;
}
