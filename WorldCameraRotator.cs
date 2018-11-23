// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldCameraRotator : MonoBehaviour
{
	private void Start()
	{
		this.fovSize = this.worldCamera.fieldOfView;
	}

	private void LateUpdate()
	{
		if (this.cameraFollowTarget != null)
		{
			this.targetLookTarget = Vector3.RotateTowards(this.cameraFollowTarget.localPosition.normalized, Vector3.down, 0.2f, 0.15f);
		}
		if (Mathf.Abs((this.targetLookTarget - this.currentLookTarget).x) > 0.001f || Mathf.Abs((this.targetLookTarget - this.currentLookTarget).y) > 0.001f || Mathf.Abs((this.targetLookTarget - this.currentLookTarget).z) > 0.001f)
		{
			this.currentLookTarget = Vector3.RotateTowards(this.currentLookTarget, this.targetLookTarget, Time.deltaTime * 1f, Time.deltaTime);
			float y = global::Math.GetAngle(this.currentLookTarget.x, this.currentLookTarget.z) / 3.14159274f * 180f + 90f;
			float x = global::Math.GetAngle(this.currentLookTarget.y, 1f) / 3.14159274f * 180f - 90f;
			this.worldYDummyTransform.LookAt(new Vector3(this.currentLookTarget.x, 0f, -this.currentLookTarget.z), Vector3.up);
			this.worldXDummyTransform.LookAt(new Vector3(0f, this.currentLookTarget.y, -this.currentLookTarget.z), Vector3.up);
			this.worldXTransform.localEulerAngles = new Vector3(x, 0f, 0f);
			this.worldYTransform.localEulerAngles = new Vector3(0f, y, 0f);
		}
		else if (this.holdingMouse)
		{
			Vector3 a = Input.mousePosition - this.mouseStart;
			Vector3 vector = a / (float)Screen.width;
			this.worldXTransform.localEulerAngles = new Vector3(this.mouseStartEulerAngleX + vector.y * this.mouseMultiplierSpeed, 0f, 0f);
			this.worldYTransform.localEulerAngles = new Vector3(0f, this.mouseStartEulerAngleY - vector.x * this.mouseMultiplierSpeed, 0f);
			this.currentLookTarget = this.worldActualTransform.InverseTransformPoint(Vector3.back).normalized;
			this.targetLookTarget = this.currentLookTarget;
		}
	}

	private void Update()
	{
		this.left = Input.GetKey(KeyCode.LeftArrow);
		this.right = Input.GetKey(KeyCode.RightArrow);
		this.up = Input.GetKey(KeyCode.UpArrow);
		this.down = Input.GetKey(KeyCode.DownArrow);
		if (Input.GetMouseButtonDown(0))
		{
			this.mouseStart = Input.mousePosition;
			this.mouseStartEulerAngleX = this.worldXTransform.localEulerAngles.x;
			this.mouseStartEulerAngleY = this.worldYTransform.localEulerAngles.y;
			this.holdingMouse = true;
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.holdingMouse = false;
		}
		if (Input.GetAxis("Mouse ScrollWheel") != 0f)
		{
			this.fovSize = Mathf.Clamp(this.fovSize - Input.GetAxis("Mouse ScrollWheel") * 10f, 30f, 65f);
			this.worldCamera.fieldOfView = this.fovSize;
			this.interfaceCamera.fieldOfView = this.fovSize;
		}
	}

	public void SetStartingPositionAndZoom(Vector3 vector3, float lastCameraZoom)
	{
		this.currentLookTarget = vector3.normalized * 0.9f;
		this.targetLookTarget = vector3.normalized;
	}

	internal void GotoLocation(Transform location, float p)
	{
		UnityEngine.Debug.Log(" targetLookTarget " + this.targetLookTarget);
		this.targetLookTarget = this.worldActualTransform.InverseTransformPoint(location.position).normalized;
	}

	internal void GoToZoom(float zoomLevel)
	{
		throw new NotImplementedException();
	}

	protected bool holdingMouse;

	protected Vector3 mouseStart = Vector3.zero;

	protected float mouseStartEulerAngleX;

	protected float mouseStartEulerAngleY;

	public float mouseMultiplierSpeed = 60f;

	public float moveSpeed = 60f;

	protected bool left;

	protected bool right;

	protected bool up;

	protected bool down;

	public Transform worldXTransform;

	public Transform worldYTransform;

	public Transform worldXDummyTransform;

	public Transform worldYDummyTransform;

	public Transform worldActualTransform;

	public Camera worldCamera;

	public Camera interfaceCamera;

	public Vector3 currentLookTarget = Vector3.back;

	public Vector3 targetLookTarget = Vector3.back;

	public float currentZoom = 1f;

	public Transform cameraFollowTarget;

	public float fovSize = 60f;
}
