// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TextGlowOnEnabled : MonoBehaviour
{
	private void Start()
	{
		global::Math.SetupLookupTables();
		this.textMesh = base.GetComponent<TextMesh>();
		if (this.textMesh != null)
		{
			this.baseColor = this.textMesh.GetComponent<Renderer>().material.color;
		}
	}

	private void OnEnable()
	{
		if (this.textMesh != null)
		{
			this.glowing = true;
			this.textMesh.GetComponent<Renderer>().material.color = this.glowColor;
			this.counter = 1f;
		}
	}

	private void Update()
	{
		if (this.textMesh != null && this.glowing)
		{
			float t = 1f - Mathf.Clamp(0.5f + global::Math.Sin(1.57079637f + this.counter * this.counter * 12f) * 0.5f, 0f, 1f) * this.counter;
			Color color = Color.Lerp(this.glowColor, this.baseColor, t);
			this.counter -= Time.deltaTime * 0.6f;
			if (this.counter <= 0f)
			{
				this.glowing = false;
				color = this.baseColor;
			}
			this.textMesh.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, 1f);
		}
	}

	public Color glowColor = Color.yellow;

	protected Color baseColor = Color.white;

	protected float counter;

	protected bool glowing;

	protected TextMesh textMesh;
}
