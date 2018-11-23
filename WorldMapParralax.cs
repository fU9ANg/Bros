// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapParralax : MonoBehaviour
{
	private void Start()
	{
		if (this.followTransform == null)
		{
			this.followTransform = Camera.main.transform;
		}
		this.followStartPosition = this.followTransform.position;
		this.startPosition = base.transform.position;
		if (this.shadowTransform != null)
		{
			this.shadowTransform.transform.parent = base.transform.parent;
			this.shadowTransform.transform.position = new Vector3(this.shadowTransform.position.x, 0.01f, this.shadowTransform.position.z);
		}
	}

	private void Update()
	{
		Vector3 a = new Vector3(this.followTransform.position.x, 0.01f, this.followTransform.position.z) - new Vector3(base.transform.position.x, 0.01f, base.transform.position.z);
		base.transform.position = this.startPosition + a * this.followM;
	}

	public Transform followTransform;

	public float followM = 0.1f;

	protected Vector3 startPosition;

	protected Vector3 followStartPosition;

	public Transform shadowTransform;
}
