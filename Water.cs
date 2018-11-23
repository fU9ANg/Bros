// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class Water : MonoBehaviour
{
	private void Awake()
	{
		this.fallingWater.GetComponent<Renderer>().enabled = false;
		this.stillWater.GetComponent<Renderer>().enabled = false;
		this.waterSurface.GetComponent<Renderer>().enabled = false;
		this.normalMap.GetComponent<Renderer>().enabled = false;
	}

	public void Draw(bool still, bool surface, bool falling, bool offsetFalling, float stillHeight, float fallingHeight)
	{
		this.HideAll();
		if (still)
		{
			this.stillWater.GetComponent<Renderer>().enabled = true;
			this.SetSpriteHeight(this.stillWater, stillHeight);
		}
		else
		{
			this.stillWater.GetComponent<Renderer>().enabled = false;
		}
		if (falling)
		{
			if (offsetFalling)
			{
				this.SetSpriteFalling(this.fallingWater, fallingHeight);
			}
			else if (still)
			{
				fallingHeight = Mathf.Min(fallingHeight, 1f - stillHeight);
				this.SetSpriteFalling(this.fallingWater, fallingHeight);
			}
			else
			{
				this.SetSpriteHeight(this.fallingWater, fallingHeight);
			}
			this.fallingWater.GetComponent<Renderer>().enabled = true;
		}
		else
		{
			this.fallingWater.GetComponent<Renderer>().enabled = false;
		}
		if (still)
		{
			this.normalMap.GetComponent<Renderer>().enabled = true;
			this.SetSpriteHeight(this.normalMap, stillHeight);
		}
		else if (falling)
		{
			this.normalMap.GetComponent<Renderer>().enabled = true;
			this.SetSpriteHeight(this.normalMap, fallingHeight);
		}
		else
		{
			this.normalMap.GetComponent<Renderer>().enabled = false;
		}
		if (surface)
		{
			this.waterSurface.GetComponent<Renderer>().enabled = true;
			this.waterSurface.transform.localPosition = Vector3.up * (stillHeight * 16f - 8f);
		}
	}

	public void Draw(bool still, bool falling, bool surface, float stillHeight, float fallingHeight)
	{
		if (still)
		{
			this.stillWater.GetComponent<Renderer>().enabled = true;
			this.SetSpriteHeight(this.stillWater, stillHeight);
		}
		if (falling)
		{
			if (still)
			{
				this.SetSpriteBottom(this.fallingWater, stillHeight);
				this.fallingWater.GetComponent<Renderer>().enabled = true;
			}
			else
			{
				this.SetSpriteHeight(this.fallingWater, fallingHeight);
				this.fallingWater.GetComponent<Renderer>().enabled = true;
			}
		}
		if (still || falling)
		{
			this.normalMap.GetComponent<Renderer>().enabled = true;
			this.SetSpriteHeight(this.normalMap, fallingHeight);
		}
		if (surface)
		{
			this.waterSurface.GetComponent<Renderer>().enabled = true;
			this.waterSurface.transform.localPosition = Vector3.up * (stillHeight * 16f - 8f);
		}
	}

	public void HideAll()
	{
		this.waterSurface.GetComponent<Renderer>().enabled = false;
		this.stillWater.GetComponent<Renderer>().enabled = false;
		this.fallingWater.GetComponent<Renderer>().enabled = false;
		this.normalMap.GetComponent<Renderer>().enabled = false;
	}

	private void SetSpriteFalling(SpriteSM sprite, float h)
	{
		float num = h * 16f;
		float num2 = 16f - num;
		float y = num2 / 2f;
		float y2 = num;
		sprite.SetSize(sprite.width, num);
		sprite.SetPixelDimensions(new Vector2(sprite.pixelDimensions.x, num));
		sprite.SetLowerLeftPixel(new Vector2(sprite.lowerLeftPixel.x, y2));
		sprite.SetOffset(new Vector3(sprite.offset.x, y, sprite.offset.z));
		sprite.UpdateUVs();
	}

	private void SetSpriteHeight(SpriteSM sprite, float h)
	{
		float num = h * 16f;
		float num2 = 16f - num;
		float y = -num2 / 2f;
		float y2 = 16f;
		sprite.SetSize(sprite.width, num);
		sprite.SetPixelDimensions(new Vector2(sprite.pixelDimensions.x, num));
		sprite.SetLowerLeftPixel(new Vector2(sprite.lowerLeftPixel.x, y2));
		sprite.SetOffset(new Vector3(sprite.offset.x, y, sprite.offset.z));
		sprite.UpdateUVs();
	}

	private void SetSpriteBottom(SpriteSM sprite, float height)
	{
		float num = (1f - height) * 16f;
		float num2 = 16f - num;
		float y = num2 / 2f;
		float y2 = num;
		sprite.SetSize(sprite.width, num);
		sprite.SetPixelDimensions(new Vector2(sprite.pixelDimensions.x, num));
		sprite.SetLowerLeftPixel(new Vector2(sprite.lowerLeftPixel.x, y2));
		sprite.SetOffset(new Vector3(sprite.offset.x, y, sprite.offset.z));
		sprite.UpdateUVs();
	}

	public SpriteSM fallingWater;

	public SpriteSM stillWater;

	public SpriteSM waterSurface;

	public SpriteSM normalMap;
}
