// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class CampaignHeader
{
	public void PrintEverything()
	{
		UnityEngine.Debug.Log(string.Concat(new object[]
		{
			this.name,
			" ",
			this.author,
			" ",
			this.description,
			" ",
			this.length
		}));
	}

	public string name;

	public string author;

	public string description;

	public int length;

	public string md5;

	public bool hasBrotalityScoreboard;

	public bool hasTimeScoreBoard = true;

	[XmlIgnore]
	public bool isPublished;

	public GameMode gameMode;
}
