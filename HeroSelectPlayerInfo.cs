// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HeroSelectPlayerInfo : MonoBehaviour
{
	public Color color
	{
		get
		{
			return HeroController.GetHeroColor(this.PlayerNumber);
		}
	}

	private void Start()
	{
		this.playerName.color = this.color;
	}

	private void Update()
	{
	}

	public void SetCharacter(CharacterHolder holder)
	{
		this.currentCharacter = holder;
	}

	public void SetSelection()
	{
		this.isChoosingHero = false;
		this.currentCharacter.hasBeenSelected = true;
	}

	public SpriteSM[] fists;

	public int PlayerNumber;

	public CharacterHolder currentCharacter;

	public TextMesh playerName;

	public bool isChoosingHero;
}
