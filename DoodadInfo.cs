// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public class DoodadInfo
{
	public DoodadInfo()
	{
		this.variation = -1;
	}

	public DoodadInfo(GridPos pos, DoodadType type)
	{
		this.type = type;
		this.position = pos;
		this.entity = null;
		this.variation = -1;
	}

	public DoodadInfo(GridPos pos, DoodadType type, int variation)
	{
		this.type = type;
		this.position = pos;
		this.entity = null;
		this.variation = variation;
	}

	public GridPoint TagAsGridPoint
	{
		get
		{
			return GridPoint.FromString(this.tag);
		}
		set
		{
			this.tag = value.ToString();
		}
	}

	public GridPos position;

	public DoodadType type;

	[XmlIgnore]
	public GameObject entity;

	public int variation;

	public TriggerInfo triggerInfo;

	public string tag;
}
