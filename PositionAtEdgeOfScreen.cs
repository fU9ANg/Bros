// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PositionAtEdgeOfScreen : MonoBehaviour
{
	private void Start()
	{
		float num = this.percentageToEdge / 100f;
		base.transform.parent = this.uiCamera.transform;
		switch (this.direction)
		{
		case DirectionEnum.Up:
			base.transform.position = this.uiCamera.ScreenToWorldPoint(new Vector3((float)Screen.width / 2f, (float)(Screen.height / 2) + (float)(Screen.height / 2) * num, this.cameraZOffset));
			break;
		case DirectionEnum.Down:
			base.transform.position = this.uiCamera.ScreenToWorldPoint(new Vector3((float)Screen.width / 2f, (float)(Screen.height / 2) - (float)(Screen.height / 2) * num, this.cameraZOffset));
			break;
		case DirectionEnum.Left:
			base.transform.position = this.uiCamera.ScreenToWorldPoint(new Vector3((float)(Screen.width / 2) - (float)(Screen.width / 2) * num, (float)(Screen.height / 2), this.cameraZOffset));
			break;
		case DirectionEnum.Right:
			base.transform.position = this.uiCamera.ScreenToWorldPoint(new Vector3((float)(Screen.width / 2) + (float)(Screen.width / 2) * num, (float)(Screen.height / 2), this.cameraZOffset));
			break;
		}
	}

	private void Update()
	{
	}

	public Camera uiCamera;

	public DirectionEnum direction;

	public float cameraZOffset = 10f;

	public float percentageToEdge = 90f;
}
