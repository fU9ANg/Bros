// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class AssMouthOrifice : Doodad
{
	protected override void Start()
	{
		base.Start();
		this.connectedOrifices = this.FindConnectedOrifices();
	}

	private Queue<AssMouthOrifice> FindConnectedOrifices()
	{
		List<AssMouthBlock> list = new List<AssMouthBlock>();
		Stack<AssMouthBlock> stack = new Stack<AssMouthBlock>();
		Queue<AssMouthOrifice> queue = new Queue<AssMouthOrifice>();
		stack.Push(this.rootA);
		stack.Push(this.rootB);
		while (stack.Count > 0)
		{
			AssMouthBlock assMouthBlock = stack.Pop();
			list.Add(assMouthBlock);
			if (assMouthBlock.orificeInstance != null && assMouthBlock.orificeInstance != this && !queue.Contains(assMouthBlock.orificeInstance))
			{
				queue.Enqueue(assMouthBlock.orificeInstance);
			}
			this.TryAdd(assMouthBlock.RightBlock, stack, list);
			this.TryAdd(assMouthBlock.LeftBlock, stack, list);
			this.TryAdd(assMouthBlock.aboveBlock, stack, list);
			this.TryAdd(assMouthBlock.belowBlock, stack, list);
		}
		return queue;
	}

	private void Update()
	{
		foreach (Player player in HeroController.players)
		{
			if (player != null && player.character != null)
			{
				Vector3 position = player.character.transform.position;
				Vector3 point = player.character.transform.position + Vector3.up * 16f;
				Vector3 point2 = player.character.transform.position + Vector3.up * 8f + Vector3.right * 8f;
				Vector3 point3 = player.character.transform.position + Vector3.up - Vector3.right * 8f;
				if (base.GetComponent<Collider>().bounds.Contains(position) || base.GetComponent<Collider>().bounds.Contains(point) || base.GetComponent<Collider>().bounds.Contains(point2) || base.GetComponent<Collider>().bounds.Contains(point3))
				{
					Vector3 lhs = new Vector3(player.character.xI, player.character.yI, 0f);
					AssMouthOrifice assMouthOrifice = this.connectedOrifices.Peek();
					if (Vector3.Dot(lhs, assMouthOrifice.transform.up) > 0f)
					{
						MonoBehaviour.print("whhhooop");
						player.character.x = assMouthOrifice.x;
						player.character.y = assMouthOrifice.y;
						player.character.xI += assMouthOrifice.transform.up.x * 50f;
						player.character.yI += assMouthOrifice.transform.up.y * 50f;
						this.connectedOrifices.Dequeue();
						this.connectedOrifices.Enqueue(assMouthOrifice);
					}
				}
			}
		}
	}

	private void TryAdd(Block block, Stack<AssMouthBlock> searchStack, List<AssMouthBlock> processedOrifices)
	{
		AssMouthBlock assMouthBlock = block as AssMouthBlock;
		if (assMouthBlock != null && !searchStack.Contains(assMouthBlock) && !processedOrifices.Contains(assMouthBlock))
		{
			searchStack.Push(assMouthBlock);
		}
	}

	public AssMouthBlock rootA;

	public AssMouthBlock rootB;

	public Queue<AssMouthOrifice> connectedOrifices = new Queue<AssMouthOrifice>();
}
