// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Autodestruct : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		this.countDown -= Time.deltaTime;
		if (this.countDown < 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public float countDown = 1f;
}
