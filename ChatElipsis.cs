// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ChatElipsis : MonoBehaviour
{
	private void Update()
	{
	}

	public static void CreateOnAllLocallyOwnedHeros()
	{
		foreach (Player player in HeroController.players)
		{
			if (player != null && player.character != null && player.character.IsMine && player.IsAlive())
			{
				player.character.CreateEllipsisOnHero();
			}
		}
	}
}
