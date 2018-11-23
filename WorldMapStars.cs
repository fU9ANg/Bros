// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapStars : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < this.starCount; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.starPrefab) as GameObject;
			gameObject.transform.position = UnityEngine.Random.onUnitSphere * this.starHeight;
			gameObject.transform.parent = base.transform;
		}
		for (int j = 0; j < this.starBlueCount; j++)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(this.starBluePrefab) as GameObject;
			gameObject2.transform.position = UnityEngine.Random.onUnitSphere * this.starHeight;
			gameObject2.transform.parent = base.transform;
		}
	}

	private void Update()
	{
		if (this.parallaxM > 0f)
		{
			base.transform.localEulerAngles = base.transform.parent.eulerAngles * this.parallaxM;
		}
	}

	public GameObject starPrefab;

	public GameObject starBluePrefab;

	public float starHeight = 8f;

	public int starCount = 80;

	public int starBlueCount = 80;

	public float parallaxM;
}
