// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class MineFieldSign : MonoBehaviour
{
	private void Awake()
	{
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
		this.seed = Networking.RandomSeed + (int)base.transform.position.sqrMagnitude;
		this.random = new Randomf(this.seed);
	}

	private void Start()
	{
		this.SetupMines();
	}

	private void SetupMines()
	{
		List<Vector3> list = new List<Vector3>();
		RaycastHit raycastHit = default(RaycastHit);
		LayerMask mask = 1 << LayerMask.NameToLayer("Ground");
		list.Add(new Vector3(this.x, this.y + 5f, 0f));
		if (Physics.Raycast(new Vector3(this.x - 16f, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask) && !Physics.Raycast(new Vector3(this.x - 16f, this.y + 21f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			list.Add(new Vector3(this.x - 16f, this.y + 5f, 0f));
			if (Physics.Raycast(new Vector3(this.x - 32f, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask) && !Physics.Raycast(new Vector3(this.x - 32f, this.y + 21f, 0f), Vector3.down, out raycastHit, 16f, mask))
			{
				list.Add(new Vector3(this.x - 32f, this.y + 5f, 0f));
			}
		}
		if (Physics.Raycast(new Vector3(this.x + 16f, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask) && !Physics.Raycast(new Vector3(this.x + 16f, this.y + 21f, 0f), Vector3.down, out raycastHit, 16f, mask))
		{
			list.Add(new Vector3(this.x + 16f, this.y + 5f, 0f));
			if (Physics.Raycast(new Vector3(this.x + 32f, this.y + 5f, 0f), Vector3.down, out raycastHit, 16f, mask) && !Physics.Raycast(new Vector3(this.x + 32f, this.y + 21f, 0f), Vector3.down, out raycastHit, 16f, mask))
			{
				list.Add(new Vector3(this.x + 32f, this.y + 5f, 0f));
			}
		}
		int num = 0;
		while (list.Count > 0)
		{
			int index = this.random.Range(0, list.Count);
			Vector3 origin = list[index];
			list.RemoveAt(index);
			if (Physics.Raycast(origin, Vector3.down, out raycastHit, 16f, mask))
			{
				if (this.TryPlaceMine(raycastHit.collider.GetComponent<Block>()))
				{
					num++;
				}
				if (num == this.numberOfMines)
				{
					break;
				}
			}
		}
	}

	private bool TryPlaceMine(Block block)
	{
		if (block == null || block.mine != null)
		{
			return false;
		}
		if (!this.CanPlaceInBlock(block.groundType))
		{
			return false;
		}
		if (block.replaceOnCollapse && !this.CanPlaceInBlock(block.replacementBlockType))
		{
			return false;
		}
		if (block.replaceOnCollapse && !this.CanPlaceInBlock(block.replacementBlockType))
		{
			return false;
		}
		Mine mine = UnityEngine.Object.Instantiate(this.mine) as Mine;
		mine.transform.parent = block.transform;
		mine.transform.localPosition = Vector3.zero;
		mine.randomSeed = this.seed;
		block.mine = mine;
		this.plantedMines.Add(block.mine);
		return true;
	}

	private bool CanPlaceInBlock(GroundType blockType)
	{
		return blockType != GroundType.Barrel && blockType != GroundType.PropaneBarrel && blockType != GroundType.Steel && blockType != GroundType.BuriedRocket && blockType != GroundType.Bridge && blockType != GroundType.Bridge2 && blockType != GroundType.FallingBlock;
	}

	private void OnDestroy()
	{
		foreach (Mine mine in this.plantedMines)
		{
			if (mine != null)
			{
				UnityEngine.Object.Destroy(mine.gameObject);
			}
		}
	}

	[HideInInspector]
	public float x;

	[HideInInspector]
	public float y;

	private List<Mine> plantedMines = new List<Mine>();

	private int numberOfMines = 1;

	private int seed;

	private Randomf random;

	public Mine mine;
}
