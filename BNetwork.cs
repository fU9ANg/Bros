// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Badumna;
using Badumna.Match;
using UnityEngine;

public class BNetwork : SingletonMono<BNetwork>
{
	public static INetworkFacade iNeteworkFacade
	{
		get
		{
			return BNetwork.inetworkFacade;
		}
	}

	public static float KbReceived
	{
		get
		{
			if (BNetwork.inetworkFacade != null)
			{
				return (float)System.Math.Round(BNetwork.inetworkFacade.InboundBytesPerSecond * 0.00097656197613105178, 1);
			}
			return 0f;
		}
	}

	public static float KbSent
	{
		get
		{
			if (BNetwork.inetworkFacade != null)
			{
				return (float)System.Math.Round(BNetwork.inetworkFacade.OutboundBytesPerSecond * 0.00097656197613105178, 1);
			}
			return 0f;
		}
	}

	public static void TryLogin()
	{
		SingletonMono<BNetwork>.Instance.StartCoroutine(SingletonMono<BNetwork>.Instance.TryLoginRoutine());
	}

	private IEnumerator TryLoginRoutine()
	{
		MonoBehaviour.print("> Try Log In as " + Connect.PlayerName);
		this.LogInfailed = false;
		this.LogInSucceeded = false;
		if (this.attemptingLogin)
		{
			UnityEngine.Debug.LogWarning("> Already attempting log in");
			yield break;
		}
		this.attemptingLogin = true;
		BNetwork.status = "Shutting Down...";
		yield return null;
		if (BNetwork.iNeteworkFacade != null)
		{
			BNetwork.iNeteworkFacade.Shutdown();
			BNetwork.status = BNetwork.iNeteworkFacade.IsLoggedIn + " " + BNetwork.iNeteworkFacade.IsOffline;
			BNetwork.inetworkFacade = null;
		}
		BNetwork.status = "Awaiting Log In...";
		yield return null;
		SingletonMono<BNetwork>.Instance.initializationResult = NetworkFacade.BeginCreate(SingletonMono<BadumnaController>.Instance.ApplicationIdentifier, null);
		BNetwork.status = "Begun Create...";
		while (!this.initializationResult.IsCompleted)
		{
			BNetwork.status = "BeginCreate...";
			yield return null;
		}
		if (BNetwork.inetworkFacade == null)
		{
			try
			{
				BNetwork.status = "Initializing Badumna...";
				BNetwork.inetworkFacade = NetworkFacade.EndCreate(this.initializationResult);
				BNetwork.inetworkFacade.AddressChangedEvent += SingletonMono<BadumnaController>.Instance.AddressChangedEventHandler;
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				string errorMessage = "Badumna initialization failed: " + ex.Message;
				BNetwork.status = errorMessage;
				UnityEngine.Debug.LogError(errorMessage);
				this.LogInfailed = true;
				this.attemptingLogin = false;
				yield break;
			}
		}
		if (!BNetwork.iNeteworkFacade.Login(Connect.PlayerName, SingletonMono<BadumnaController>.Instance.KeyPairXml))
		{
			this.LogInfailed = true;
			BNetwork.status = "Failed to login";
			UnityEngine.Debug.LogError("> Failed to login");
			this.attemptingLogin = false;
			yield break;
		}
		BNetwork.inetworkFacade.RegisterEntityDetails(20f, 6f);
		this.RegisterCustomTypes();
		BNetwork.status = "Logged In";
		this.LogInSucceeded = true;
		this.attemptingLogin = false;
		UnityEngine.Debug.Log("> Log in successful");
		yield break;
	}

	private void RegisterCustomTypes()
	{
		BNetwork.inetworkFacade.TypeRegistry.RegisterMutableReferenceType<Dictionary<string, int>>(delegate(Dictionary<string, int> d, BinaryWriter w)
		{
			w.Write(d.Count);
			foreach (KeyValuePair<string, int> keyValuePair in d)
			{
				w.Write(keyValuePair.Key);
				w.Write(keyValuePair.Value);
			}
		}, delegate(BinaryReader r)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			int num = r.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				dictionary.Add(r.ReadString(), r.ReadInt32());
			}
			return dictionary;
		}, delegate(Dictionary<string, int> d)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (KeyValuePair<string, int> keyValuePair in d)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return dictionary;
		});
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<Dictionary<string, int>>();
		BNetwork.iNeteworkFacade.TypeRegistry.RegisterMutableReferenceType<PID>(delegate(PID b, BinaryWriter w)
		{
			if (b == null)
			{
				w.Write(-1);
			}
			else
			{
				w.Write(b.AsByte);
			}
		}, delegate(BinaryReader r)
		{
			int num = (int)r.ReadByte();
			if (num == -1)
			{
				return null;
			}
			return (PID)num;
		}, (PID c) => new PID(c.AsByte));
		BNetwork.iNeteworkFacade.TypeRegistry.RegisterMutableReferenceType<byte[]>(delegate(byte[] b, BinaryWriter w)
		{
			if (b == null)
			{
				w.Write(-1);
			}
			else
			{
				w.Write(b.Length);
				w.Write(b);
			}
		}, delegate(BinaryReader r)
		{
			int num = r.ReadInt32();
			if (num == -1)
			{
				return null;
			}
			return r.ReadBytes(num);
		}, delegate(byte[] c)
		{
			byte[] array = new byte[c.Length];
			Array.Copy(c, array, c.Length);
			return array;
		});
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<byte[]>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<double>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<PID>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<byte, MemberIdentity>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<byte, byte, MemberIdentity>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<byte, byte, string, string, MemberIdentity>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<byte, byte, string, string>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<byte, string>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<byte, string, byte, byte>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<byte, byte>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<MemberIdentity>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<byte>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<MemberIdentity, byte>();
		BNetwork.inetworkFacade.RPCManager.RegisterRPCSignature<PID, bool>();
	}

	public static void Shutdown()
	{
		if (BNetwork.inetworkFacade != null)
		{
			BNetwork.inetworkFacade.ProcessNetworkState();
			BNetwork.inetworkFacade.Shutdown();
			BNetwork.inetworkFacade = null;
			SingletonMono<BadumnaController>.Instance.GenerateKeyPair();
		}
	}

	private static INetworkFacade inetworkFacade;

	public static string status;

	private IAsyncResult initializationResult;

	private bool attemptingLogin;

	[HideInInspector]
	public bool LogInfailed;

	[HideInInspector]
	public bool LogInSucceeded;
}
