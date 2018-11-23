// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
	private void Start()
	{
	}

	public void SetPlayerInfo(string name, Material avatarMaterial, string info1, float info1Value, string info2, float info2Value, string specialDescription)
	{
		base.gameObject.SetActive(true);
		this.avatar.GetComponent<Renderer>().material = avatarMaterial;
		if (info1 != this.bulletPoints1NameString)
		{
			this.bulletPoints1NameString = info1;
			this.bulletPoints1Name.Write(this.bulletPoints1NameString);
		}
		if (info2 != this.bulletPoints2NameString)
		{
			this.bulletPoints2NameString = info2;
			this.bulletPoints2Name.Write(this.bulletPoints2NameString);
		}
		this.bulletPoints1Name.gameObject.SetActive(true);
		this.bulletPoints2Name.gameObject.SetActive(true);
		this.bulletPoints1.SetPoints(info1Value);
		this.bulletPoints2.SetPoints(info2Value);
		this.specialDescription.Write(specialDescription);
		this.broName.Write(name);
		this.bulletPoints1.gameObject.SetActive(true);
		this.bulletPoints2.gameObject.SetActive(true);
		this.lockedDescription.gameObject.SetActive(false);
	}

	public void SetPlayerLockedInfo(string lockedDescription)
	{
		this.avatar.GetComponent<Renderer>().material = this.lockedMaterial;
		this.broName.Write("locked bro");
		this.lockedDescription.Write(lockedDescription);
		this.bulletPoints1.ClearPoints();
		this.bulletPoints2.ClearPoints();
		this.bulletPoints1.gameObject.SetActive(false);
		this.bulletPoints2.gameObject.SetActive(false);
		this.specialDescription.gameObject.SetActive(false);
	}

	public void SetPlayerRandomInfo()
	{
		this.avatar.GetComponent<Renderer>().material = this.randomMaterial;
		this.broName.Write("random bro");
		this.lockedDescription.Write("A RANDOM BRO\nWILL BE CHOSEN\nFOR YOU!");
		this.bulletPoints1.ClearPoints();
		this.bulletPoints2.ClearPoints();
		this.bulletPoints1.gameObject.SetActive(false);
		this.bulletPoints2.gameObject.SetActive(false);
		this.specialDescription.gameObject.SetActive(false);
	}

	private void Update()
	{
	}

	public BulletPointsBar bulletPoints1;

	public BulletPointsBar bulletPoints2;

	public TextWritesSelf bulletPoints1Name;

	public TextWritesSelf bulletPoints2Name;

	public TextWritesSelf lockedDescription;

	public Material lockedMaterial;

	public Material randomMaterial;

	protected string bulletPoints1NameString;

	protected string bulletPoints2NameString;

	public TextWritesSelf specialDescription;

	public TextWritesSelf broName;

	public SpriteSM avatar;
}
