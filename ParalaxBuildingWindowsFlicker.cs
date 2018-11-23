// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ParalaxBuildingWindowsFlicker : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.windows.Length; i++)
		{
			this.windows[i].SetActive(false);
		}
		int num = UnityEngine.Random.Range(0, (int)((float)this.windows.Length / 4f));
		for (int j = 0; j < num; j++)
		{
			this.windows[UnityEngine.Random.Range(0, this.windows.Length)].SetActive(true);
		}
	}

	private void Update()
	{
		this.turnOnCounter += Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.turnOnCounter > 2f)
		{
			this.turnOnCounter = -UnityEngine.Random.value * 50f;
			this.currentWindow = UnityEngine.Random.Range(0, this.windows.Length);
			if (!this.windows[this.currentWindow].activeInHierarchy)
			{
				if (UnityEngine.Random.value > 0.5f)
				{
					this.flickerCounter = 0.2f + UnityEngine.Random.value * 0.4f;
				}
				else
				{
					this.windows[this.currentWindow].SetActive(true);
					this.currentWindow = -1;
				}
			}
		}
		this.turnOffCounter += Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.turnOffCounter > 1f)
		{
			this.turnOffCounter = -UnityEngine.Random.value * 12f;
			this.windows[UnityEngine.Random.Range(0, this.windows.Length)].SetActive(false);
		}
		if (this.flickerCounter > 0f)
		{
			if (this.flickerCounter > 0.3f)
			{
				this.flickerCounter -= Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
				if (this.flickerCounter <= 0.2f)
				{
					this.flickerCounter -= UnityEngine.Random.value * 0.0666f;
					if (this.currentWindow >= 0)
					{
						this.windows[this.currentWindow].SetActive(false);
					}
				}
			}
			else if (this.flickerCounter > 0.2f)
			{
				this.flickerCounter -= Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
				if (this.flickerCounter <= 0.2f)
				{
					this.flickerCounter -= UnityEngine.Random.value * 0.0666f;
					if (this.currentWindow >= 0)
					{
						this.windows[this.currentWindow].SetActive(true);
					}
				}
			}
			else if (this.flickerCounter > 0.1f)
			{
				this.flickerCounter -= Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
				if (this.flickerCounter <= 0.1f)
				{
					this.flickerCounter -= UnityEngine.Random.value * 0.0666f;
					if (this.currentWindow >= 0)
					{
						this.windows[this.currentWindow].SetActive(false);
					}
				}
			}
			else if (this.flickerCounter > 0f)
			{
				this.flickerCounter -= Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
				if (this.flickerCounter <= 0f)
				{
					this.flickerCounter -= UnityEngine.Random.value * 0.0666f;
					if (this.currentWindow >= 0)
					{
						this.windows[this.currentWindow].SetActive(true);
					}
					this.currentWindow = -1;
				}
			}
		}
	}

	protected float turnOnCounter;

	protected float turnOffCounter;

	protected float flickerCounter;

	protected int currentWindow;

	public GameObject[] windows;
}
