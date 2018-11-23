// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DoodadFloorGrating : DoodadDestroyable
{
	public void StepOn(TestVanDammeAnim character)
	{
		if (!Map.isEditing && character.yI < -200f)
		{
			this.Damage(new DamageObject(5, DamageType.Crush, 0f, -100f, null));
		}
		if (!Map.isEditing && character.yI > 10f)
		{
			this.Damage(new DamageObject(5, DamageType.Crush, 0f, 120f, null));
		}
	}

	public override void Death()
	{
		base.Death();
		Vector3 origin = Map.GetBlockCenter(this.collumn, this.row);
		foreach (RaycastHit raycastHit in Physics.RaycastAll(origin, Vector3.up, 16f, Map.groundLayer))
		{
			FallingBlock component = raycastHit.collider.GetComponent<FallingBlock>();
			MonoBehaviour.print(raycastHit.collider.gameObject);
			if (component != null)
			{
				component.DisturbNetworked();
			}
		}
	}
}
