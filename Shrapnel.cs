// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Shrapnel : BroforceObject
{
	protected virtual void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		if (this.RandomizeSeed)
		{
			this.seed = UnityEngine.Random.Range(0, 999999999);
		}
		this.random = new Randomf(this.seed);
		this.zOffset = UnityEngine.Random.value * 0.05f;
	}

	protected virtual void Start()
	{
		this.groundLayer = 1 << LayerMask.NameToLayer("Ground");
		this.seeSawCounter = UnityEngine.Random.value * 100f;
		this.seeSawSpeed = 8f + UnityEngine.Random.value * 5f;
		this.spritePixelHeight = (int)this.sprite.pixelDimensions.y;
		global::Math.SetupLookupTables();
	}

	public virtual void Launch(float x, float y, float xI, float yI)
	{
		this.x = x;
		this.y = y;
		this.xI = xI;
		this.yI = yI;
		this.r = UnityEngine.Random.value * this.r;
		this.life = (2f + this.random.value) * this.lifeM;
		this.startLife = this.life;
		this.spriteWidth = this.sprite.width;
		this.spriteHeight = this.sprite.height;
		this.spriteWidthI = -this.spriteWidth / this.life * 1f;
		this.spriteHeightI = -this.spriteHeight / this.life * 1f;
		this.rI = -Mathf.Sign(xI) * (200f + UnityEngine.Random.value * 300f) * this.rotationSpeedMultiplier;
		this.SetPosition();
		if (!this.shrapnelControlsMotion && base.GetComponent<Rigidbody>() == null)
		{
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(this.size * 2f + 2f, this.size * 2f + 2f, 6f);
			base.gameObject.AddComponent<Rigidbody>();
			base.GetComponent<Rigidbody>().AddForce(new Vector3(xI, yI, 0f), ForceMode.VelocityChange);
			base.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)56;
			base.GetComponent<Rigidbody>().AddTorque(new Vector3(0f, 0f, this.rI * 9f), ForceMode.VelocityChange);
			base.GetComponent<Rigidbody>().drag = 0.5f;
			base.GetComponent<Rigidbody>().angularDrag = 1f;
		}
	}

	public virtual void SetPosition()
	{
		base.transform.position = new Vector3(Mathf.Round(this.x), Mathf.Round(this.y), this.zOffset);
		if (this.rotateAtRightAngles)
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Round(this.r / 90f) * 90f);
		}
		else
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, this.r);
		}
	}

	protected virtual void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.shrapnelControlsMotion)
		{
			this.r += this.rI * this.t;
			this.yI -= 500f * this.t * this.gravityM;
			if (this.randomMotion)
			{
				this.yI += (6f - UnityEngine.Random.value * 12f) * this.randomMotionM;
				this.xI += (6f - UnityEngine.Random.value * 12f) * this.randomMotionM;
				this.yI *= 1f - this.t * 2f * this.randomMotionM;
				this.xI *= 1f - this.t * 2f * this.randomMotionM;
			}
			if (this.fades)
			{
				float a = this.life / this.startLife;
				this.sprite.SetColor(new Color(1f, 1f, 1f, a));
			}
			if (this.fadeUVs)
			{
				float num = this.life / this.startLife;
				if (num < 0.3f)
				{
					this.sprite.SetLowerLeftPixel((float)(this.spritePixelHeight * 3), (float)this.spritePixelHeight);
				}
				else if (num < 0.6f)
				{
					this.sprite.SetLowerLeftPixel((float)(this.spritePixelHeight * 2), (float)this.spritePixelHeight);
				}
				else if (num < 0.8f)
				{
					this.sprite.SetLowerLeftPixel((float)(this.spritePixelHeight * 1), (float)this.spritePixelHeight);
				}
				else
				{
					this.sprite.SetLowerLeftPixel(0f, (float)this.spritePixelHeight);
				}
			}
			if (this.seeSawFalling)
			{
				this.xI *= 1f - this.t * 1f;
				this.yI *= 1f - this.t * 2f;
				this.seeSawCounter += this.t * this.seeSawSpeed;
				if (this.yI < -8f)
				{
					this.xI += global::Math.Sin(this.seeSawCounter) * this.t * 300f;
				}
				if (this.yI < 7f)
				{
					this.yI += global::Math.Sin(this.seeSawCounter * 2f + 3.14159274f) * this.t * 300f;
				}
			}
			if (this.shrink)
			{
				this.spriteWidth += this.spriteWidthI * this.t;
				this.spriteHeight += this.spriteHeightI * this.t;
				this.sprite.SetSize(this.spriteWidth, this.spriteHeight);
			}
			this.RunMovement();
			this.SetPosition();
		}
		this.RunLife();
	}

	protected virtual void RunLife()
	{
		this.life -= this.t;
		if (this.life <= 0f)
		{
			this.Death();
		}
	}

	protected virtual void RunMovement()
	{
		bool bounceY = false;
		bool bounceX = false;
		float num = this.yI * this.t;
		float num2 = this.xI * this.t;
		if (this.collidesWithWalls)
		{
			if (this.destroyInsideWalls && Map.InsideWall(this.x, this.y, this.size / 2f))
			{
				this.life = -0.1f;
			}
			if (Map.ConstrainToBlocks(this.x, this.y, this.size, ref num2, ref num, ref bounceX, ref bounceY))
			{
				this.Bounce(bounceX, bounceY);
			}
		}
		this.x += num2;
		this.y += num;
	}

	public virtual void Death()
	{
		this.isDead = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected virtual void Bounce(bool bounceX, bool bounceY)
	{
		if (bounceX)
		{
			this.xI *= -0.6f;
			this.yI *= 0.8f;
		}
		if (bounceY)
		{
			this.xI *= 0.8f * this.frictionM;
			this.yI *= -0.4f;
			this.life -= 0.1f;
			this.rI *= 0.8f;
		}
		if (this.deathOnBounce)
		{
			this.Death();
		}
		if (this.bloodSplashOnDeath && bounceY && this.yI > 0f)
		{
			EffectsController.CreateBloodSplashEffect(this.bloodColor, this.x, this.y, this.xI, this.yI);
			if (!this.isDead)
			{
				this.Death();
			}
		}
		else if (this.lifeLossOnBounce)
		{
			this.life -= this.startLife * this.lifeLossM;
		}
	}

	public float r;

	public float rI;

	public bool rotateAtRightAngles = true;

	protected float life;

	protected float startLife = 1f;

	protected float t;

	protected LayerMask groundLayer;

	protected float spriteWidth = 2f;

	protected float spriteHeight = 2f;

	public float size = 3f;

	public float lifeM = 1f;

	public float frictionM = 1f;

	protected float spriteWidthI;

	protected float spriteHeightI;

	public bool shrink = true;

	public bool fades;

	public bool fadeUVs;

	protected int spritePixelHeight = 2;

	public float gravityM = 1f;

	public bool randomMotion;

	public float randomMotionM = 1f;

	public bool collidesWithWalls = true;

	public bool seeSawFalling;

	protected float seeSawCounter;

	protected float seeSawSpeed = 1f;

	public bool deathOnBounce;

	public bool bloodSplashOnDeath;

	public float rotationSpeedMultiplier = 1f;

	public bool lifeLossOnBounce;

	public float lifeLossM = 0.2f;

	protected bool isDead;

	protected float zOffset;

	public bool destroyInsideWalls = true;

	protected SpriteSM sprite;

	public int seed = 10;

	public bool RandomizeSeed;

	protected Randomf random;

	public bool shrapnelControlsMotion = true;

	private BloodColor bloodColor;
}
