// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapDetailsText : MonoBehaviour
{
	private void Awake()
	{
		this.textMesh = base.GetComponent<TextMesh>();
		this.restingPosition = base.transform.localPosition;
	}

	public void SetText(string text)
	{
		this.textMesh.text = text;
	}

	public void SetColor(Color c)
	{
		this.textMesh.color = c;
		this.color = c;
	}

	public void Appear(float delay)
	{
		this.Appear(this.textMesh.text, this.color, delay);
	}

	public void Appear(string text, Color color, float delay)
	{
		this.color = color;
		this.appearingDelay = delay;
		this.appearing = true;
		base.transform.localPosition = this.restingPosition + Vector3.right * this.appearOffsetX;
		this.textMesh.color = new Color(color.r, color.g, color.b, 0f);
		this.textMesh.text = text;
		this.counter = 0f;
		base.gameObject.SetActive(true);
	}

	private void Update()
	{
		if (this.appearing)
		{
			if (this.appearingDelay > 0f)
			{
				this.appearingDelay -= Time.deltaTime;
			}
			else
			{
				this.counter += Time.deltaTime / this.fadeInTime;
				if (base.transform.localPosition.x > this.restingPosition.x)
				{
					base.transform.localPosition -= Vector3.right * Time.deltaTime * this.appearXSpeed;
					if (base.transform.localPosition.x <= this.restingPosition.x)
					{
						base.transform.localPosition = this.restingPosition;
						WorldMapInterfaceTerritoryDetails.Shake();
						Sound.GetInstance().PlaySoundEffectAt(this.audioClips, 0.25f, Sound.GetInstance().transform.position);
					}
				}
				this.textMesh.color = new Color(this.color.r, this.color.g, this.color.b, this.counter);
				if (this.counter > 1f)
				{
					this.appearing = false;
					base.transform.localPosition = this.restingPosition;
				}
			}
		}
	}

	protected TextMesh textMesh;

	protected Color color;

	protected float counter;

	protected Vector3 restingPosition;

	protected float appearingDelay;

	protected bool appearing;

	public float appearOffsetX = 20f;

	public float appearXSpeed = 60f;

	public float fadeInTime = 0.3f;

	public AudioClip[] audioClips;
}
