// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ReactionBubble : MonoBehaviour
{
	public bool IsHidden
	{
		get
		{
			return !base.gameObject.active;
		}
	}

	public void RefreshYStart()
	{
		this.yStart = base.transform.localPosition.y;
	}

	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.yStart = base.transform.localPosition.y;
		global::Math.SetupLookupTables();
		this.pixelSize = (int)this.sprite.pixelDimensions.x;
		if (!this.restartedBubble)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnEnable()
	{
		if (!this.restartedBubble)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			this.restartedBubble = false;
		}
	}

	public void RestartBubble()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, this.yStart - 5f, base.transform.localPosition.z);
		this.counter = 0f;
		this.sinCounter = 0f;
		this.restartedBubble = true;
		base.gameObject.SetActive(true);
		this.life = 1.5f;
		if (this.animatedBubble)
		{
			this.frame = 0;
			this.frameCounter = 0f;
			this.SetFrame(this.frame);
		}
		this.frameCounter -= this.delayOnFirstFrame;
	}

	public void SetupHudPosition(float xPos, int yDirection)
	{
		if (yDirection > 0)
		{
			this.yStart = -8f;
		}
		else
		{
			this.yStart = -44f;
		}
		this.riseDirection = yDirection;
		base.transform.localPosition = new Vector3(xPos, this.yStart, -4f);
	}

	public void SetPosition(Vector3 vector3)
	{
		base.transform.localPosition = vector3;
		this.yStart = vector3.y;
	}

	public void RestartBubble(float life)
	{
		this.RestartBubble();
		this.life = life;
	}

	public void ShowBubble()
	{
		if (!base.gameObject.activeSelf || this.frame > this.animatedRestFrame)
		{
			this.RestartBubble();
		}
		else if (this.frame == this.animatedRestFrame)
		{
			this.frameCounter = -0.1f;
		}
	}

	public void GoAway()
	{
		if (this.frame <= this.animatedRestFrame)
		{
			this.frame = this.animatedRestFrame + 1;
			this.SetFrame(this.frame);
			this.frameCounter = 0f;
		}
	}

	protected void SetFrame(int f)
	{
		this.sprite.SetLowerLeftPixel((float)(f * this.pixelSize), (float)this.pixelSize);
	}

	private void LateUpdate()
	{
		if (base.transform.lossyScale.x < 0f)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x * -1f, 1f, 1f);
		}
	}

	private void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.033f);
		if (this.animatedBubble)
		{
			if (!this.stay || this.frame != this.animatedRestFrame)
			{
				this.frameCounter += this.t;
			}
			if (this.frameCounter > 0.0334f)
			{
				if (this.slowlyRise)
				{
					base.transform.Translate(0f, (float)this.riseDirection, 0f);
				}
				this.frameCounter -= 0.0334f;
				this.frame++;
				this.SetFrame(this.frame);
				if (this.frame == this.animatedRestFrame)
				{
					this.frameCounter -= this.restTime;
				}
				else if (this.frame >= this.animatedFinishFrame)
				{
					base.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			this.counter += this.t;
			if (this.counter < this.life)
			{
				if (base.transform.parent.localScale.x < 0f)
				{
					base.transform.localScale = new Vector3(-1f, 1f, 1f);
				}
				else
				{
					base.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				if (this.counter < this.tweenTime)
				{
					this.m = this.counter / this.tweenTime;
					base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Lerp(this.yStart - 5f, this.yStart, this.m), base.transform.localPosition.z);
				}
				else if (this.counter < this.life - this.tweenTime)
				{
					this.m = 1f;
					base.transform.localPosition = new Vector3(base.transform.localPosition.x, this.yStart, base.transform.localPosition.z);
				}
				else
				{
					this.m = (this.life - this.counter) / this.tweenTime;
					base.transform.localPosition = new Vector3(base.transform.localPosition.x, Mathf.Lerp(this.yStart + 5f, this.yStart, this.m), base.transform.localPosition.z);
				}
				if (this.flash)
				{
					this.sinCounter += this.t * this.sinSpeed;
					this.sinM = global::Math.Sin(this.sinCounter);
				}
				else
				{
					this.sinM = 1f;
				}
				this.sprite.SetColor(new Color(1f, 1f, 1f, (this.sinM * 0.3f + 0.7f) * this.m + (1f - this.m) * this.m));
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	internal void StopBubble()
	{
		this.stay = false;
		this.frameCounter = (float)(this.animatedRestFrame + 1);
	}

	internal void RestartBubbleWithBoolParam(bool stay)
	{
		this.stay = stay;
		this.RestartBubble();
	}

	protected SpriteSM sprite;

	protected float counter;

	public float life = 1.5f;

	protected float sinCounter;

	public float sinSpeed = 12f;

	public float tweenTime = 0.18f;

	protected float t = 0.01f;

	public bool flash;

	protected float yStart;

	protected float m;

	protected float sinM;

	public bool animatedBubble;

	public int animatedRestFrame = 8;

	public int animatedFinishFrame = 15;

	protected int frame;

	protected float frameCounter;

	protected int pixelSize = 32;

	public float restTime = 0.5f;

	public float delayOnFirstFrame;

	protected bool restartedBubble;

	public bool slowlyRise;

	protected int riseDirection = 1;

	protected bool stay;
}
