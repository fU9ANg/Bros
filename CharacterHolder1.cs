// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CharacterHolder1 : MonoBehaviour
{
	private void Awake()
	{
		this.SetColor(Color.gray);
	}

	private void Update()
	{
		Color color = Color.gray;
		bool enabled = true;
		if (this.playerInfo != null)
		{
			if (this.hasBeenSelected)
			{
				enabled = false;
				color = this.playerInfo.color;
			}
			else if (this.playerInfo.isChoosingHero)
			{
				enabled = false;
				color = this.playerInfo.color;
				color -= Color.white * Useful.Sin01(Time.time * 10f) * 0.15f;
			}
		}
		this.SetColor(color);
		this.renderCam.GetComponent<GrayscaleEffect>().enabled = enabled;
	}

	public void SetHero(HeroType Hero)
	{
		this.sprite.GetComponent<Renderer>().sharedMaterial = NewHeroSelectController1.GetAvatar(Hero);
		this.heroType = Hero;
	}

	public void SetPlayerInfo(HeroSelectPlayerInfo1 info)
	{
		this.playerInfo = info;
		if (info != null)
		{
			this.SetColor(info.color);
		}
		else
		{
			this.SetColor(Color.gray);
		}
	}

	private void SetColor(Color color)
	{
		this.renderCam.backgroundColor = color;
		this.heroName.color = color;
	}

	public Camera renderCam;

	public SpriteSM sprite;

	public TextMesh heroName;

	public HeroType heroType;

	public bool hasBeenSelected;

	public HeroSelectPlayerInfo1 playerInfo;

	public int index;
}
