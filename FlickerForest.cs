// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class FlickerForest : MonoBehaviour
{
	private IEnumerator Start()
	{
		SpriteSM sprite = base.GetComponent<SpriteSM>();
		for (;;)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.2f));
			sprite.SetLowerLeftPixel_X(256f);
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.05f));
			sprite.SetLowerLeftPixel_X(0f);
		}
		yield break;
	}

	private void Update()
	{
	}
}
