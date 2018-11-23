// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class TerrainHelper : MonoBehaviour
{
	private void Awake()
	{
		float x = base.transform.position.x;
		float y = base.transform.position.y;
		Map.GetRowCollumn(x, y, ref this.row, ref this.collumn);
		if ((this.rotateLeft || this.rotateRight) && (!this.rotateLeft || !this.rotateRight))
		{
			Map.RotateBlock(this.collumn, this.row, (!this.rotateLeft) ? ((!this.rotateRight) ? 0 : -1) : 1);
		}
	}

	public bool rotateLeft;

	public bool rotateRight;

	public int row;

	public int collumn;
}
