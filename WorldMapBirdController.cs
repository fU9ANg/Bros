// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapBirdController : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.birdCount; i++)
		{
			WorldMapFlockOfSeagulls worldMapFlockOfSeagulls = UnityEngine.Object.Instantiate(this.flockOfSeagullsPrefab) as WorldMapFlockOfSeagulls;
			worldMapFlockOfSeagulls.transform.position = new Vector3(-5f + UnityEngine.Random.value * 10f, this.birdHeight, -3f + UnityEngine.Random.value * 6f);
			worldMapFlockOfSeagulls.transform.parent = base.transform;
			worldMapFlockOfSeagulls.helicopterTransform = this.helicopterTransform;
		}
	}

	private void Update()
	{
	}

	public float birdHeight = 6f;

	public int birdCount = 24;

	public WorldMapFlockOfSeagulls flockOfSeagullsPrefab;

	public Transform helicopterTransform;
}
