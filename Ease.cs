// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Ease : MonoBehaviour
{
	public void Awake()
	{
	}

	private void Update()
	{
		if (this.startDelay > 0f)
		{
			this.startDelay -= Time.deltaTime;
		}
		else
		{
			this.lerp += Time.deltaTime * this.rate;
		}
		float num = Easer.Ease(this._ease, 0f, 1f, this.lerp);
		base.transform.position -= this.offset;
		this.offset = this.startOffset * (1f - num);
		base.transform.position += this.offset;
		base.transform.localScale -= this.scaleOffset;
		this.scaleOffset = this.scale * num * Vector3.one;
		base.transform.localScale += this.scaleOffset;
		base.transform.rotation *= Quaternion.Inverse(this.rotOffset);
		this.rotOffset = Quaternion.AngleAxis(this.rotationOffset * num, Vector3.forward);
		base.transform.rotation *= this.rotOffset;
		if (this.lerp >= 1f)
		{
			MonoBehaviour.print("Disable");
			base.enabled = false;
		}
	}

	public Vector3 startOffset;

	public float scale;

	private bool setTargetPos;

	public EaserEase _ease;

	private float lerp;

	public float rate = 1f;

	private Vector3 offset = Vector3.zero;

	private Vector3 scaleOffset = Vector3.zero;

	public float rotationOffset;

	public float startDelay;

	private Quaternion rotOffset = Quaternion.identity;
}
