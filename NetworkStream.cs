// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkStream : MonoBehaviour
{
	public void Send(RPCObject rpc)
	{
		if (Network.connections.Length == 0)
		{
			return;
		}
		this.bytesArraysToSend.Enqueue(rpc);
	}

	private void Receive(byte[] bytesRecieved)
	{
		NetworkStream.RecievedByteArray item = default(NetworkStream.RecievedByteArray);
		item.byteArray = bytesRecieved;
		item.TimeStamp = Connect.Time + (double)Connect.SimulatedLatency;
		if (this.streamType == NetworkStream.StreamType.Unreliable)
		{
			NetworkStream.byteArraysRecievedUnRelilable.Enqueue(item);
		}
		else
		{
			NetworkStream.byteArraysRecievedRelilable.Enqueue(item);
		}
	}

	public byte[] RecieveNext()
	{
		Queue<NetworkStream.RecievedByteArray> queue = NetworkStream.byteArraysRecievedUnRelilable;
		if (this.streamType == NetworkStream.StreamType.Reliable)
		{
			queue = NetworkStream.byteArraysRecievedRelilable;
		}
		if (queue.Count == 0)
		{
			return null;
		}
		if (queue.Peek().TimeStamp > Connect.Time)
		{
			return null;
		}
		return queue.Dequeue().byteArray;
	}

	private void Destroy()
	{
	}

	private void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		try
		{
			Network.Destroy(base.gameObject);
		}
		catch (Exception message)
		{
			MonoBehaviour.print(message);
		}
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			int num = (int)PID.MyID;
			stream.Serialize(ref num);
			int count = this.bytesArraysToSend.Count;
			stream.Serialize(ref count);
			for (int i = 0; i < count; i++)
			{
				RPCObject rpcobject = this.bytesArraysToSend.Dequeue();
				byte[] bytes;
				if (rpcobject is NonStaticRPCObject)
				{
					bytes = TypeSerializer.ObjectToByteArray<NonStaticRPCObject>((NonStaticRPCObject)rpcobject);
				}
				else
				{
					bytes = TypeSerializer.ObjectToByteArray<StaticRPCObject>((StaticRPCObject)rpcobject);
				}
				char[] array = NetworkStream.ConvertBytesToChars(bytes);
				int num2 = array.Length;
				if (num2 != 0)
				{
					stream.Serialize(ref num2);
					for (int j = 0; j < num2; j++)
					{
						stream.Serialize(ref array[j]);
					}
				}
			}
			this.bytesArraysToSend.Clear();
		}
		else
		{
			int num3 = -1;
			stream.Serialize(ref num3);
			int num4 = 0;
			stream.Serialize(ref num4);
			for (int k = 0; k < num4; k++)
			{
				int num5 = 0;
				stream.Serialize(ref num5);
				if (num5 != 0)
				{
					char[] array2 = new char[num5];
					for (int l = 0; l < num5; l++)
					{
						stream.Serialize(ref array2[l]);
					}
					byte[] array3 = NetworkStream.ConvertCharsToBytes(array2);
					RPCObject rpcobject2 = (RPCObject)TypeSerializer.BytesToObject(array3);
					bool flag = false;
					if (rpcobject2.messageInfo.Destination == PID.TargetOthers || rpcobject2.messageInfo.Destination == PID.TargetAll)
					{
						this.Receive(array3);
						if (Network.isServer)
						{
							flag = true;
						}
					}
					else if (rpcobject2.messageInfo.Destination == PID.MyID || rpcobject2.messageInfo.Destination == PID.TargetServer || PID.ServerID == PID.MyID)
					{
						this.Receive(array3);
					}
					else
					{
						UnityEngine.Debug.Log(string.Concat(new object[]
						{
							"Ignoring ",
							rpcobject2,
							" to ",
							rpcobject2.messageInfo.Destination
						}));
						if (Network.isServer)
						{
							flag = true;
						}
					}
					if (flag)
					{
						this.bytesArraysToSend.Enqueue(rpcobject2);
					}
				}
			}
		}
	}

	public static char[] ConvertBytesToChars(byte[] bytes)
	{
		char[] array = new char[bytes.Length];
		for (int i = 0; i < bytes.Length; i++)
		{
			array[i] = (char)bytes[i];
		}
		return array;
	}

	public static byte[] ConvertCharsToBytes(char[] chars)
	{
		byte[] array = new byte[chars.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (byte)chars[i];
		}
		return array;
	}

	public NetworkStream.StreamType streamType = NetworkStream.StreamType.Unreliable;

	private static Queue<NetworkStream.RecievedByteArray> byteArraysRecievedUnRelilable = new Queue<NetworkStream.RecievedByteArray>();

	private static Queue<NetworkStream.RecievedByteArray> byteArraysRecievedRelilable = new Queue<NetworkStream.RecievedByteArray>();

	private Queue<RPCObject> bytesArraysToSend = new Queue<RPCObject>();

	public enum StreamType
	{
		Reliable,
		Unreliable
	}

	private struct RecievedByteArray
	{
		public byte[] byteArray;

		public double TimeStamp;
	}
}
