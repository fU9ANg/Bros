// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class OutOfDateObject : MonoBehaviour
{
	private void Start()
	{
		this.outOfDateObject.SetActive(false);
	}

	private void Update()
	{
		this.counter += Time.deltaTime;
		if (this.counter > 0.3f)
		{
			this.counter -= 0.3f;
			if (PlaytomicController.IsThisBuildOutOfDate())
			{
				this.outOfDateObject.SetActive(true);
			}
		}
	}

	public GameObject outOfDateObject;

	public bool isForBeta = true;

	private float counter;
}
