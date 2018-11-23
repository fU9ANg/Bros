// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class VictoryCave : MonoBehaviour
{
	private void Start()
	{
		UnityEngine.Debug.Log("Place Things");
	}

	private void Update()
	{
		if (this.firstFrame)
		{
			this.firstFrame = false;
			Map.MakeAllBlocksUltraTough();
		}
	}

	public GameObject wallBlockFloor;

	public GameObject wallBlockCeiling;

	public GameObject backgroundBlock;

	protected bool firstFrame = true;
}
