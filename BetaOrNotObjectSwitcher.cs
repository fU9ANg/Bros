// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class BetaOrNotObjectSwitcher : MonoBehaviour
{
	private void Awake()
	{
		if (this.exhibitionObject != null && PlaytomicController.isExhibitionBuild)
		{
			this.exhibitionObject.SetActive(true);
			this.betaObject.SetActive(false);
			this.brototypeObject.SetActive(false);
		}
		else
		{
			this.betaObject.SetActive(true);
			this.brototypeObject.SetActive(false);
			if (this.exhibitionObject != null)
			{
				this.exhibitionObject.SetActive(false);
			}
		}
	}

	public GameObject betaObject;

	public GameObject brototypeObject;

	public GameObject exhibitionObject;
}
