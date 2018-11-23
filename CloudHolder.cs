// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CloudHolder : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.cloudCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.cloudPrefab) as GameObject;
			gameObject.transform.position = UnityEngine.Random.onUnitSphere * this.cloudHeight;
			gameObject.transform.parent = base.transform;
		}
	}

	private void Update()
	{
		base.transform.Rotate(this.xRotationSpeed * Time.deltaTime, this.yRotationSpeed * Time.deltaTime, 0f, Space.World);
	}

	public float xRotationSpeed = 6f;

	public float yRotationSpeed = 36f;

	public GameObject cloudPrefab;

	public float cloudHeight = 6f;

	public int cloudCount = 15;
}
