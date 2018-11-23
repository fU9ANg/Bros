// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class GlowPingText : MonoBehaviour
{
	private void Awake()
	{
		this.textMesh = base.GetComponent<TextMesh>();
	}

	public void Ping(Color color)
	{
		this.mainColor = color;
		this.counter = 0f;
		this.textMesh.color = Color.Lerp(this.pingColor, this.mainColor, this.counter / this.pingTime);
	}

	public void Ping()
	{
		this.counter = 0f;
		this.textMesh.color = Color.Lerp(this.pingColor, this.mainColor, this.counter / this.pingTime);
	}

	private void Update()
	{
		this.textMesh.color = Color.Lerp(this.mainColor, this.pingColor, ((!this.wibble) ? 1f : (Mathf.Clamp01(this.counter - 1f) + this.counter + 0.5f + Mathf.Sin(this.counter * 50f) * 0.5f)) * (1f - this.counter / this.pingTime));
		this.counter += Time.deltaTime;
	}

	public Color pingColor = Color.white;

	public Color mainColor = Color.grey;

	protected TextMesh textMesh;

	protected float counter;

	public float pingTime = 1f;

	public bool wibble = true;
}
