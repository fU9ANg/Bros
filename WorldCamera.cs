// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldCamera : MonoBehaviour
{
	private void Awake()
	{
		this.orthoSize = base.GetComponent<Camera>().orthographicSize;
		WorldCamera.instance = this;
	}

	private void Start()
	{
		if (this.targetLocation != null)
		{
			this.startPosition = this.targetLocation.position;
		}
		else
		{
			this.startPosition = base.transform.position;
		}
		this.SetPosition(1f);
		this.CalculateCameraExtents();
		if (this.followTransform == null)
		{
			base.transform.position = new Vector3(base.transform.position.x, this.heightOffGround, base.transform.position.z + this.zOffset);
		}
		else
		{
			base.transform.position = this.ClampView(new Vector3(this.followTransform.position.x, this.heightOffGround, this.followTransform.position.z + this.zOffset));
		}
	}

	public void SetZoom(float zoom)
	{
		if (!this.lerpingToLocation)
		{
			this.currentZoom = zoom;
			base.GetComponent<Camera>().orthographicSize = this.orthoSize * (1f / this.currentZoom);
		}
	}

	public void SetStartingPositionAndZoom(Vector3 position, float zoom)
	{
		position.z = -100f;
		Vector3 position2 = position;
		base.transform.position = position2;
		this.startPosition = position2;
		this.currentZoom = zoom;
		this.startZoom = zoom;
		this.SetPosition(1f);
		this.lerpDuration = 1f; this.lerpC = (this.lerpDuration );
		this.lerpingToLocation = false;
	}

	public void GotoLocation(Transform location, float zoomLevel, float lerpDuration)
	{
		this.startPosition = base.transform.position;
		this.lerpC = 0f;
		this.lerpDuration = lerpDuration;
		this.targetLocation = location;
		this.startZoom = this.currentZoom;
		this.lerpingToLocation = true;
		this.startZoom = this.currentZoom;
		this.targetZoom = zoomLevel;
	}

	public void GoToZoom(float z)
	{
		this.startZoom = this.currentZoom;
		this.targetZoom = z;
	}

	public static void AddLerpTime(float time)
	{
		WorldCamera.instance.lerpDuration += time;
	}

	protected void SetPosition(float m)
	{
		if (this.targetLocation != null)
		{
			base.transform.position = new Vector3(Mathf.Lerp(this.startPosition.x, this.targetLocation.position.x, m), Mathf.Lerp(this.startPosition.y, this.targetLocation.position.y, m), -100f);
		}
	}

	private void LateUpdate()
	{
		base.GetComponent<Camera>().orthographicSize = this.orthoSize * (1f / this.currentZoom);
		this.CalculateCameraExtents();
		if (this.lerpingToLocation)
		{
			if (this.lerpC < this.lerpDuration)
			{
				this.lerpC += Time.deltaTime;
				float num = this.lerpC / this.lerpDuration;
				base.transform.position = Vector3.Lerp(base.transform.position, this.ClampView(new Vector3(this.targetLocation.position.x, this.heightOffGround, this.targetLocation.position.z + this.zOffset)), Time.deltaTime * 2f);
				this.currentZoom = Mathf.Lerp(this.currentZoom, this.targetZoom, Time.deltaTime * 1f);
				if (this.lerpC >= this.lerpDuration)
				{
					this.lerpingToLocation = false;
				}
			}
		}
		else if (this.followTransform != null)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.ClampView(new Vector3(this.followTransform.position.x, this.heightOffGround, this.followTransform.position.z + this.zOffset)), Time.deltaTime * (5f * this.currentZoom));
		}
	}

	protected void CalculateCameraExtents()
	{
		Vector3 vector = base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(0f, 0f, this.zOffset * 0.8f));
		Vector3 vector2 = base.GetComponent<Camera>().ScreenToWorldPoint(new Vector3((float)Screen.width, (float)Screen.height, this.zOffset * 1.4f));
		this.xCameraWidth = base.transform.position.x - vector.x;
		this.zCameraLowerDepth = base.transform.position.z - vector.z;
		this.zCameraUpperDepth = vector2.z - base.transform.position.z;
	}

	protected Vector3 ClampView(Vector3 desiredPos)
	{
		return new Vector3(Mathf.Clamp(desiredPos.x, this.worldMinX + this.xCameraWidth + 0.2f, this.worldMaxX - this.xCameraWidth - 0.2f), desiredPos.y, Mathf.Clamp(desiredPos.z, this.worldMinZ + this.zCameraLowerDepth + 0.2f, this.worldMaxZ - this.zCameraUpperDepth - 0.2f));
	}

	public Transform targetLocation;

	protected Vector3 startPosition;

	public float lerpC;

	protected bool lerpingToLocation;

	protected float lerpDuration = 1f;

	protected float targetZoom = 1f;

	public float currentZoom = 1f;

	protected float startZoom = 1f;

	protected float orthoSize = 48f;

	public float heightOffGround = 6.6f;

	public float zOffset = -1f;

	protected float xCameraWidth = 2f;

	protected float zCameraUpperDepth = 2f;

	protected float zCameraLowerDepth = 2f;

	public float worldMinX = -4f;

	public float worldMaxX = 4f;

	public float worldMinZ = -3f;

	public float worldMaxZ = 3f;

	protected static WorldCamera instance;

	public Transform followTransform;
}
