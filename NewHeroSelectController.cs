// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class NewHeroSelectController : SingletonMono<NewHeroSelectController>
{
	private void Start()
	{
		List<HeroType> list = new List<HeroType>();
		list.Add(HeroType.Brochete);
		list.Add(HeroType.Brommando);
		list.Add(HeroType.BaBroracus);
		list.Add(HeroType.Rambro);
		list.Add(HeroType.BronanTheBrobarian);
		for (int i = 0; i < 10; i++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			HeroType item = list[index];
			list.RemoveAt(index);
			list.Add(item);
		}
		for (int j = 0; j < 5; j++)
		{
			this.characterHolders[j].SetHero(list[j]);
		}
		for (int k = 0; k < 4; k++)
		{
			this.playerInfos[k].SetCharacter(this.characterHolders[k]);
			this.characterHolders[k].SetPlayerInfo(this.playerInfos[k]);
		}
	}

	private void Update()
	{
		if (this.CurrentlyActivePlayer == null)
		{
			this.CurrentlyActivePlayer = this.GetNextPlayerThanNeedsToSelect();
			if (this.CurrentlyActivePlayer != null)
			{
				this.CurrentlyActivePlayer.isChoosingHero = true;
			}
		}
		else
		{
			CharacterHolder characterHolder = null;
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.CurrentlyActivePlayer.SetSelection();
				this.CurrentlyActivePlayer = null;
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				characterHolder = this.FindAvailableHeroForPlayer(this.CurrentlyActivePlayer, 1);
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				characterHolder = this.FindAvailableHeroForPlayer(this.CurrentlyActivePlayer, -1);
			}
			if (characterHolder != null)
			{
				HeroSelectPlayerInfo playerInfo = characterHolder.playerInfo;
				CharacterHolder currentCharacter = this.CurrentlyActivePlayer.currentCharacter;
				this.CurrentlyActivePlayer.SetCharacter(characterHolder);
				characterHolder.SetPlayerInfo(this.CurrentlyActivePlayer);
				currentCharacter.SetPlayerInfo(playerInfo);
				if (playerInfo != null)
				{
					playerInfo.SetCharacter(currentCharacter);
				}
			}
		}
	}

	private HeroSelectPlayerInfo GetNextPlayerThanNeedsToSelect()
	{
		if (this.SelectOrder.Count > 0)
		{
			int num = this.SelectOrder[0];
			this.SelectOrder.RemoveAt(0);
			MonoBehaviour.print(string.Concat(new object[]
			{
				"num ",
				num,
				" ",
				this.SelectOrder.Count
			}));
			return this.playerInfos[num];
		}
		return null;
	}

	private CharacterHolder FindAvailableHeroForPlayer(HeroSelectPlayerInfo player, int searchDirection)
	{
		searchDirection = (int)Mathf.Sign((float)searchDirection);
		for (int i = 0; i < 5; i++)
		{
			int num = (i + 1) * searchDirection;
			if (player.currentCharacter != null)
			{
				num += player.currentCharacter.index;
			}
			num += 5;
			num %= 5;
			if (!this.characterHolders[num].hasBeenSelected)
			{
				return this.characterHolders[num];
			}
		}
		return null;
	}

	public static Material GetAvatar(HeroType hero)
	{
		switch (hero)
		{
		case HeroType.Rambro:
			return SingletonMono<NewHeroSelectController>.Instance.RambroAvatar;
		case HeroType.Brommando:
			return SingletonMono<NewHeroSelectController>.Instance.BrommandoAvatar;
		case HeroType.BaBroracus:
			return SingletonMono<NewHeroSelectController>.Instance.BaBroracusAvatar;
		default:
			if (hero == HeroType.Brochete)
			{
				return SingletonMono<NewHeroSelectController>.Instance.BrocheteAvatar;
			}
			if (hero != HeroType.BronanTheBrobarian)
			{
				UnityEngine.Debug.LogError("Hero out of range");
				return SingletonMono<NewHeroSelectController>.Instance.RambroAvatar;
			}
			return SingletonMono<NewHeroSelectController>.Instance.BronanAvatar;
		}
	}

	public Material BaBroracusAvatar;

	public Material RambroAvatar;

	public Material BrocheteAvatar;

	public Material BrommandoAvatar;

	public Material BronanAvatar;

	public CharacterHolder[] characterHolders;

	public HeroSelectPlayerInfo[] playerInfos;

	public Color[] playerColours;

	public Color unselectedColor;

	private HeroSelectPlayerInfo CurrentlyActivePlayer;

	private List<int> SelectOrder = new List<int>
	{
		0,
		1,
		2,
		3
	};
}
