// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BridgeUnderPiece : MonoBehaviour
{
	private void Start()
	{
		this.block = base.transform.parent.GetComponent<Block>();
		int num = 1 + this.block.collumn % 2;
		num += 2 * UnityEngine.Random.Range(0, 3);
		num += this.block.row % 2;
		num %= 6;
		this.sprite = base.GetComponent<SpriteSM>();
		if (this.block.LeftBlock == null)
		{
			this.sprite.OffsetLowerLeftPixel(0f, 64f);
		}
		else if (this.block.RightBlock == null)
		{
			this.sprite.OffsetLowerLeftPixel(0f, 64f);
		}
		else
		{
			Vector2 lowerLeftPixel = this.sprite.lowerLeftPixel;
			lowerLeftPixel.x += (float)(num * 16);
			this.sprite.SetLowerLeftPixel(lowerLeftPixel);
			int num2 = UnityEngine.Random.Range(0, this.hangingBits.Length);
			for (int i = 0; i < this.hangingBits.Length; i++)
			{
				if (i != num2 && this.hangingBits[i] != null)
				{
					UnityEngine.Object.Destroy(this.hangingBits[i].gameObject);
				}
			}
		}
	}

	private void Update()
	{
		if (this.block.destroyed)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (!this.damaged)
		{
			if (this.block.LeftIsEmpty)
			{
				this.sprite.OffsetLowerLeftPixel(0f, 32f);
				this.damaged = true;
			}
			else if (this.block.RightIsEmpty)
			{
				this.sprite.OffsetLowerLeftPixel(0f, 64f);
				this.damaged = true;
			}
		}
	}

	private Block block;

	private SpriteSM sprite;

	private bool damaged;

	public SpriteSM[] hangingBits;
}
