// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class HideInOfflineMode : MonoBehaviour
{
	private void Awake()
	{
		if (Connect.IsOffline)
		{
			base.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
	}
}
