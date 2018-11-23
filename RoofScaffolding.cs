// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class RoofScaffolding : MonoBehaviour
{
	private void Start()
	{
		this.block = base.transform.parent.GetComponent<Block>();
		int num = 0;
		int num2 = 0;
		if (!(this.block.LeftBlock == null))
		{
			if (this.block.RightBlock == null)
			{
				num2 = 5;
			}
			else
			{
				num2 = 2 + this.block.collumn % 2;
			}
		}
		if (!(this.block.AboveBlock == null))
		{
			if (this.block.AboveBlock.AboveBlock == null)
			{
				num = 2;
			}
			else if (this.block.BelowBlock == null)
			{
				num = 6;
			}
			else
			{
				num = 4;
			}
		}
		SpriteSM component = base.transform.GetComponent<SpriteSM>();
		component.OffsetLowerLeftPixel((float)(num2 * 16), (float)(num * 16));
		base.transform.parent = null;
	}

	private void Update()
	{
		if (Map.isEditing && (this.block == null || this.block.destroyed))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private Block block;
}
