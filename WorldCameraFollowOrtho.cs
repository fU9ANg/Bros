// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldCameraFollowOrtho : MonoBehaviour
{
	private void Start()
	{
	}

	private void LateUpdate()
	{
		if (base.GetComponent<Camera>().orthographic)
		{
			base.GetComponent<Camera>().orthographicSize = this.followCamera.orthographicSize;
		}
		else
		{
			base.GetComponent<Camera>().fieldOfView = this.followCamera.fieldOfView;
			base.GetComponent<Camera>().transform.position = this.followCamera.transform.position;
		}
	}

	public Camera followCamera;
}
