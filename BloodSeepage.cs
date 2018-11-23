// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BloodSeepage : BlockPiece
{
	protected void Start()
	{
		if (this.bleedMaterial != null)
		{
			this.bleedMaterial.SetFloat("_SliceAmount", 1f);
		}
	}

	public override void Bloody(DirectionEnum directionFacing, BloodColor color)
	{
		base.Bloody(directionFacing, color);
	}

	public override void Bloody(BloodColor color)
	{
		foreach (ParticleSpawner particleSpawner in this.bloodDrips)
		{
			particleSpawner.color = color;
		}
		base.gameObject.SetActive(true);
		base.GetComponent<Renderer>().enabled = true;
		if (this.bloodyMaterial != null)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.bloodyMaterial;
			this.bloodyMaterial = null;
		}
		if (this.useMaterial)
		{
			if (this.bleedMaterial == null)
			{
				this.bleedMaterial = base.GetComponent<Renderer>().material;
				Color bloodColor = EffectsController.GetBloodColor(color);
				this.bleedMaterial.SetColor("_BloodColor", bloodColor);
			}
			this.bloodAmount += 0.01667f * (1f - this.bloodAmount * 0.7f);
			if (this.bloodyDirection == DirectionEnum.Down)
			{
				this.bloodAmount += 0.01667f * (1f - this.bloodAmount * 0.7f);
			}
		}
		if (this.bloodAmount >= 1f)
		{
			this.bloodied = true;
		}
		else
		{
			this.bloodied = false;
		}
	}

	private void Update()
	{
		if (this.bloodCounter < this.bloodAmount)
		{
			this.bloodCounter += Time.deltaTime * 0.2f;
			this.bleedMaterial.SetFloat("_SliceAmount", 0.96f - Mathf.Clamp(this.bloodCounter, 0f, 0.96f));
		}
	}

	public bool useMaterial = true;

	protected Material bleedMaterial;

	protected float bloodCounter;

	protected float bloodAmount;
}
