// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class LevelSelectObject : Button
{
	public virtual void Complete()
	{
		base.GetComponent<Renderer>().sharedMaterial = this.completeMaterial;
		this.isCompleted = true;
	}

	public virtual void IsInitial()
	{
		base.GetComponent<Renderer>().sharedMaterial = this.nextToCompleteMaterial;
		this.isInitial = true;
	}

	public virtual void Incomplete()
	{
		if (!this.isInitial)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.incompleteMaterial;
		}
	}

	public string levelDescription = "UNNAMEDLEVEL1";

	public string tilesetName;

	[HideInInspector]
	public bool isCompleted;

	[HideInInspector]
	public bool isInitial;

	public Material completeMaterial;

	public Material incompleteMaterial;

	public Material nextToCompleteMaterial;

	public Texture2D foregroundTexture;

	public Texture2D backgroundTexture;
}
