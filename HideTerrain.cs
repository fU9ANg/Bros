// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class HideTerrain : MonoBehaviour
{
	private void Start()
	{
		this.Hide();
	}

	private void Hide()
	{
		this.Show();
		int num = 0;
		int num2 = 0;
		Map.GetRowCollumn(base.transform.position.x, base.transform.position.y, ref num, ref num2);
		for (int i = 0; i < this.collumns; i++)
		{
			for (int j = 0; j < this.rows; j++)
			{
				Block backgroundBlock = Map.GetBackgroundBlock(num2 + i, num + j);
				if (backgroundBlock != null)
				{
					this.hiddenBlocks.Add(backgroundBlock);
				}
			}
		}
		foreach (Block block in this.hiddenBlocks)
		{
			block.gameObject.SetActive(false);
		}
	}

	private void Show()
	{
		foreach (Block block in this.hiddenBlocks)
		{
			if (block != null)
			{
				block.gameObject.SetActive(true);
			}
		}
		this.hiddenBlocks.Clear();
	}

	private void Update()
	{
		if (Map.isEditing)
		{
			this.Hide();
		}
	}

	private void OnDestroy()
	{
		this.Show();
	}

	public int rows = 1;

	public int collumns = 1;

	private List<Block> hiddenBlocks = new List<Block>();
}
