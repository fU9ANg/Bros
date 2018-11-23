// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WMSimpleButton : MonoBehaviour
{
	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
	}

	protected virtual void PressButton()
	{
	}

	protected void SetUnavailable()
	{
		this.unavailable = true;
		base.GetComponent<Collider>().enabled = false;
		base.GetComponent<Renderer>().sharedMaterial = this.buttonUnavailableMaterial;
	}

	protected void SetDone()
	{
		this.buttonAlreadyDone = true;
		base.GetComponent<Renderer>().sharedMaterial = this.buttonDoneMaterial;
	}

	protected void SetAvailabled()
	{
		this.buttonAlreadyDone = false;
		base.GetComponent<Renderer>().sharedMaterial = this.buttonNormalMaterial;
	}

	private void OnMouseEnter()
	{
		if (!this.pressed)
		{
			this.SetOver();
		}
	}

	protected virtual void SetOver()
	{
		if (this.buttonAlreadyDone)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.buttonDoneOverMaterial;
		}
		else
		{
			base.GetComponent<Renderer>().sharedMaterial = this.buttonOverMaterial;
		}
	}

	private void OnMouseExit()
	{
		if (!this.pressed)
		{
			this.SetOut();
		}
	}

	protected virtual void SetOut()
	{
		if (this.pressedHighlightDelay <= 0f)
		{
			base.GetComponent<Renderer>().sharedMaterial = this.buttonNormalMaterial;
		}
	}

	protected virtual void OnMouseUpAsButton()
	{
		this.pressedHighlightDelay = 0.1f;
		this.pressed = true;
		base.GetComponent<Renderer>().sharedMaterial = this.buttonPressedMaterial;
		this.PressButton();
	}

	protected virtual void Update()
	{
		if (this.pressed)
		{
			if (this.pressedHighlightDelay > 0f)
			{
				this.pressedHighlightDelay -= Time.deltaTime;
			}
			else
			{
				this.pressedFlickerCounter += Time.deltaTime;
				if (this.pressedFlickerCounter >= 0.07f)
				{
					this.pressedFlickerCounter -= 0.07f;
					this.flickerCount++;
					if (this.flickerCount % 2 == 1)
					{
						if (this.buttonAlreadyDone)
						{
							base.GetComponent<Renderer>().sharedMaterial = this.buttonDoneOverMaterial;
						}
						else
						{
							base.GetComponent<Renderer>().sharedMaterial = this.buttonOverMaterial;
						}
					}
					else
					{
						base.GetComponent<Renderer>().sharedMaterial = this.buttonPressedMaterial;
					}
				}
			}
		}
	}

	public Material buttonOverMaterial;

	public Material buttonPressedMaterial;

	public Material buttonDoneMaterial;

	public Material buttonUnavailableMaterial;

	public Material buttonDoneOverMaterial;

	protected Material buttonNormalMaterial;

	protected bool pressed;

	protected int flickerCount;

	protected float pressedFlickerCounter;

	protected float pressedHighlightDelay;

	protected bool buttonAlreadyDone;

	protected bool unavailable;
}
