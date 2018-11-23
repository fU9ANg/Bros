// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapSeaShoreAnim : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		this.counter += Time.deltaTime;
		if (this.counter > this.frameRate)
		{
			this.counter -= this.frameRate;
			this.frame = (this.frame + 1) % this.textures.Length;
			this.animatedMaterial.mainTexture = this.textures[this.frame];
		}
	}

	protected float counter;

	protected int frame;

	public float frameRate = 0.05f;

	public Texture[] textures;

	public Material animatedMaterial;
}
