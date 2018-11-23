// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class ConnectionInfo : MonoBehaviour
{
	private void Awake()
	{
		this.Hide();
	}

	private void Update()
	{
	}

	private void OnJoinRoom()
	{
		this.Hide();
	}

	public static void Show()
	{
	}

	private void Hide()
	{
		base.gameObject.SetActive(false);
	}
}
