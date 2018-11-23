// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MookAlertIcon : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		base.gameObject.SetActive(false);
	}

	public void SetPosition(float newTargetX)
	{
		this.targetX = newTargetX;
	}

	public void Appear()
	{
		if (!base.gameObject.activeSelf)
		{
			this.appearDelay = 0.1f;
			this.x = this.targetX;
			this.SetPosition();
			this.scale = 0.6f;
			this.scaleI = 2f;
			base.gameObject.SetActive(true);
			base.GetComponent<Renderer>().enabled = false;
			this.appearing = true;
			this.disappearing = false;
			Puff puff = EffectsController.CreatePuffDisappearRingEffect(base.transform.position.x, base.transform.position.y - 2f, 0f, 0f);
			if (puff != null)
			{
				puff.gameObject.layer = base.gameObject.layer;
				puff.transform.parent = base.transform.parent;
			}
			Puff puff2 = EffectsController.CreatePuffPeckShineEffect(base.transform.position.x, base.transform.position.y - 2f, 0f, 0f);
			if (puff2 != null)
			{
				puff2.transform.parent = base.transform.parent;
				puff2.gameObject.layer = base.gameObject.layer;
				puff2.transform.localPosition = base.transform.localPosition + new Vector3(0f, 0f, 6f);
			}
			base.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
		}
	}

	public void Disappear()
	{
		if (!this.disappearing)
		{
			this.scale = 1.25f;
			this.scaleI = -1f;
			this.appearing = false;
			this.disappearing = true;
			base.GetComponent<Renderer>().enabled = true;
			base.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
		}
	}

	protected void SetPosition()
	{
		base.transform.localPosition = new Vector3(this.x, 0f, 0f);
	}

	private void Update()
	{
		if (this.disappearing)
		{
			this.scaleI -= this.scale * Time.deltaTime * 12f;
			this.scale += this.scaleI * Time.deltaTime;
			if (this.scale <= 0.5f)
			{
				base.gameObject.SetActive(false);
			}
			base.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
			this.sprite.SetColor(new Color(1f, 1f, 1f, Mathf.Clamp01(this.scale)));
		}
		else
		{
			if (this.targetX > this.x)
			{
				this.x += Time.deltaTime * 20f;
				if (this.x > this.targetX)
				{
					this.x = this.targetX;
				}
			}
			else if (this.targetX < this.x)
			{
				this.x -= Time.deltaTime * 20f;
				if (this.x < this.targetX)
				{
					this.x = this.targetX;
				}
			}
			this.SetPosition();
			if (this.appearing)
			{
				if (this.appearDelay > 0f)
				{
					this.appearDelay -= Time.deltaTime;
					if (this.appearDelay <= 0f)
					{
						base.GetComponent<Renderer>().enabled = true;
					}
				}
				else
				{
					this.scaleI += (1f - this.scale) * Time.deltaTime * 12f;
					this.scale += this.scaleI * 10f * Time.deltaTime;
					this.scaleI *= 1f - Time.deltaTime * 6f;
					if (this.scale <= 1f && this.scaleI < 0f)
					{
						this.appearing = false;
						this.scale = 1f;
					}
					base.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
					this.sprite.SetColor(new Color(1f, 1f, 1f, Mathf.Clamp01(this.scale)));
				}
			}
		}
	}

	protected SpriteSM sprite;

	protected float counter;

	protected float scale = 0.1f;

	public float desiredScale;

	protected float scaleI;

	protected bool disappearing;

	protected bool appearing;

	protected float targetX;

	protected float x;

	protected float appearDelay;
}
