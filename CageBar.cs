// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CageBar : MonoBehaviour
{
	private void Start()
	{
		this.cage = base.transform.parent.GetComponent<Cage>();
	}

	public void Damage(DamageObject damgeObject)
	{
		this.cage.Damage(damgeObject);
	}

	public void Collapse(float xI, float yI, float chance)
	{
		this.cage.Collapse(xI, yI, chance);
	}

	private Cage cage;
}
