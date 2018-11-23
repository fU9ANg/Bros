// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
	private void OnEnable()
	{
		this.row = Mathf.FloorToInt(base.transform.position.y / 16f);
		this.col = Mathf.FloorToInt(base.transform.position.x / 16f);
		FluidController.RegisterWaterSource(this);
	}

	private void OnDisable()
	{
		FluidController.DeregisterWaterSource(this);
	}

	public float rate = 0.5f;

	public int row;

	public int col;
}
