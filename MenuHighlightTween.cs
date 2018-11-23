// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MenuHighlightTween : MonoBehaviour
{
	public Vector3 TargetPos
	{
		get
		{
			return this.targetPos;
		}
	}

	public bool HasReachedTarget
	{
		get
		{
			return base.transform.position == this.targetPos;
		}
	}

	private void Start()
	{
		this.targetPos = base.transform.position;
	}

	public void SetTargetPos(Vector3 target, bool bounce = true)
	{
		if (this.targetPos != target)
		{
			if (bounce)
			{
				this.Bounce();
			}
			this.targetPos = target;
		}
	}

	public void SetTargetSize(Vector3 size)
	{
		if (this.targetSize != size)
		{
			this.targetSize = size;
			this.Bounce();
		}
	}

	private void Update()
	{
		this.frameDelay += Time.deltaTime;
		if (this.frameDelay > 0.05f)
		{
			this.frameDelay = 0f;
			if (this.animFrame < 6)
			{
				this.animFrame++;
			}
			else
			{
				this.animFrame--;
			}
			this.SetFrame();
		}
		if (this.bouncingHilight)
		{
			this.bouncingHilightCounter += Time.deltaTime * 2f;
			float num = Mathf.Clamp((0.7f - this.bouncingHilightCounter) * 3f, 0f, 1f);
			float num2 = 1f - num;
			float num3 = global::Math.Sin(this.bouncingHilightCounter * 3f + 1f / (0.1f + this.bouncingHilightCounter * this.bouncingHilightCounter * 2f));
			base.transform.localScale = new Vector3((1f + num3 * 0.1f) * num + num2, (1f + num3 * 0.3f) * num + num2, 1f);
		}
		base.transform.position = Vector3.Lerp(base.transform.position, this.targetPos, Time.deltaTime * 26f);
		if (Mathf.Abs((this.targetPos - base.transform.position).y) < 1f)
		{
			base.transform.position = this.targetPos;
		}
		if (this.boxHorizontals != null)
		{
			this.boxHorizontals.SetSize(this.targetSize.x + 32f, 32f);
			this.boxLeft.transform.localPosition = new Vector3(this.targetSize.x * -0.5f - 16f, 0f, 0f);
			this.boxRight.transform.localPosition = new Vector3(this.targetSize.x * 0.5f + 16f, 0f, 0f);
		}
	}

	protected void Bounce()
	{
		this.animFrame = 0;
		this.SetFrame();
		this.bouncingHilight = true;
		this.bouncingHilightCounter = 0f;
	}

	protected void SetFrame()
	{
		base.GetComponent<SpriteSM>().SetLowerLeftPixel((float)(this.animFrame * 512), 64f);
		base.GetComponent<SpriteSM>().UpdateUVs();
	}

	private const float frameRate = 0.05f;

	protected int highlightIndex;

	protected float bouncingHilightCounter;

	protected bool bouncingHilight;

	protected Vector3 targetPos = Vector3.zero;

	public bool bounceWidth = true;

	public float speed = 26f;

	public SpriteSM boxHorizontals;

	public SpriteSM boxLeft;

	public SpriteSM boxRight;

	private Vector3 targetSize;

	protected int animFrame;

	private float frameDelay;
}
