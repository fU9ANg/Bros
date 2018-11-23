// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FaderSprite : MonoBehaviour
{
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteSM>();
		this.life = this.maxLife;
		if (this.mustSetColor)
		{
			this.sprite.SetColor(this.setColorTo);
		}
		if (this.mustSetMaterial)
		{
			base.GetComponent<Renderer>().material = this.setMaterialTo;
			this.sprite.RecalcTexture();
			this.sprite.SetLowerLeftPixel(this.setLowerLeftTo);
			this.sprite.SetPixelDimensions(this.setPixelDimensionsTo);
			this.sprite.SetSize(this.setPixelDimensionsTo.x, this.setPixelDimensionsTo.y);
			this.sprite.SetOffset(this.setOffsetTo + Vector3.forward);
		}
	}

	public void SetColor(Color color)
	{
		if (this.sprite != null)
		{
			this.sprite.SetColor(color);
		}
		else
		{
			this.mustSetColor = true;
			this.setColorTo = color;
		}
	}

	public void SetMaterial(Material material, Vector2 lowerLeft, Vector2 pixelDimensions, Vector3 offset)
	{
		this.mustSetMaterial = true;
		this.setMaterialTo = material;
		this.setPixelDimensionsTo = pixelDimensions;
		this.setLowerLeftTo = lowerLeft;
		this.setOffsetTo = offset;
	}

	private void Update()
	{
		this.life -= Time.deltaTime;
		this.sprite.SetColor(new Color(this.sprite.color.r, this.sprite.color.g, this.sprite.color.b, this.life / this.maxLife * this.fadeM));
		if (this.life <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (this.moveForwards)
		{
			base.transform.Translate(new Vector3(0f, 0f, 1f * Time.deltaTime));
		}
	}

	protected SpriteSM sprite;

	protected float life = 1f;

	public float maxLife = 1f;

	public float fadeM = 0.7f;

	protected bool mustSetColor;

	protected Color setColorTo = Color.black;

	public bool moveForwards;

	protected bool mustSetMaterial;

	protected Material setMaterialTo;

	protected Vector2 setLowerLeftTo;

	protected Vector2 setPixelDimensionsTo;

	protected Vector3 setOffsetTo;
}
