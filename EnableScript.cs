// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class EnableScript : MonoBehaviour
{
	private void Awake()
	{
		this.gameObjectToActive.SetActive(false);
	}

	private void Update()
	{
		this.delay -= Time.deltaTime;
		if (this.delay < 0f)
		{
			this.gameObjectToActive.SetActive(true);
		}
	}

	public float delay;

	public GameObject gameObjectToActive;
}
