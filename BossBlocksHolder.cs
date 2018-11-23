// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BossBlocksHolder : MonoBehaviour
{
	private void Start()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			BossBlockPiece component = child.GetComponent<BossBlockPiece>();
			if (component != null)
			{
				component.boss = this;
			}
		}
	}

	private void Update()
	{
	}

	public BossBlockOrgan[] organs;
}
