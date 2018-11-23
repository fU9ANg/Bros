// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WMIcon : MonoBehaviour
{
	private void Start()
	{
		this.scale = 0.1f;
		this.scaleI = 1f;
		this.materialCounter = -0.7f;
		base.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
	}

	public void Appear(int number)
	{
		this.text.text = string.Empty + number;
		this.scale = 0.1f;
		this.showing = true;
		base.gameObject.SetActive(true);
		this.Update();
		switch (number)
		{
		case 0:
			base.GetComponent<Renderer>().material = this.materialWhite;
			break;
		case 1:
			base.GetComponent<Renderer>().material = this.materialOrange;
			break;
		case 2:
			base.GetComponent<Renderer>().material = this.materialRed;
			break;
		case 3:
			base.GetComponent<Renderer>().material = this.materialSalmon;
			break;
		case 4:
			base.GetComponent<Renderer>().material = this.materialBlack;
			break;
		default:
			base.GetComponent<Renderer>().material = this.materialBlack;
			break;
		}
	}

	public void Appear(string number)
	{
		this.text.text = number;
		this.scale = 0.1f;
		this.showing = true;
		base.gameObject.SetActive(true);
		this.Update();
	}

	public void Bounce()
	{
		this.scaleI = 5f;
		this.scale *= 0.8f;
		this.showing = true;
		this.Update();
	}

	public void Disappear()
	{
		this.showing = false;
		this.Update();
	}

	private void Update()
	{
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (this.showing)
		{
			if (this.flickerMaterial)
			{
				this.materialCounter += num;
				if (this.materialCounter > 0.1f)
				{
					this.materialCounter -= 0.1f;
					this.materialCount++;
					if (this.materialCount % 2 == 1)
					{
						base.GetComponent<Renderer>().material = this.blank;
					}
					else
					{
						base.GetComponent<Renderer>().material = this.exclaim;
					}
				}
			}
			this.scale += num * this.scaleI;
			this.scaleI += (1f - this.scale) * num * 120f;
			this.scaleI *= 1f - num * 7f;
			base.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
		}
		else
		{
			base.GetComponent<Renderer>().material = this.blank;
			this.scale -= num * 6f;
			base.transform.localScale = new Vector3(this.scale, this.scale, this.scale);
			if (this.scale <= 0f)
			{
				base.gameObject.SetActive(false);
			}
		}
	}

	protected float scaleI = 1f;

	protected float scale = 1f;

	public bool showing = true;

	public Material exclaim;

	public Material blank;

	protected float materialCounter;

	protected int materialCount;

	public bool flickerMaterial;

	public Material materialWhite;

	public Material materialYellow;

	public Material materialOrange;

	public Material materialRed;

	public Material materialPurple;

	public Material materialBlack;

	public Material materialSalmon;

	public Material materialMaroon;

	public Material materialBurgundy;

	public TextMesh text;
}
