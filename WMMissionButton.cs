// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WMMissionButton : WMSimpleButton
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		this.buttonNormalMaterial = base.GetComponent<Renderer>().sharedMaterial;
	}

	public void AnimateIn()
	{
		base.gameObject.SetActive(true);
		this.animate = true;
		this.scaleI = 1f;
		this.scale = 0.1f;
		base.transform.localScale = Vector3.one * this.scale;
	}

	protected override void SetOut()
	{
		WorldMapController.ClearMissionDetailsText();
		base.SetOut();
	}

	protected override void SetOver()
	{
		this.worldLocation.SetMissionText();
		base.SetOver();
	}

	protected override void PressButton()
	{
		base.PressButton();
		if (!this.buttonAlreadyDone)
		{
			WorldMapController.TransportGoTo(this.worldLocation);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!Application.isEditor || Input.GetKeyDown(KeyCode.F12))
		{
		}
		if (this.animate)
		{
			this.scale += this.scaleI * Time.deltaTime;
			this.scaleI += (1f - this.scale) * Time.deltaTime * 200f;
			this.scaleI *= 1f - Time.deltaTime * 12f;
			base.transform.localScale = Vector3.one * this.scale;
		}
	}

	public string campaignName;

	public int minPrestigeLevel;

	protected bool animate;

	protected float scale = 1f;

	protected float scaleI = 1f;

	public SpriteSM radiusSprite;

	public WorldLocation worldLocation;
}
