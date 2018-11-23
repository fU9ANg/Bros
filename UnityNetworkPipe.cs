// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class UnityNetworkPipe : SingletonNetObj<UnityNetworkPipe>
{
	private static UnityNetworkPipe LocalInstance
	{
		get
		{
			if (Network.connections.Length > 0 && UnityNetworkPipe.localInstance == null)
			{
				UnityNetworkPipe.InstantiateLocalStream();
			}
			return UnityNetworkPipe.localInstance;
		}
	}

	private void Awake()
	{
		base.transform.parent = SingletonMono<Connect>.Instance.transform;
		base.transform.localPosition = Vector3.zero;
	}

	public static void InstantiateLocalStream()
	{
		GameObject prefab = Resources.Load("Stream") as GameObject;
		GameObject gameObject = Network.Instantiate(prefab, Vector3.zero, Quaternion.identity, 0) as GameObject;
		GameObject gameObject2 = gameObject;
		gameObject2.name += " (Local)";
		UnityNetworkPipe.localInstance = gameObject.GetComponent<UnityNetworkPipe>();
	}

	public static byte[] RecieveNextUnreliable()
	{
		if (UnityNetworkPipe.LocalInstance != null)
		{
			return UnityNetworkPipe.LocalInstance.UnreliableStream.RecieveNext();
		}
		return null;
	}

	public static void SendUnreliable(RPCObject rpc)
	{
		if (UnityNetworkPipe.LocalInstance != null)
		{
			UnityNetworkPipe.LocalInstance.UnreliableStream.Send(rpc);
		}
	}

	public static byte[] RecieveNextReliable()
	{
		if (UnityNetworkPipe.LocalInstance != null)
		{
			return UnityNetworkPipe.LocalInstance.ReliableStream.RecieveNext();
		}
		return null;
	}

	public static void SendReliable(RPCObject rpc)
	{
		if (UnityNetworkPipe.LocalInstance != null)
		{
			UnityNetworkPipe.LocalInstance.ReliableStream.Send(rpc);
		}
	}

	public static void DestoyLocalStream()
	{
		Network.Destroy(UnityNetworkPipe.LocalInstance.gameObject);
	}

	public void OnPlayerDisconnected(NetworkPlayer player)
	{
		if (player == base.GetComponent<NetworkView>().owner)
		{
			Network.Destroy(base.gameObject);
			Network.DestroyPlayerObjects(player);
		}
	}

	public NetworkStream ReliableStream;

	public NetworkStream UnreliableStream;

	private static UnityNetworkPipe localInstance;
}
