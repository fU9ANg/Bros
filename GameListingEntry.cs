// dnSpy decompiler from Assembly-CSharp.dll
using System;
using Badumna.Match;
using UnityEngine;

public class GameListingEntry : MonoBehaviour
{
	public void SetupRandom(int index)
	{
		this.Number.text = string.Empty + index;
		this.Host.text = "Host name " + UnityEngine.Random.Range(1000, 10000);
		this.GameName.text = "Game name " + UnityEngine.Random.Range(1000, 10000);
		this.Country.text = "South Africa";
		this.Version.text = "4";
		this.Space.text = 2 + "/4";
	}

	public void SetColor(Color color)
	{
		this.Number.color = color;
		this.GameName.color = color;
		this.Host.color = color;
		this.Country.color = color;
		this.Version.color = color;
		this.Space.color = color;
	}

	public TextMesh Number;

	public TextMesh Host;

	public TextMesh GameName;

	public TextMesh Country;

	public TextMesh Version;

	public TextMesh Space;

	public MatchmakingResult match;
}
