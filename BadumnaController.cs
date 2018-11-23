// dnSpy decompiler from Assembly-CSharp.dll
using System;
using Badumna.Security;
using UnityEngine;

public class BadumnaController : SingletonMono<BadumnaController>
{
	public string KeyPairXml
	{
		get
		{
			return this.keyPairXml;
		}
	}

	public void AddressChangedEventHandler()
	{
		UnityEngine.Debug.Log("> Address changed dectected.");
		ChatSystem.AddMessage("IP has Changed", PID.NoID, Connect.Timef);
	}

	private void Awake()
	{
		if (this.keyPairXml == null)
		{
			this.GenerateKeyPair();
		}
	}

	public void GenerateKeyPair()
	{
		this.keyPairXml = UnverifiedIdentityProvider.GenerateKeyPair();
	}

	public string ApplicationIdentifier;

	private string keyPairXml;

	private string keyFileName = "key.bin";

	[HideInInspector]
	public bool IsHosting = true;
}
