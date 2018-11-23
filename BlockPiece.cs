// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BlockPiece : MonoBehaviour
{
	protected virtual void Awake()
	{
		this.bloodDrips = base.GetComponentsInChildren<ParticleSpawner>();
	}

	public virtual void Bloody(DirectionEnum directionFacing, BloodColor color)
	{
		if (!this.bloodied && this.bloodyDirection != DirectionEnum.None && (directionFacing == this.bloodyDirection || directionFacing == DirectionEnum.Any || this.bloodyDirection == DirectionEnum.Any))
		{
			this.Bloody(color);
		}
	}

	public virtual void Bloody(BloodColor color)
	{
		foreach (ParticleSpawner particleSpawner in this.bloodDrips)
		{
			particleSpawner.color = color;
		}
		this.bloodied = true;
		if (base.GetComponent<Renderer>() != null)
		{
			Color bloodColor = EffectsController.GetBloodColor(color);
			base.GetComponent<Renderer>().material.SetColor("_BloodColor", bloodColor);
			base.GetComponent<Renderer>().material.SetFloat("_SliceAmount", 0.1f);
		}
	}

	public Material bloodyMaterial;

	public DirectionEnum bloodyDirection;

	protected bool bloodied;

	protected ParticleSpawner[] bloodDrips;
}
