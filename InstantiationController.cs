// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationController : SingletonMono<InstantiationController>
{
	public static List<UnityEngine.Object> PrefabList
	{
		get
		{
			if (InstantiationController.prefabList == null)
			{
				InstantiationController.prefabList = new List<UnityEngine.Object>(Resources.LoadAll(string.Empty, typeof(GameObject)));
			}
			return InstantiationController.prefabList;
		}
	}

	public static void Clear()
	{
		SingletonMono<InstantiationController>.Instance.MyInstantiatedPrefabs = new Dictionary<NID, InstantiationController.InstantiatedPrefab>();
		SingletonMono<InstantiationController>.Instance.SceneOwnedPrefabs = new Dictionary<NID, InstantiationController.InstantiatedPrefab>();
		SingletonMono<InstantiationController>.Instance.instances = new Dictionary<PID, List<GameObject>>();
	}

	public static void StoreTempData(byte[] data, NetworkObject obj)
	{
		if (obj == null)
		{
			return;
		}
		obj.instantiationDataBytes = data;
	}

	public static void ClearTempData(NetworkObject obj)
	{
		if (obj == null)
		{
			return;
		}
		obj.instantiationDataBytes = null;
	}

	public static void SendInstantiatedPrefabs(PID senderID)
	{
		SingletonMono<InstantiationController>.Instance.MyInstantiatedPrefabs.StripNullValues();
		List<NID> list = new List<NID>();
		foreach (KeyValuePair<NID, InstantiationController.InstantiatedPrefab> keyValuePair in SingletonMono<InstantiationController>.Instance.MyInstantiatedPrefabs)
		{
			try
			{
				InstantiationController.InstantiatedPrefab value = keyValuePair.Value;
				if (value.instance != null)
				{
					Vector3 position = value.instance.transform.position;
					Quaternion rotation = value.instance.transform.rotation;
					Networking.RPC<int, PID, NID, Vector3, Quaternion, byte[], bool>(senderID, new RpcSignature<int, PID, NID, Vector3, Quaternion, byte[], bool>(SingletonMono<InstantiationController>.Instance.InstatiateRPC), value.prefabIndex, PID.MyID, keyValuePair.Key, position, rotation, value.data, true, true);
				}
				else
				{
					list.Add(keyValuePair.Key);
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.Log(message);
			}
		}
		foreach (NID key in list)
		{
			SingletonMono<InstantiationController>.Instance.MyInstantiatedPrefabs.Remove(key);
		}
	}

	public static void SendSceneOwnedObjects(PID senderID)
	{
		foreach (KeyValuePair<NID, InstantiationController.InstantiatedPrefab> keyValuePair in SingletonMono<InstantiationController>.Instance.SceneOwnedPrefabs)
		{
			try
			{
				InstantiationController.InstantiatedPrefab value = keyValuePair.Value;
				if (value.instance != null)
				{
					Vector3 position = value.instance.transform.position;
					Quaternion rotation = value.instance.transform.rotation;
					Networking.RPC<int, PID, NID, Vector3, Quaternion, byte[], bool>(senderID, new RpcSignature<int, PID, NID, Vector3, Quaternion, byte[], bool>(SingletonMono<InstantiationController>.Instance.InstatiateRPC), value.prefabIndex, PID.TargetServer, keyValuePair.Key, position, rotation, value.data, true, false);
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
	}

	public static T NetworkedInstantiate<T>(PID playerID, T Prefab, Vector3 position, Quaternion rotation, object[] instantiationData, bool bufferThisRPC, bool executeImmediately) where T : UnityEngine.Object
	{
		T t = (T)((object)null);
		int prefabIndex = InstantiationController.GetPrefabIndex(Prefab.GetGameObject());
		if (prefabIndex < 0)
		{
			UnityEngine.Debug.LogError("Prefab not found " + Prefab + ". Make sure the prefab is located under a resourses folder");
		}
		else
		{
			if (instantiationData == null)
			{
				instantiationData = new object[0];
			}
			List<Type> list = new List<Type>();
			foreach (object obj in instantiationData)
			{
				if (obj != null)
				{
					list.Add(obj.GetType());
				}
				else
				{
					list.Add(typeof(object));
				}
			}
			byte[] array2 = TypeSerializer.SerializeParameterList(list.ToArray(), instantiationData);
			NID arg = Registry.AllocatePlayerUniqueID();
			GameObject gameObject = Prefab.GetGameObject();
			if (gameObject != null)
			{
				InstantiationController.StoreTempData(array2, gameObject.GetComponent<NetworkObject>());
				t = (UnityEngine.Object.Instantiate(Prefab, position, rotation) as T);
				InstantiationController.ClearTempData(gameObject.GetComponent<NetworkObject>());
				SingletonMono<InstantiationController>.Instance.InitializeInstance(t.GetGameObject(), prefabIndex, playerID, ref arg, array2, bufferThisRPC);
			}
			else
			{
				UnityEngine.Debug.LogError("is not Mono " + Prefab);
			}
			Networking.RPC<int, PID, NID, Vector3, Quaternion, byte[], bool>(PID.TargetOthers, new RpcSignature<int, PID, NID, Vector3, Quaternion, byte[], bool>(SingletonMono<InstantiationController>.Instance.InstatiateRPC), prefabIndex, playerID, arg, position, rotation, array2, bufferThisRPC, executeImmediately);
		}
		return t;
	}

	private void InstatiateRPC(int indexOfPrefab, PID playerID, NID networkID, Vector3 position, Quaternion rotation, byte[] instantiationDataAsBytes, bool buffer)
	{
		GameObject prefabFromIndex = InstantiationController.GetPrefabFromIndex(indexOfPrefab);
		if (Registry.IsKeyInUse(networkID))
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Key is in use ",
				prefabFromIndex,
				"  ",
				Registry.Components[networkID]
			}));
			return;
		}
		if (prefabFromIndex != null)
		{
			InstantiationController.StoreTempData(instantiationDataAsBytes, prefabFromIndex.GetComponent<NetworkObject>());
			GameObject obj = UnityEngine.Object.Instantiate(prefabFromIndex, position, rotation) as GameObject;
			InstantiationController.ClearTempData(prefabFromIndex.GetComponent<NetworkObject>());
			this.InitializeInstance(obj, indexOfPrefab, playerID, ref networkID, instantiationDataAsBytes, buffer);
		}
	}

	private void InitializeInstance(GameObject obj, int prefabIndex, PID playerID, ref NID baseID, byte[] instantiationDataAsBytes, bool buffer)
	{
		if (!Registry.RegisterGameObject(ref baseID, obj))
		{
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
			{
				"Object already registered. Destroying it. ",
				obj,
				" ",
				baseID
			}));
			UnityEngine.Object.Destroy(obj);
			return;
		}
		if (playerID != PID.TargetServer)
		{
			if (!this.instances.ContainsKey(playerID))
			{
				this.instances.Add(playerID, new List<GameObject>());
			}
			List<GameObject> list = this.instances[playerID];
			list.Add(obj);
		}
		NetworkObject component = obj.GetComponent<NetworkObject>();
		if (component != null)
		{
			if (component.InstantiationData == null)
			{
			}
			InstantiationController.InstantiatedPrefab value = new InstantiationController.InstantiatedPrefab(prefabIndex, component, instantiationDataAsBytes);
			if (playerID == PID.MyID && buffer)
			{
				SingletonMono<InstantiationController>.Instance.MyInstantiatedPrefabs.Add(baseID, value);
			}
			if (playerID == PID.TargetServer && buffer)
			{
				SingletonMono<InstantiationController>.Instance.SceneOwnedPrefabs.Add(baseID, value);
			}
			component.NetworkSetup(playerID);
		}
	}

	private static int GetPrefabIndex(UnityEngine.Object Prefab)
	{
		if (InstantiationController.PrefabList.Contains(Prefab))
		{
			return InstantiationController.PrefabList.IndexOf(Prefab);
		}
		return -1;
	}

	private static GameObject GetPrefabFromIndex(int index)
	{
		if (index < 0)
		{
			UnityEngine.Debug.LogWarning("Invalid Index" + index);
			return null;
		}
		if (InstantiationController.PrefabList.Count > index)
		{
			return (GameObject)InstantiationController.PrefabList[index];
		}
		return null;
	}

	public static void Destroy_Networked(GameObject gameObject)
	{
		if (gameObject == null)
		{
			return;
		}
		Networking.RPC<GameObject>(PID.TargetAll, new RpcSignature<GameObject>(InstantiationController.Destroy_Local), gameObject, false);
	}

	public static void Destroy_Local(GameObject goToDestoy)
	{
		NID nid = Registry.GetNID(goToDestoy);
		if (SingletonMono<InstantiationController>.Instance.MyInstantiatedPrefabs.ContainsKey(nid))
		{
			SingletonMono<InstantiationController>.Instance.MyInstantiatedPrefabs.Remove(nid);
		}
		if (SingletonMono<InstantiationController>.Instance.SceneOwnedPrefabs.ContainsKey(nid))
		{
			SingletonMono<InstantiationController>.Instance.SceneOwnedPrefabs.Remove(nid);
		}
		PID ownerID = nid.OwnerID;
		if (SingletonMono<InstantiationController>.Instance.instances.ContainsKey(ownerID))
		{
			List<GameObject> list = SingletonMono<InstantiationController>.Instance.instances[ownerID];
			list.Remove(goToDestoy);
		}
		UnityEngine.Object.Destroy(goToDestoy);
	}

	private static void RemoveValue<T1, T2>(Dictionary<T1, T2> dictionary, T2 valueToRemove)
	{
		foreach (KeyValuePair<T1, T2> keyValuePair in dictionary)
		{
			T2 value = keyValuePair.Value;
			if (value.Equals(valueToRemove))
			{
				dictionary.Remove(keyValuePair.Key);
				break;
			}
		}
	}

	public static void DeregisterPlayer(PID playerID)
	{
		if (SingletonMono<InstantiationController>.Instance.instances.ContainsKey(playerID))
		{
			List<GameObject> list = new List<GameObject>(SingletonMono<InstantiationController>.Instance.instances[playerID]);
			foreach (GameObject goToDestoy in list)
			{
				InstantiationController.Destroy_Local(goToDestoy);
			}
			SingletonMono<InstantiationController>.Instance.instances[playerID].Clear();
			SingletonMono<InstantiationController>.Instance.instances.Remove(playerID);
		}
	}

	private Dictionary<NID, InstantiationController.InstantiatedPrefab> MyInstantiatedPrefabs = new Dictionary<NID, InstantiationController.InstantiatedPrefab>();

	private Dictionary<NID, InstantiationController.InstantiatedPrefab> SceneOwnedPrefabs = new Dictionary<NID, InstantiationController.InstantiatedPrefab>();

	private Dictionary<PID, List<GameObject>> instances = new Dictionary<PID, List<GameObject>>();

	private static List<UnityEngine.Object> prefabList;

	private class InstantiatedPrefab
	{
		public InstantiatedPrefab(int PrefabIndex, NetworkObject Instance, byte[] Data)
		{
			this.prefabIndex = PrefabIndex;
			this.instance = Instance;
			this.data = Data;
		}

		public int prefabIndex;

		public NetworkObject instance;

		public byte[] data;
	}
}
