// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RandomLowerLeftPixel : MonoBehaviour
{
	private void Start()
	{
		SpriteSM component = base.GetComponent<SpriteSM>();
		component.SetLowerLeftPixel((float)((int)((float)UnityEngine.Random.Range(0, this.uVCollumns) * component.pixelDimensions.x)), (float)((int)component.lowerLeftPixel.y));
	}

	public int uVCollumns = 8;
}
