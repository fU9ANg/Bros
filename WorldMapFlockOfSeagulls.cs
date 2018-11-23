// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapFlockOfSeagulls : MonoBehaviour
{
	private void Start()
	{
		this.targetPoint = this.GetRandomTargetPoint();
		this.currentTargetPoint = this.targetPoint;
		for (int i = 0; i < this.seagulls.Length; i++)
		{
			if (UnityEngine.Random.value > 0.5f)
			{
				UnityEngine.Object.Destroy(this.seagulls[i].gameObject);
			}
			else
			{
				this.seagulls[i].transform.parent = base.transform.parent;
				this.seagulls[i].avoidTransform = this.helicopterTransform;
			}
		}
	}

	protected Vector3 GetRandomTargetPoint()
	{
		return new Vector3(this.minX + UnityEngine.Random.value * (this.maxX - this.minX), this.flyHeight, this.minZ + UnityEngine.Random.value * (this.maxZ - this.minZ));
	}

	private void Update()
	{
		this.currentTargetPoint += (this.targetPoint - this.currentTargetPoint).normalized * this.flySpeed * Time.deltaTime;
		base.transform.position += (this.currentTargetPoint - base.transform.position).normalized * this.flySpeed * Time.deltaTime;
		this.flyCounter += Time.deltaTime;
		if (this.flyCounter > 4f || (this.currentTargetPoint - this.targetPoint).magnitude < 1f)
		{
			this.flyCounter = 0f;
			Vector3 randomTargetPoint = this.GetRandomTargetPoint();
			this.targetPoint = base.transform.position + (randomTargetPoint - base.transform.position).normalized * 5f;
			this.targetPoint = new Vector3(this.targetPoint.x, this.flyHeight, this.targetPoint.z);
		}
	}

	public float flyHeight = 5.4f;

	public float flySpeed = 0.5f;

	protected float flyCounter;

	protected Vector3 targetPoint = Vector3.up * 5f;

	protected Vector3 currentTargetPoint = Vector3.up * 0.5f;

	public WorldMapBird[] seagulls;

	public Transform helicopterTransform;

	public float minX = -3f;

	public float maxX = 3f;

	public float minZ = -3f;

	public float maxZ = 3f;
}
