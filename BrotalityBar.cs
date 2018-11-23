// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BrotalityBar : MonoBehaviour
{
	public void Setup(Vector3 pos, float width)
	{
		base.transform.position = pos;
		this.screenWidth = width;
	}

	public void SetSize(float m, float invM)
	{
		this.scrollCounter += Time.deltaTime;
		this.backgroundSprite.SetSize(this.screenWidth * m, 5f);
		this.backgroundSprite.SetPixelDimensions((int)Mathf.Ceil(this.screenWidth * m), 5);
		if (this.glowSprite != null)
		{
			this.glowCounter += Time.deltaTime * this.glowPulseSpeed;
			float t = Mathf.Sin(this.glowCounter) * 0.5f + 0.5f;
			this.glowSprite.SetColor(Color.Lerp(this.glowColor1, this.glowColor2, t));
			this.glowSprite.SetSize((this.screenWidth + 2f) * (m - invM), 7f);
			this.glowSprite.transform.localPosition = new Vector3((this.screenWidth + 2f) * invM - 1f, 0f, 0f);
		}
		if (this.ParticleEmitter != null)
		{
			this.particleCounter += Time.deltaTime;
			if (this.particleCounter > this.particleRate)
			{
				this.particleCounter -= this.particleRate;
				this.particleMassing += (float)this.particleCount * (m - invM);
				for (int i = 0; i < (int)this.particleMassing; i++)
				{
					this.particleMassing -= 1f;
					this.ParticleEmitter.Emit(base.transform.position + new Vector3(invM * this.screenWidth + UnityEngine.Random.value * (this.screenWidth * (m - invM)), -2f + UnityEngine.Random.value * 4f, -2f), Vector3.zero, Mathf.Lerp(this.ParticleEmitter.minSize, this.ParticleEmitter.maxSize, UnityEngine.Random.value), Mathf.Lerp(this.ParticleEmitter.minEnergy, this.ParticleEmitter.maxEnergy, UnityEngine.Random.value), Color.white);
				}
			}
		}
		foreach (SpriteSM spriteSM in this.sprites)
		{
			this.sCount++;
			spriteSM.SetSize(this.screenWidth * m, (float)this.secondaryBarsHeight);
			spriteSM.SetPixelDimensions((int)Mathf.Ceil(this.screenWidth * m), this.secondaryBarsHeight);
			if (this.scrollLowerLeft)
			{
				spriteSM.SetLowerLeftPixel((float)((int)(this.scrollCounter * (this.scrollXSpeed + (float)(this.sCount % this.sprites.Length) * this.scrollXSpeed * 0.33f))), (float)((int)(this.scrollCounter * (this.scrollYSpeed + (float)(this.sCount % this.sprites.Length) * this.scrollYSpeed * 0.33f))));
			}
		}
	}

	public SpriteSM[] sprites;

	public SpriteSM backgroundSprite;

	protected float screenWidth = 200f;

	public bool scrollLowerLeft;

	protected float scrollCounter;

	protected int sCount;

	public float scrollXSpeed = 2f;

	public float scrollYSpeed = 5f;

	public int secondaryBarsHeight = 5;

	public SpriteSM glowSprite;

	protected float glowCounter;

	public Color glowColor1 = Color.white;

	public Color glowColor2 = Color.white;

	public float glowPulseSpeed = 15f;

	public ParticleEmitter ParticleEmitter;

	protected float particleCounter;

	public float particleRate = 0.1f;

	public int particleCount;

	protected float particleMassing;
}
