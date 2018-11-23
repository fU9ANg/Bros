// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using UnityEngine;

public class WaitingForOthersHUD : MonoBehaviour
{
	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		MonoBehaviour.print("Start");
		for (;;)
		{
			if (!Connect.IsOffline)
			{
				if (HeroController.AllPlayersHaveJoined)
				{
					break;
				}
				this.SetActiveOnChildren(true);
			}
			yield return new WaitForEndOfFrame();
		}
		this.SetActiveOnChildren(false);
		yield break;
		yield break;
	}

	private void SetActiveOnChildren(bool active)
	{
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			transform.gameObject.SetActive(active);
		}
	}
}
