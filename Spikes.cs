// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Spikes : Doodad
{
	protected override void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		if (UnityEngine.Random.value < 0.5f)
		{
			this.sprite.lowerLeftPixel.x = this.sprite.pixelDimensions.x * 2f;
			this.sprite.SetLowerLeftPixel(this.sprite.lowerLeftPixel);
			this.spriteInFront.lowerLeftPixel.x = this.spriteInFront.pixelDimensions.x * 2f;
			this.spriteInFront.SetLowerLeftPixel(this.spriteInFront.lowerLeftPixel);
		}
		base.Start();
	}

	public void ImpaleUnit(TestVanDammeAnim unit)
	{
		float num = Mathf.Abs(unit.transform.position.x - base.transform.position.x);
		float num2 = Mathf.Abs(unit.transform.position.y - base.transform.position.y);
		if (num > 8f || num2 > 8f)
		{
			unit.x = this.x;
			unit.y = this.y + 6f;
		}
		unit.impaledOnSpikes = this;
		unit.Damage(25, DamageType.Spikes, 0f, 1f, Mathf.RoundToInt(unit.transform.localScale.x), null, unit.x, unit.y + 8f);
		this.sprite.lowerLeftPixel.y = this.sprite.pixelDimensions.y * 2f;
		this.sprite.SetLowerLeftPixel(this.sprite.lowerLeftPixel);
		this.spriteInFront.lowerLeftPixel.y = this.spriteInFront.pixelDimensions.y * 2f;
		this.spriteInFront.SetLowerLeftPixel(this.spriteInFront.lowerLeftPixel);
		this.PlayDeathSound();
		GameObject gameObject = new GameObject("ImpalementCollider");
		BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
		boxCollider.size = new Vector3(20f, 9f, 64f);
		gameObject.transform.parent = unit.transform;
		gameObject.transform.localPosition = new Vector3(1f, 4f, 0f);
		gameObject.layer = LayerMask.NameToLayer("Platform");
		gameObject.tag = "Slime";
	}

	private void PlayDeathSound()
	{
		Sound instance = Sound.GetInstance();
		if (instance != null)
		{
			instance.PlaySoundEffectAt(this.soundHolder.deathSounds, 0.6f, base.transform.position);
		}
	}

	protected SpriteSM sprite;

	public SpriteSM spriteInFront;
}
