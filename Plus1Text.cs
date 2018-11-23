// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Plus1Text : MonoBehaviour
{
	private void Start()
	{
	}

	public void Setup()
	{
		this.x = base.transform.localPosition.x;
		this.y = base.transform.localPosition.y;
		base.gameObject.SetActive(false);
	}

	public void Restart()
	{
		this.counter = 2f;
		base.transform.localPosition = new Vector3(this.x, this.y, base.transform.localPosition.z);
		base.gameObject.SetActive(true);
	}

	public void Stop()
	{
		base.gameObject.SetActive(false);
	}

	private void Update()
	{
		base.transform.Translate(Vector3.up * 5f * Time.deltaTime);
		this.flashCounter += Time.deltaTime;
		if (this.flashCounter > 0.099f)
		{
			this.flashCounter -= 0.099f;
			if (this.colorM > 0.5f)
			{
				this.colorM = 0f;
			}
			else
			{
				this.colorM = 1f;
			}
		}
		this.counter -= Time.deltaTime * 1.5f;
		float num = Mathf.Clamp(this.counter * 2f, 0f, 1f);
		base.GetComponent<Renderer>().material.color = new Color(this.colorM, this.colorM, this.colorM, num);
		if (num <= 0f)
		{
			base.gameObject.SetActive(false);
		}
	}

	protected float x;

	protected float y;

	protected float counter;

	protected float flashCounter;

	private float colorM = 1f;
}
