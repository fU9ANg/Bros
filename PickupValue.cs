// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class PickupValue
{
	public PickupType type;

	[HideInInspector]
	public int amount = 1;

	public int minAmount = 2;

	public int maxAmount = 4;

	public Color color = Color.white;
}
