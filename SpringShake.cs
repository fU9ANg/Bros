// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SpringShake : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		base.transform.localPosition -= this.offset;
		this.acceleration = -this.stiffness * this.offset - this.damping * this.velocity;
		this.velocity += this.acceleration * deltaTime;
		this.offset += this.velocity * deltaTime;
		base.transform.localPosition += this.offset;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			base.transform.localPosition -= this.offset;
			this.offset = UnityEngine.Random.insideUnitCircle.normalized * this.debug;
			base.transform.localPosition += this.offset;
			MonoBehaviour.print("Offset " + this.offset);
		}
	}

	public void Shake(Vector3 newOffset)
	{
		base.transform.localPosition -= this.offset;
		this.offset = newOffset;
		base.transform.localPosition += this.offset;
	}

	private Vector3 acceleration = Vector3.zero;

	private Vector3 velocity = Vector3.zero;

	public float stiffness = 200f;

	public float damping = 0.01f;

	private Vector3 offset = Vector3.zero;

	public float debug;
}
