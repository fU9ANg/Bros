// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AnchorObjectToScreen : MonoBehaviour
{
	private void Update()
	{
		Vector3 position = this.anchoredCamera.ScreenToWorldPoint(new Vector3((float)Screen.width * this.screenXM, (float)Screen.height * this.screenYM, 5f));
		base.transform.position = position;
	}

	public Camera anchoredCamera;

	public float screenXM = 0.05f;

	public float screenYM = 0.05f;
}
