// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SometimesSpawn : MonoBehaviour
{
	private void Start()
	{
		if (SometimesSpawn.sometimesIndex < 0)
		{
			SometimesSpawn.sometimesIndex = UnityEngine.Random.Range(0, 12);
		}
		if (SometimesSpawn.sometimesIndex % this.oneInHowMany != 0 || (GameModeController.GameMode == GameMode.BroDown && !this.appearInBrodown))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			SometimesSpawn.sometimesIndex += UnityEngine.Random.Range(0, 2);
		}
		SometimesSpawn.sometimesIndex++;
	}

	protected static int sometimesIndex = -1;

	public int oneInHowMany = 4;

	public bool appearInBrodown = true;
}
