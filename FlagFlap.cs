// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FlagFlap : MonoBehaviour
{
	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.yTarget = base.transform.localPosition.y;
		this.y = this.yTarget - 32f;
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, this.y, base.transform.localPosition.z);
	}

	private void Update()
	{
		this.frameCounter += Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.frameCounter > 0.027f)
		{
			this.frameCounter -= 0.027f;
			this.frame++;
			if (this.frame >= this.frameCount)
			{
				this.frame = 0;
			}
			this.sprite.SetLowerLeftPixel((float)(this.frame * 32), 32f);
		}
		if (this.y < this.yTarget)
		{
			this.y += 130f * Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
			if (this.y > this.yTarget)
			{
				this.y = this.yTarget;
			}
			base.transform.localPosition = new Vector3(base.transform.localPosition.x, this.y, base.transform.localPosition.z);
		}
	}

	protected SpriteSM sprite;

	protected int frame;

	protected float frameCounter;

	protected float yTarget;

	private float y;

	protected int frameCount = 33;
}
