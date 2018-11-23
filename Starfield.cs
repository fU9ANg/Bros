// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Starfield : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < 50; i++)
		{
			GameObject gameObject;
			switch (UnityEngine.Random.Range(0, 4))
			{
			case 0:
				gameObject = (UnityEngine.Object.Instantiate(this.star1, new Vector3((float)(-200 + UnityEngine.Random.Range(0, 400)), (float)(-190 + UnityEngine.Random.Range(0, 350)), 80f), Quaternion.identity) as GameObject);
				break;
			case 1:
				gameObject = (UnityEngine.Object.Instantiate(this.star2, new Vector3((float)(-200 + UnityEngine.Random.Range(0, 400)), (float)(-190 + UnityEngine.Random.Range(0, 350)), 80f), Quaternion.identity) as GameObject);
				break;
			case 2:
				gameObject = (UnityEngine.Object.Instantiate(this.star3, new Vector3((float)(-200 + UnityEngine.Random.Range(0, 400)), (float)(-190 + UnityEngine.Random.Range(0, 350)), 80f), Quaternion.identity) as GameObject);
				break;
			default:
				gameObject = (UnityEngine.Object.Instantiate(this.star4, new Vector3((float)(-200 + UnityEngine.Random.Range(0, 400)), (float)(-190 + UnityEngine.Random.Range(0, 350)), 80f), Quaternion.identity) as GameObject);
				break;
			}
			gameObject.transform.parent = base.transform;
		}
	}

	private void Update()
	{
	}

	public GameObject star1;

	public GameObject star2;

	public GameObject star3;

	public GameObject star4;
}
