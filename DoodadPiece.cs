// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DoodadPiece : MonoBehaviour
{
	protected virtual void Awake()
	{
		base.gameObject.SetActive(false);
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.halfHeight = this.size / 2f;
	}

	protected virtual void Start()
	{
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		if (this.shrapnels.Length > 0)
		{
			this.shrapnelCount = Mathf.CeilToInt((float)this.shrapnelCount / (float)this.shrapnels.Length);
		}
		if (this.mustSetSpriteValues)
		{
			this.sprite.RecalcTexture();
			this.sprite.SetPixelDimensions((int)this.spritePixelDimensions.x, (int)this.spritePixelDimensions.y);
			UnityEngine.Debug.Log("Deactivate dooad piece " + this.sprite.pixelDimensions.x);
			this.sprite.SetLowerLeftPixel(this.spriteLowerLeft);
			this.sprite.SetOffset(this.spriteOffset);
			this.sprite.SetSize(this.spriteWidth, this.spriteHeight);
			this.sprite.CalcSize();
		}
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
	}

	public void SetupSprite(Material material, Vector2 lowerLeft, Vector2 pixelD, Vector3 offset, float width, float height)
	{
		this.sprite = base.GetComponent<SpriteSM>();
		if (this.sprite != null)
		{
			this.mustSetSpriteValues = true;
			base.GetComponent<Renderer>().sharedMaterial = material;
			this.spritePixelDimensions = pixelD;
			this.spriteOffset = offset;
			this.spriteLowerLeft = lowerLeft;
			this.spriteWidth = width;
			this.spriteHeight = height;
		}
		else
		{
			UnityEngine.Debug.LogError("No Sprite!!!");
		}
	}

	public virtual void Collapse()
	{
		base.gameObject.SetActive(false);
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.delay > 0f)
		{
			this.delay -= num;
			this.SetPosition(global::Math.Sin(Time.time * 60f) * 1f);
		}
		else
		{
			if (Physics.Raycast(base.transform.position, Vector3.down, out this.groundHit, 24f, this.groundLayer))
			{
				this.groundHeight = this.groundHit.point.y;
			}
			this.yI -= 900f * num;
			float num2 = this.yI * num;
			if (this.ClampToGround(ref num2))
			{
				if (this.yI < -200f)
				{
					this.y += num2;
					this.Death();
				}
				else
				{
					this.y += num2;
					this.Death();
				}
			}
			else if (this.damageMooks && Map.HitLivingUnits(this, 10, 3, DamageType.Stun, this.size, this.x + this.offsetX, this.y + this.offsetY + num2 - this.halfHeight, 0f, -this.yI * 0.5f, false, true))
			{
				if (this.flickPuff != null)
				{
					EffectsController.CreateEffect(this.flickPuff, this.x + this.offsetX, this.y + this.offsetY);
				}
				this.y += num2;
				this.Death();
			}
			else
			{
				this.y += num2;
			}
			this.SetPosition(0f);
		}
	}

	protected void SetPosition(float xOffset)
	{
		base.transform.position = new Vector3(Mathf.Round(this.x + xOffset), Mathf.Round(this.y), 0f);
	}

	public virtual void Death()
	{
		float num = this.baseForce + Mathf.Clamp(Mathf.Abs(this.yI) / 3f - 50f * this.forceM, 0f, 200f);
		for (int i = 0; i < this.shrapnels.Length; i++)
		{
			EffectsController.CreateShrapnel(this.shrapnels[i], this.x + this.offsetX, this.y + this.offsetY, this.size, num, 7f, 0f, num * 0.7f);
		}
		if (this.soundHolder != null)
		{
			if (!this.damageMooks)
			{
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.deathSounds, 0.4f, base.transform.position);
			}
			else
			{
				Sound.GetInstance().PlaySoundEffectAt(this.soundHolder.deathSounds, 0.4f, base.transform.position);
			}
		}
		base.gameObject.SetActive(false);
	}

	protected bool ClampToGround(ref float yIT)
	{
		if (Physics.Raycast(new Vector3(this.x, this.y + 4f, 0f), Vector3.down, out this.groundHit, this.size + 7f, this.groundLayer) && this.y + yIT - this.halfHeight < this.groundHit.point.y)
		{
			yIT = this.groundHit.point.y - (this.y - this.halfHeight);
			return true;
		}
		return false;
	}

	protected LayerMask groundLayer;

	protected RaycastHit groundHit;

	protected float groundHeight;

	public Shrapnel[] shrapnels;

	public FlickerFader flickPuff;

	public int shrapnelCount = 15;

	public float size = 3f;

	protected float halfHeight = 2f;

	public float offsetX;

	public float offsetY;

	protected float x;

	protected float y;

	protected float yI;

	public bool damageMooks;

	public SoundHolder soundHolder;

	public float baseForce = 80f;

	public float forceM = 1f;

	public float delay;

	protected Vector2 spriteLowerLeft;

	protected Vector2 spritePixelDimensions;

	protected Vector3 spriteOffset;

	protected Vector3 spriteSize;

	protected bool mustSetSpriteValues;

	protected SpriteSM sprite;

	protected float spriteWidth;

	protected float spriteHeight;
}
