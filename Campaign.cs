// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Xml.Serialization;

[Serializable]
public class Campaign
{
	public int Length
	{
		get
		{
			return this.levels.Length;
		}
	}

	public string name;

	public MapData[] levels = new MapData[0];

	[XmlIgnore]
	public CampaignHeader header = new CampaignHeader();
}
