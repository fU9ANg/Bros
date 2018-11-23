// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PanningStuff : MonoBehaviour
{
	private void Start()
	{
		PanningStuff.instance = this;
	}

	public static void PanVertical(float amount)
	{
		if (PanningStuff.instance != null)
		{
			PanningStuff.instance.panning = true;
			PanningStuff.instance.panVerticalAmount = amount;
		}
	}

	private void Update()
	{
		if (this.panning)
		{
			if (this.panVerticalAmount > 80f)
			{
				this.yI = Mathf.Lerp(this.yI, 200f, Time.deltaTime * 4f);
			}
			else if (this.panVerticalAmount > 40f)
			{
				this.yI = Mathf.Lerp(this.yI, 50f, Time.deltaTime * 4f);
			}
			else if (this.panVerticalAmount > 0f)
			{
				this.yI = Mathf.Lerp(this.yI, 25f, Time.deltaTime * 4f);
			}
			else
			{
				this.yI = Mathf.Lerp(this.yI, 0f, Time.deltaTime * 4f);
			}
			float num = this.yI * Time.deltaTime;
			this.panVerticalAmount -= num;
			this.panFullSpeed.Translate(new Vector3(0f, num, 0f));
			this.panSlowly.Translate(new Vector3(0f, num * 0.25f, 0f));
			this.panVerySlowly.Translate(new Vector3(0f, num * 0.15f, 0f));
		}
	}

	protected static PanningStuff instance;

	public Transform panSlowly;

	public Transform panVerySlowly;

	public Transform panFullSpeed;

	protected float panVerticalAmount;

	protected bool panning;

	protected float yI;
}
