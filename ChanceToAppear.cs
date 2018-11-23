// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ChanceToAppear : MonoBehaviour
{
	private void Start()
	{
		if (UnityEngine.Random.value > this.chanceToAppear)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			for (int i = 0; i < this.otherObjectsThatMustNotAppear.Length; i++)
			{
				UnityEngine.Object.Destroy(this.otherObjectsThatMustNotAppear[i]);
			}
		}
		base.enabled = false;
	}

	public float chanceToAppear = 0.5f;

	public GameObject[] otherObjectsThatMustNotAppear;
}
