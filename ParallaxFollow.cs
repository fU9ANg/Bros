// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ParallaxFollow : MonoBehaviour
{
	private void Start()
	{
		if (this.followTransform == null && SortOfFollow.GetInstance() != null)
		{
			this.followTransform = SortOfFollow.GetInstance().transform;
		}
		if (this.followTransform == null && Camera.main != null)
		{
			this.followTransform = Camera.main.transform;
		}
		if (!this.setup && this.followTransform != null)
		{
			this.SetFollow(this.followTransform);
		}
		base.transform.position += Vector3.forward * 0.001f * base.transform.position.y;
	}

	public void SetFollow(Transform followTransform)
	{
		this.followTransform = followTransform;
		this.startPos = followTransform.position;
		Vector3 vector = this.startPos - base.transform.position;
		this.originalPos = base.transform.position + new Vector3(vector.x * this.parallaxXM, vector.y * this.parallaxYM, 0f);
		this.setup = true;
	}

	private void LateUpdate()
	{
		if (this.followTransform != null)
		{
			float x = base.transform.position.x;
			float y = base.transform.position.y;
			if (this.parallaxXM != 0f)
			{
				x = this.originalPos.x + (this.followTransform.position - this.startPos).x * this.parallaxXM + ParallaxController.ParallaxXOffset / this.parallaxXM;
			}
			if (this.parallaxYM != 0f)
			{
				y = this.originalPos.y + (this.followTransform.position - this.startPos).y * this.parallaxYM;
			}
			base.transform.position = new Vector3(x, y, base.transform.position.z);
		}
	}

	private Vector3 startPos;

	private Vector3 originalPos;

	public Transform followTransform;

	public float parallaxXM = 0.5f;

	public float parallaxYM = 0.5f;

	protected bool setup;
}
