// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class WorldMapInterfaceTerritoryDetails : MonoBehaviour
{
	private void Awake()
	{
		WorldMapInterfaceTerritoryDetails.instance = this;
		WorldMapInterfaceTerritoryDetails.activateAdvisor = false;
	}

	private void Start()
	{
		Vector3 position = this.uiCamera.ScreenToWorldPoint(new Vector3(0f, (float)(-(float)Screen.height) * 0.2f, 1f));
		this.screenMinY = this.uiCamera.transform.InverseTransformPoint(position).y;
		if (!this.appearing)
		{
			base.transform.localPosition = new Vector3(0f, this.screenMinY - 3f, this.zOffset);
			base.gameObject.SetActive(false);
		}
	}

	public static void Shake()
	{
		WorldMapInterfaceTerritoryDetails.instance.whiteBarShakeCouner = 0f;
		WorldMapInterfaceTerritoryDetails.instance.whiteBarShakeM = 1f;
	}

	public static void SayGoodbye()
	{
		WorldMapInterfaceTerritoryDetails.instance.nameText.gameObject.SetActive(false);
		WorldMapInterfaceTerritoryDetails.instance.actionText.gameObject.SetActive(false);
		WorldMapInterfaceTerritoryDetails.instance.threatText.gameObject.SetActive(false);
		WorldMapInterfaceTerritoryDetails.instance.goodLuckText.Appear("GOD SPEED BROFORCE!", Color.black, 0.1f);
	}

	public static void Appear(string nameText, string actionText, int threatLevel, string threatName, Color threatColor)
	{
		WorldMapInterfaceTerritoryDetails.instance.appearing = true;
		WorldMapInterfaceTerritoryDetails.instance.gameObject.SetActive(true);
		WorldMapInterfaceTerritoryDetails.instance.nameText.Appear(nameText, Color.black, 0.3f);
		WorldMapInterfaceTerritoryDetails.instance.actionText.Appear(actionText, Color.black, 0.9f);
		WorldMapInterfaceTerritoryDetails.instance.threatText.Appear("THREAT LEVEL: " + threatName, threatColor, 0.6f);
		WorldMapInterfaceTerritoryDetails.instance.goodLuckText.gameObject.SetActive(false);
	}

	public static void Disappear()
	{
		WorldMapInterfaceTerritoryDetails.instance.disappearing = true;
		WorldMapInterfaceTerritoryDetails.instance.appearing = false;
	}

	private void Update()
	{
		if (this.whiteBarShakeM > 0f)
		{
			this.whiteBarShakeCouner -= Time.deltaTime * 40f;
			this.whiteBarShakeM = Mathf.Clamp(this.whiteBarShakeM - Time.deltaTime * 4f, 0f, 100f);
			this.whiteBar.transform.localPosition = new Vector3(Mathf.Sin(this.whiteBarShakeCouner) * this.whiteBarShakeM * this.whiteBarShakeM * this.whiteBarShakeM * 3f, this.whiteBar.transform.localPosition.y, this.whiteBar.transform.localPosition.z);
		}
		if (this.disappearing)
		{
			if (WorldMapInterfaceTerritoryDetails.activateAdvisor)
			{
				WorldMapInterfaceTerritoryDetails.activateAdvisor = false;
			}
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, this.screenMinY - 20f, this.zOffset), Time.deltaTime * 8f);
			if (base.transform.localPosition.y < this.screenMinY)
			{
				if (!this.appearing)
				{
					base.gameObject.SetActive(false);
				}
				this.disappearing = false;
			}
		}
		else if (this.appearing)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, this.restingHeight, this.zOffset), Time.deltaTime * 8f);
			if (!WorldMapInterfaceTerritoryDetails.activateAdvisor)
			{
				WorldMapInterfaceTerritoryDetails.activateAdvisor = true;
				WorldMapAdvisor.TalkAtEase();
			}
		}
	}

	public Camera uiCamera;

	protected float screenMinY;

	public float restingHeight = -30f;

	protected bool appearing;

	protected bool disappearing;

	protected float zOffset = 200f;

	protected float whiteBarShakeM;

	protected float whiteBarShakeCouner;

	public Transform whiteBar;

	public WorldMapDetailsText nameText;

	public WorldMapDetailsText actionText;

	public WorldMapDetailsText threatText;

	public WorldMapDetailsText goodLuckText;

	protected static bool activateAdvisor;

	public WorldMapAdvisor advisor;

	protected static WorldMapInterfaceTerritoryDetails instance;
}
