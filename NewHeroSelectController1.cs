// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewHeroSelectController1 : SingletonMono<NewHeroSelectController1>
{
	private void Start()
	{
		base.StartCoroutine(this.RandomizeOrder());
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
			this.playerInfos[k].SetCharacter(null);
		}
	}

	private IEnumerator RandomizeOrder()
	{
		this.PlayerOrder = new List<int>
		{
			1,
			3,
			0,
			2
		};
		yield return new WaitForSeconds(1f);
		int i = 0;
		int j = 0;
		for (int k = 0; k < 20; k++)
		{
			yield return new WaitForSeconds(0.1f);
			i += UnityEngine.Random.Range(0, 3);
			j = i + UnityEngine.Random.Range(0, 3);
			i %= 4;
			j %= 4;
			int t = this.PlayerOrder[i];
			this.PlayerOrder[i] = this.PlayerOrder[j];
			this.PlayerOrder[j] = t;
		}
		this.SelectOrder = new List<int>(this.PlayerOrder);
		yield break;
	}

	private void Update()
	{
		if (this.CurrentlyActivePlayer == null)
		{
			this.CurrentlyActivePlayer = this.GetNextPlayerThanNeedsToSelect();
			if (this.CurrentlyActivePlayer != null)
			{
				CharacterHolder1 characterHolder = this.FindAvailableHeroForPlayer(this.CurrentlyActivePlayer, 1);
				this.CurrentlyActivePlayer.isChoosingHero = true;
				this.CurrentlyActivePlayer.SetCharacter(characterHolder);
				characterHolder.SetPlayerInfo(this.CurrentlyActivePlayer);
			}
		}
		else
		{
			CharacterHolder1 characterHolder2 = null;
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.CurrentlyActivePlayer.SetSelection();
				this.CurrentlyActivePlayer = null;
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				characterHolder2 = this.FindAvailableHeroForPlayer(this.CurrentlyActivePlayer, 1);
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				characterHolder2 = this.FindAvailableHeroForPlayer(this.CurrentlyActivePlayer, -1);
			}
			if (characterHolder2 != null)
			{
				HeroSelectPlayerInfo1 playerInfo = characterHolder2.playerInfo;
				CharacterHolder1 currentCharacter = this.CurrentlyActivePlayer.currentCharacter;
				this.CurrentlyActivePlayer.SetCharacter(characterHolder2);
				characterHolder2.SetPlayerInfo(this.CurrentlyActivePlayer);
				currentCharacter.SetPlayerInfo(null);
				if (playerInfo != null)
				{
					playerInfo.SetCharacter(currentCharacter);
				}
			}
		}
	}

	private HeroSelectPlayerInfo1 GetNextPlayerThanNeedsToSelect()
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

	private CharacterHolder1 FindAvailableHeroForPlayer(HeroSelectPlayerInfo1 player, int searchDirection)
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
			return SingletonMono<NewHeroSelectController1>.Instance.RambroAvatar;
		case HeroType.Brommando:
			return SingletonMono<NewHeroSelectController1>.Instance.BrommandoAvatar;
		case HeroType.BaBroracus:
			return SingletonMono<NewHeroSelectController1>.Instance.BaBroracusAvatar;
		default:
			if (hero == HeroType.Brochete)
			{
				return SingletonMono<NewHeroSelectController1>.Instance.BrocheteAvatar;
			}
			if (hero != HeroType.BronanTheBrobarian)
			{
				UnityEngine.Debug.LogError("Hero out of range");
				return SingletonMono<NewHeroSelectController1>.Instance.RambroAvatar;
			}
			return SingletonMono<NewHeroSelectController1>.Instance.BronanAvatar;
		}
	}

	public Material BaBroracusAvatar;

	public Material RambroAvatar;

	public Material BrocheteAvatar;

	public Material BrommandoAvatar;

	public Material BronanAvatar;

	public CharacterHolder1[] characterHolders;

	public HeroSelectPlayerInfo1[] playerInfos;

	public Color[] playerColours;

	public Color unselectedColor;

	private HeroSelectPlayerInfo1 CurrentlyActivePlayer;

	public List<int> SelectOrder = new List<int>
	{
		0,
		1,
		2,
		3
	};

	public List<int> PlayerOrder = new List<int>
	{
		0,
		1,
		2,
		3
	};
}
