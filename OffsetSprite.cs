// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class OffsetSprite : MonoBehaviour
{
	private void Update()
	{
		if (this.onFirstFrame)
		{
			SpriteSM component = base.GetComponent<SpriteSM>();
			if (this.RandomRowOffset > 0)
			{
				Block component2 = base.transform.parent.GetComponent<Block>();
				float num;
				if (this.modulateColumns)
				{
					num = (float)(component2.collumn % this.modulationFrequency);
				}
				else
				{
					num = (float)UnityEngine.Random.Range(0, this.RandomRowOffset);
				}
				num *= component.width;
				component.OffsetLowerLeftPixel(num, 0f);
			}
			this.onFirstFrame = false;
		}
	}

	public int RandomRowOffset;

	public bool modulateColumns;

	public int modulationFrequency = 2;

	private bool onFirstFrame = true;
}
