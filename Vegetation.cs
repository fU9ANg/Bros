// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Vegetation : MonoBehaviour
{
	public void Rustle(float rustleX, float rustleY, float xI)
	{
		if (Mathf.Abs(this.x - rustleX) < this.size && Mathf.Abs(this.y - rustleY) < this.size)
		{
			this.Rustle(xI);
		}
	}

	protected void Rustle(float xI)
	{
		UnityEngine.Debug.Log("Ruslte");
	}

	private void Awake()
	{
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
	}

	protected virtual void Update()
	{
		if (this.rustling)
		{
			this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		}
	}

	protected float x;

	protected float y;

	public float size = 9f;

	protected float rustleAmount;

	protected bool rustling;

	protected float t = 0.01f;

	public float bounciness = 1f;

	public bool clockwiseIsRight = true;

	public Transform[] rotateTransforms;
}
