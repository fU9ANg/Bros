// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Shake : MonoBehaviour
{
	public void AddShake(float amplitude, float freqX, float freqY)
	{
		this.amplitudeX = amplitude;
		this.amplitudeY = amplitude;
		this.frequencyX = (32f + UnityEngine.Random.value * 34f) * freqX;
		this.frequencyY = (30f + UnityEngine.Random.value * 34f) * freqY;
		this.counterX = 3.14159274f * freqX;
		this.counterY = 3.14159274f * freqY;
	}

	public void AddShake2(float AmplitudeX, float AmplitudeY, float freq)
	{
		this.amplitudeX = Mathf.Max(this.amplitudeX, AmplitudeX);
		this.amplitudeY = Mathf.Max(this.amplitudeY, AmplitudeY);
		float b = UnityEngine.Random.Range(0.9f, 1.1f) * freq;
		float b2 = UnityEngine.Random.Range(0.9f, 1.1f) * freq;
		this.frequencyX = Mathf.Max(this.frequencyX, b);
		this.frequencyY = Mathf.Max(this.frequencyY, b2);
		this.counterX = 3.14159274f;
		this.counterY = 3.14159274f;
	}

	private void Awake()
	{
		this.counterX = UnityEngine.Random.value * 3.14159274f * 2f;
		this.counterY = UnityEngine.Random.value * 3.14159274f * 2f;
		global::Math.SetupLookupTables();
	}

	private void LateUpdate()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.counterX += this.frequencyX * num;
		this.counterY += this.frequencyY * num;
		if (this.applyDampimg)
		{
			if (this.frequencyX > 0f)
			{
				this.frequencyX *= 1f - num * 4f;
				if (this.frequencyX < 0.01f)
				{
					this.frequencyX = 0f;
				}
			}
			if (this.frequencyY > 0f)
			{
				this.frequencyY *= 1f - num * 4f;
				if (this.frequencyY < 0.01f)
				{
					this.frequencyY = 0f;
				}
			}
		}
		if (this.amplitudeX > 0f || this.amplitudeY > 0f)
		{
			if (this.applyDampimg)
			{
				this.amplitudeX -= num * this.damping;
				if (this.amplitudeX <= 0f)
				{
					this.amplitudeX = 0f;
				}
				this.amplitudeY -= num * this.damping;
				if (this.amplitudeY <= 0f)
				{
					this.amplitudeY = 0f;
				}
			}
			base.transform.position -= this.offset;
			this.offset = new Vector3(global::Math.Sin(this.counterX) * this.amplitudeX, global::Math.Sin(this.counterY) * this.amplitudeY, 0f);
			base.transform.position += this.offset;
		}
	}

	protected Vector3 offset;

	public float frequencyX;

	public float frequencyY;

	protected float counterX;

	protected float counterY;

	public float amplitudeX;

	public float amplitudeY;

	public float damping = 2f;

	public bool applyDampimg = true;
}
