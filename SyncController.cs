// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SyncController : MonoBehaviour
{
	public static SyncController Instance
	{
		get
		{
			if (SyncController.instance == null)
			{
				SyncController.instance = (UnityEngine.Object.FindObjectOfType(typeof(SyncController)) as SyncController);
			}
			return SyncController.instance;
		}
	}

	private void Update()
	{
		if (Connect.IsOffline)
		{
			return;
		}
		float num = 1f / (float)SyncController.SendRate;
		this.sendTimer -= NetworkTimeSync.RealDeltaTime;
		this.sendTimer = Mathf.Max(0f, this.sendTimer);
		if (this.sendTimer <= 0f)
		{
			byte[] array = this.SerializeAllOwnedObjects();
			if (array != null && array.Length > 0)
			{
				Networking.UnreliableRPC<byte[]>(PID.TargetOthers, true, false, new RpcSignature<byte[]>(this.RecieveStatesRPC), array);
			}
			this.sendTimer += num;
		}
	}

	private void RecieveStatesRPC(byte[] bytes)
	{
		if (bytes != null)
		{
			List<State> states = this.DeserializeAllProperties(bytes);
			this.DispatchStates(states);
		}
	}

	private byte[] SerializeAllOwnedObjects()
	{
		UnityStream unityStream = new UnityStream();
		List<NID> list = new List<NID>();
		int num = 0;
		foreach (KeyValuePair<NID, NetworkObject> keyValuePair in Registry.ComponentsToSync)
		{
			if (keyValuePair.Value == null)
			{
				list.Add(keyValuePair.Key);
			}
			else if (keyValuePair.Value.Syncronize && keyValuePair.Value.IsMine && keyValuePair.Value.ReadyTobeSynced())
			{
				State obj = new State(keyValuePair.Key, keyValuePair.Value);
				unityStream.Serialize<State>(obj);
				num++;
			}
		}
		foreach (NID key in list)
		{
			Registry.ComponentsToSync.Remove(key);
		}
		return unityStream.ByteArray;
	}

	private List<State> DeserializeAllProperties(byte[] byteArray)
	{
		List<State> list = new List<State>();
		UnityStream unityStream = new UnityStream(byteArray);
		while (!unityStream.Finished)
		{
			State item = (State)unityStream.DeserializeNext();
			list.Add(item);
		}
		return list;
	}

	private void DispatchStates(List<State> states)
	{
		foreach (State state in states)
		{
			try
			{
				if (Registry.Components.ContainsKey(state.Key))
				{
					NetworkObject networkObject = Registry.Components[state.Key] as NetworkObject;
					if (networkObject == null)
					{
						UnityEngine.Debug.LogWarning(string.Concat(new object[]
						{
							"Obj is null for state syncing ",
							state.Key,
							" <",
							state.Key.Obj,
							"> "
						}));
					}
					if (networkObject.IsMine)
					{
						UnityEngine.Debug.LogWarning(string.Concat(new object[]
						{
							"Warning: ",
							networkObject,
							" is owned locally, so its state sholud not be set. Key: ",
							state.Key
						}));
					}
					networkObject.AddState(state);
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception, this);
			}
		}
	}

	public static void GetSyncedPropertyInfos(UnityEngine.Object obj, out PropertyInfo[] propsOut, out bool[] propsToInterpolate)
	{
		List<PropertyInfo> list = new List<PropertyInfo>();
		List<bool> list2 = new List<bool>();
		PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (PropertyInfo propertyInfo in properties)
		{
			object[] customAttributes = propertyInfo.GetCustomAttributes(true);
			foreach (object obj2 in customAttributes)
			{
				Syncronize syncronize = obj2 as Syncronize;
				if (syncronize != null)
				{
					list.Add(propertyInfo);
					list2.Add(syncronize.Interpolate);
					break;
				}
			}
		}
		propsOut = list.ToArray();
		propsToInterpolate = list2.ToArray();
	}

	public static object[] GetPropertyValues(PropertyInfo[] props, UnityEngine.Object obj)
	{
		object[] array = new object[props.Length];
		for (int i = 0; i < props.Length; i++)
		{
			array[i] = props[i].GetValue(obj, null);
		}
		return array;
	}

	private static int SendRate = 12;

	public static float SendInterval = 1f / (float)SyncController.SendRate;

	private static SyncController instance;

	private float sendTimer;
}
