// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class StupidArial : MonoBehaviour
{
	private void Start()
	{
		this.block = base.transform.parent.GetComponent<Block>();
		if (!(this.block.AboveBlock == null) || !(this.block.LeftBlock == null))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (this.block.destroyed)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private Block block;
}
