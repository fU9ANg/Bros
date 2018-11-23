// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ZoomInCamera : MonoBehaviour
{
	private void Start()
	{
		this.targetOrthoSize = base.GetComponent<Camera>().orthographicSize;
		this.orthoSize = this.targetOrthoSize * 2f;
		base.GetComponent<Camera>().orthographicSize = this.orthoSize;
	}

	private void Update()
	{
		this.frameCount++;
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.frameCount > 3)
		{
			this.orthoSize = Mathf.Lerp(this.orthoSize, this.targetOrthoSize, num * this.zoomInLerp);
			base.GetComponent<Camera>().orthographicSize = this.orthoSize;
		}
	}

	protected float orthoSize = 300f;

	protected float targetOrthoSize = 160f;

	public float zoomInLerp = 25f;

	protected int frameCount;
}
