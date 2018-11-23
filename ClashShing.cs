// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ClashShing : MonoBehaviour
{
	private void Start()
	{
		int num = UnityEngine.Random.Range(this.minShings, this.maxShings + 1);
		float num2 = UnityEngine.Random.value * 360f;
		for (int i = 0; i < num; i++)
		{
			Shing shing = UnityEngine.Object.Instantiate(this.shingPrefab, base.transform.position, Quaternion.identity) as Shing;
			this.shings.Add(shing);
			shing.transform.parent = base.transform;
			float value = UnityEngine.Random.value;
			shing.rotationSpeed = this.rotationSpeed * (1.5f - value);
			shing.xScale = 60f * (0.5f + value * 0.5f) * (0.5f + value * 0.5f);
			shing.life = this.life * value;
			shing.rotation = num2 + (float)i * 360f / (float)num + UnityEngine.Random.value * 360f / (float)(num + 1);
			shing.transform.localScale = new Vector3(shing.xScale, 400f, 1f);
			shing.transform.eulerAngles = new Vector3(0f, 0f, shing.rotation);
		}
		DistortionGrow distortionGrow = UnityEngine.Object.Instantiate(this.ringPrefab, base.transform.position, Quaternion.identity) as DistortionGrow;
		distortionGrow.transform.localScale = Vector3.one * (0.8f + UnityEngine.Random.value * 0.4f);
	}

	private void Update()
	{
		float deltaTime = Time.deltaTime;
		for (int i = 0; i < this.shings.Count; i++)
		{
			if (this.shings[i] != null)
			{
				this.shings[i].life += deltaTime * 0.1f;
				this.shings[i].xScale -= 60f / this.shings[i].life * deltaTime;
				if (this.shings[i].xScale <= 0f)
				{
					UnityEngine.Object.Destroy(this.shings[i].gameObject);
					this.shings[i] = null;
				}
				else
				{
					this.shings[i].rotation += this.shings[i].rotationSpeed * deltaTime;
					this.shings[i].rotationSpeed *= 1f - deltaTime * 3f;
					this.shings[i].transform.localScale = new Vector3(this.shings[i].xScale, 400f, 1f);
					this.shings[i].transform.eulerAngles = new Vector3(0f, 0f, this.shings[i].rotation);
				}
			}
		}
	}

	public Shing shingPrefab;

	public DistortionGrow ringPrefab;

	protected List<Shing> shings = new List<Shing>();

	public float life = 0.4f;

	public float rotationSpeed = -90f;

	public int minShings = 3;

	public int maxShings = 6;
}
