// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class Registry : SingletonMono<Registry>
{
	public static List<NID> DestroyedMapObjects
	{
		get
		{
			return Registry.destroyedMapObjects;
		}
	}

	public static Dictionary<NID, object> Components
	{
		get
		{
			return SingletonMono<Registry>.Instance.components;
		}
	}

	public static Dictionary<object, NID> Keys
	{
		get
		{
			return SingletonMono<Registry>.Instance.keys;
		}
	}

	public static Dictionary<NID, NetworkObject> ComponentsToSync
	{
		get
		{
			return SingletonMono<Registry>.Instance.componentsToSync;
		}
	}

	private void Awake()
	{
		if (Connect.SessionID == null)
		{
			Connect.SetRandomSessionID();
		}
		SingletonMono<Connect>.DestroyDuplicates();
		this.BroadcastMessageToAllMonoBehaviours("RegisterDeterministicObjects");
	}

	public static void RecacheDictionaries()
	{
		SingletonMono<Registry>.Instance.components = Registry.RecacheDictionary<NID, object>(Registry.Components);
		SingletonMono<Registry>.Instance.componentsToSync = Registry.RecacheDictionary<NID, NetworkObject>(Registry.ComponentsToSync);
	}

	public static Dictionary<T, T1> RecacheDictionary<T, T1>(Dictionary<T, T1> dict)
	{
		List<T> list = new List<T>(dict.Keys);
		List<T1> list2 = new List<T1>(dict.Values);
		dict = new Dictionary<T, T1>();
		for (int i = 0; i < list.Count; i++)
		{
			dict.Add(list[i], list2[i]);
		}
		return dict;
	}

	private void OnLevelWasLoaded()
	{
	}

	public void RegisterDeterministicObjects()
	{
		Registry.Reset();
		MonoBehaviour.print(">  ");
		MonoBehaviour.print("> ============================================= RegisterControllers ============================================== ");
		Registry.RegisterDeterminsiticGameObject(SingletonMono<Connect>.Instance.gameObject);
		if (SingletonNetObj<JoinScreen>.Instance != null)
		{
			Registry.RegisterDeterminsiticGameObject(SingletonNetObj<JoinScreen>.Instance.gameObject);
		}
		if (HeroController.Instance != null)
		{
			Registry.RegisterDeterminsiticGameObject(HeroController.Instance.gameObject);
		}
		if (EffectsController.instance != null)
		{
			Registry.RegisterDeterminsiticGameObject(EffectsController.instance.gameObject);
		}
		if (GameModeController.Instance != null)
		{
			Registry.RegisterDeterminsiticGameObject(GameModeController.Instance.gameObject);
		}
		if (ProjectileController.instance != null)
		{
			Registry.RegisterDeterminsiticGameObject(ProjectileController.instance.gameObject);
		}
		if (Map.Instance != null)
		{
			Registry.RegisterDeterminsiticGameObject(Map.Instance.gameObject);
		}
		if (MissionScreenController.instance != null)
		{
			Registry.RegisterDeterminsiticGameObject(MissionScreenController.instance.gameObject);
		}
		if (SingletonMono<DebugLobby>.Instance != null)
		{
			Registry.RegisterDeterminsiticGameObject(SingletonMono<DebugLobby>.Instance.gameObject);
		}
		if (SingletonMono<CutsceneCharacterController>.Instance != null)
		{
			Registry.RegisterDeterminsiticGameObject(SingletonMono<CutsceneCharacterController>.Instance.gameObject);
		}
	}

	public static void Reset()
	{
		MonoBehaviour.print("--- Reset Registry --- ");
		SingletonMono<Registry>.Instance.components.Clear();
		SingletonMono<Registry>.Instance.keys.Clear();
		SingletonMono<Registry>.Instance.componentsToSync.Clear();
		InstantiationController.Clear();
		SingletonMono<Registry>.Instance.registationOrder.Clear();
		SingletonMono<Registry>.Instance.DebugKeyList = null;
		SingletonMono<Registry>.Instance.DebugObjList = null;
		Registry.DestroyedMapObjects.Clear();
		SingletonMono<Registry>.Instance.playersThatIHaveSyncedWith.Clear();
		Registry.ResetIDs();
		SingletonMono<Registry>.Instance.registationOrder.Clear();
	}

	public static NID AllocateDeterministicID()
	{
		if (Registry.DeterminsticGameObjCount > 100000u)
		{
			UnityEngine.Debug.Log("DeterminsticCount might be getting a little high");
		}
		return new NID(Connect.SessionID, PID.TargetServer, Registry.DeterminsticGameObjCount++)
		{
			objName = "base"
		};
	}

	public static NID AllocatePlayerUniqueID()
	{
		if (Registry.DynamicallyAllocatedGameObjCount > 100000u)
		{
			UnityEngine.Debug.Log("DynamicallyAllocatedGameObjCount might be getting a little high");
		}
		return new NID(Connect.SessionID, PID.MyID, Registry.GetNewDynnamicID())
		{
			objName = "base"
		};
	}

	public static NID AllocateSceneID()
	{
		if (Registry.DynamicallyAllocatedGameObjCount > 100000u)
		{
			UnityEngine.Debug.Log("DynamicallyAllocatedGameObjCount might be getting a little high");
		}
		return new NID(Connect.SessionID, PID.TargetServer, Registry.GetNewDynnamicID());
	}

	private static uint GetNewDynnamicID()
	{
		uint num = 10000u;
		uint num2 = Registry.DynamicallyAllocatedGameObjCount++;
		if (num2 > num)
		{
			UnityEngine.Debug.LogError("Error. All IDs Assigned. Increase Range: " + num);
		}
		return num2 + (uint)((long)PID.MyID.AsInt * (long)((ulong)num));
	}

	private static void ResetIDs()
	{
		Registry.DeterminsticGameObjCount = 0u;
		Registry.DynamicallyAllocatedGameObjCount = 0u;
	}

	public static void RegisterDeterminsiticGameObject(GameObject go)
	{
		NID nid = Registry.AllocateDeterministicID();
		Registry.RegisterGameObject(ref nid, go);
	}

	public static bool RegisterGameObject(ref NID baseID, GameObject go)
	{
		if (Registry.DebugPrint)
		{
			MonoBehaviour.print("-RegisterGameObject- " + go);
		}
		if (!Registry.RegisterObject(ref baseID, go))
		{
			UnityEngine.Debug.LogError("Registration failed " + go);
			return false;
		}
		if (Registry.DebugPrint)
		{
			MonoBehaviour.print("-RegisterGameObjecssst- " + go);
		}
		Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
		if (Registry.DebugPrint)
		{
			MonoBehaviour.print("-RegisterGameObjecssst- " + go.GetComponents<Transform>().Length);
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i].GetComponent<NetworkStream>() != null))
			{
				NID nid = baseID.AllocateComponentID((byte)i, 0);
				GameObject gameObject = componentsInChildren[i].gameObject;
				if (gameObject != go && !Registry.RegisterObject(ref nid, gameObject))
				{
					UnityEngine.Debug.LogError("Registration failed " + gameObject);
					return false;
				}
				Component[] array = gameObject.GetComponents<Component>();
				if (Registry.DebugPrint)
				{
					MonoBehaviour.print("components " + array.Length);
				}
				for (int j = 0; j < array.Length; j++)
				{
					NID nid2 = baseID.AllocateComponentID((byte)i, (byte)(j + 1));
					if (!Registry.RegisterObject(ref nid2, array[j]))
					{
						UnityEngine.Debug.LogError("Registration failed " + gameObject);
						return false;
					}
				}
			}
		}
		return true;
	}

	public static bool RegisterObject(ref NID id, object comp)
	{
		Registry.CheckForNull<NID, object>(id, Registry.Components);
		if (Registry.DebugPrint)
		{
			MonoBehaviour.print(string.Concat(new object[]
			{
				"RegisterComponent ",
				comp,
				"  ",
				id
			}));
		}
		if (comp == null)
		{
			UnityEngine.Debug.Log("comp is null ");
			return false;
		}
		if (!Registry.Components.ContainsKey(id) && !Registry.Keys.ContainsKey(comp))
		{
			id.objName = string.Empty + comp;
			Registry.Components[id] = comp;
			Registry.Keys[comp] = id;
			SingletonMono<Registry>.Instance.registationOrder.Add(comp);
			return true;
		}
		UnityEngine.Debug.LogError(string.Concat(new object[]
		{
			"Cannot Register: ",
			id,
			" ",
			comp
		}));
		if (Registry.Components.ContainsKey(id))
		{
			UnityEngine.Debug.LogError("In use by: " + Registry.Components[id]);
		}
		else if (Registry.Keys.ContainsKey(comp))
		{
			UnityEngine.Debug.LogError("Already registered to " + Registry.Keys[comp]);
		}
		return false;
	}

	public static void CheckForNull<T1, T2>(T1 key, Dictionary<T1, T2> dict)
	{
		if (dict.ContainsKey(key))
		{
			T2 t = dict[key];
			if (t.Equals(null))
			{
				dict.Remove(key);
			}
		}
	}

	public static bool IsKeyInUse(NID key)
	{
		return Registry.Components.ContainsKey(key) && !Registry.Components[key].Equals(null);
	}

	public static NID GetNID(object comp)
	{
		if (comp == null || comp.Equals(null))
		{
			return NID.NoID;
		}
		if (Registry.Keys.ContainsKey(comp))
		{
			return Registry.Keys[comp];
		}
		if (!(comp is Map) && !(comp is HiddenExplosives))
		{
			UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Component not found ",
				comp.ToString(),
				"  ",
				comp.GetType(),
				" ",
				RPCController.CurrentRPC
			}));
		}
		return NID.NoID;
	}

	public static object GetObject(NID key)
	{
		if (Registry.destroyedMapObjects.Contains(key))
		{
			return null;
		}
		if (Registry.Components.ContainsKey(key))
		{
			return Registry.Components[key];
		}
		return null;
	}

	private void RefreshKeyList()
	{
		this.DebugKeyList = new List<NID>(this.components.Keys);
		this.DebugKeyList.Sort((NID A, NID B) => A.ToString().CompareTo(B.ToString()));
	}

	private void RefreshObjList()
	{
		this.DebugObjList = new List<object>(this.keys.Keys);
		this.DebugObjList.Sort((object A, object B) => A.ToString().CompareTo(B.ToString()));
	}

	public void DrawRegistrationOrder()
	{
		int num = Mathf.CeilToInt((float)this.registationOrder.Count / (float)this.entriesPerPage);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		this.currentPage = (int)GUILayout.HorizontalSlider((float)this.currentPage, 0f, (float)num, new GUILayoutOption[0]);
		string s = GUILayout.TextField(this.currentPage.ToString(), new GUILayoutOption[]
		{
			GUILayout.Width(40f)
		});
		int.TryParse(s, out this.currentPage);
		GUILayout.Label("of " + num, new GUILayoutOption[]
		{
			GUILayout.Width(45f)
		});
		if (GUILayout.Button("<<", new GUILayoutOption[]
		{
			GUILayout.Width(30f)
		}))
		{
			this.currentPage--;
		}
		if (GUILayout.Button(">>", new GUILayoutOption[]
		{
			GUILayout.Width(30f)
		}))
		{
			this.currentPage++;
		}
		if (this.currentPage < 0)
		{
			this.currentPage += num;
		}
		this.currentPage %= num;
		GUILayout.EndHorizontal();
		for (int i = 0; i < this.entriesPerPage; i++)
		{
			int num2 = this.currentPage * this.entriesPerPage + i;
			if (num2 < this.registationOrder.Count)
			{
				GUILayout.Label(num2 + ".  " + this.registationOrder[num2], new GUILayoutOption[0]);
			}
		}
	}

	public void DrawRegistry()
	{
		int num = Mathf.CeilToInt((float)this.components.Count / (float)this.entriesPerPage);
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		string text = "Order By Obj";
		if (!this.orderByKey)
		{
			text = "Order by Key";
		}
		if (GUILayout.Button(text, new GUILayoutOption[]
		{
			GUILayout.Width(110f)
		}))
		{
			this.orderByKey = !this.orderByKey;
			if (this.orderByKey)
			{
				this.RefreshKeyList();
			}
			else
			{
				this.RefreshObjList();
			}
		}
		this.currentPage = (int)GUILayout.HorizontalSlider((float)this.currentPage, 0f, (float)num, new GUILayoutOption[0]);
		string s = GUILayout.TextField(this.currentPage.ToString(), new GUILayoutOption[]
		{
			GUILayout.Width(40f)
		});
		int.TryParse(s, out this.currentPage);
		GUILayout.Label("of " + num, new GUILayoutOption[]
		{
			GUILayout.Width(45f)
		});
		if (GUILayout.Button("<<", new GUILayoutOption[]
		{
			GUILayout.Width(30f)
		}))
		{
			this.currentPage--;
		}
		if (GUILayout.Button(">>", new GUILayoutOption[]
		{
			GUILayout.Width(30f)
		}))
		{
			this.currentPage++;
		}
		if (this.currentPage < 0)
		{
			this.currentPage += num;
		}
		num = Mathf.Max(1, num);
		this.currentPage %= num;
		GUILayout.EndHorizontal();
		if (this.orderByKey)
		{
			this.DrawKeyFirst();
		}
		else
		{
			this.DrawObjFirst();
		}
	}

	private void DrawObjFirst()
	{
		if (this.DebugObjList == null)
		{
			this.RefreshObjList();
		}
		for (int i = 0; i < this.entriesPerPage; i++)
		{
			int num = this.currentPage * this.entriesPerPage + i;
			if (num < this.registationOrder.Count)
			{
				object obj = this.DebugObjList[num];
				try
				{
					this.DrawEnty(num, this.keys[obj], obj);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(ex.Message + "  " + obj);
				}
			}
		}
	}

	private void DrawKeyFirst()
	{
		if (this.DebugKeyList == null)
		{
			this.RefreshKeyList();
		}
		for (int i = 0; i < this.entriesPerPage; i++)
		{
			int num = this.currentPage * this.entriesPerPage + i;
			if (num < this.registationOrder.Count)
			{
				NID nid = this.DebugKeyList[num];
				try
				{
					this.DrawEnty(num, nid, this.components[nid]);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(ex.Message + "  " + nid);
				}
			}
		}
	}

	private void DrawEnty(int index, NID key, object obj)
	{
		string text = obj.ToString();
		MonoBehaviour monoBehaviour = obj as MonoBehaviour;
		if (monoBehaviour != null)
		{
			Block componentInHeirarchy = monoBehaviour.GetComponentInHeirarchy<Block>();
			if (componentInHeirarchy != null)
			{
				text = string.Concat(new object[]
				{
					"[",
					componentInHeirarchy.collumn,
					", ",
					componentInHeirarchy.row,
					"] ",
					text
				});
			}
		}
		GUILayout.Label(string.Concat(new object[]
		{
			index,
			".  ",
			key,
			"   ::    ",
			text
		}), new GUILayoutOption[0]);
	}

	public static void RequestNetworkObjectSync(PID requestee)
	{
		MonoBehaviour.print("> Registry.RequestNetworkObjectSync " + requestee);
		InstantiationController.SendInstantiatedPrefabs(requestee);
		if (Connect.IsHost)
		{
			InstantiationController.SendSceneOwnedObjects(requestee);
		}
		foreach (KeyValuePair<NID, object> keyValuePair in SingletonMono<Registry>.Instance.components)
		{
			MookArmouredGuy mookArmouredGuy = keyValuePair.Value as MookArmouredGuy;
			if (mookArmouredGuy != null)
			{
				mookArmouredGuy.SetPosition(mookArmouredGuy.transform.position);
			}
		}
		List<NID> list = new List<NID>();
		foreach (KeyValuePair<NID, object> keyValuePair2 in SingletonMono<Registry>.Instance.components)
		{
			try
			{
				NetworkObject networkObject = keyValuePair2.Value as NetworkObject;
				if (networkObject != null && networkObject.IsMine)
				{
					Block block = networkObject as Block;
					if (block != null && block.destroyed)
					{
						list.Add(networkObject.Nid);
					}
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> Sending Destroyed Blocks ",
			Useful.Round((float)(list.Count * 8) * 0.000976562f),
			" kb, ",
			list.Count,
			" obj"
		}));
		Networking.RPC<NID[]>(requestee, new RpcSignature<NID[]>(Registry.RecieveDestroyedBlocks), list.ToArray(), true);
		int num = 0;
		UnityStream unityStream = new UnityStream();
		foreach (KeyValuePair<NID, object> keyValuePair3 in SingletonMono<Registry>.Instance.components)
		{
			try
			{
				NetworkObject networkObject2 = keyValuePair3.Value as NetworkObject;
				if (networkObject2 != null && networkObject2.IsMine)
				{
					bool flag = networkObject2 is Block;
					if (!flag || networkObject2 is FallingBlock)
					{
						byte[] state = networkObject2.GetState();
						if (state.Length > 0)
						{
							num++;
							unityStream.Serialize<NID>(networkObject2.Nid);
							unityStream.Serialize<byte[]>(state);
						}
					}
				}
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogException(exception2);
			}
		}
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> Sending Sync ",
			Useful.Round((float)unityStream.ByteArray.Length * 0.000976562f),
			" kb, ",
			num,
			" obj"
		}));
		Networking.RPC<UnityStream>(requestee, new RpcSignature<UnityStream>(Registry.RecieveSync), unityStream, true);
		int num2 = 100;
		List<NID> list2 = new List<NID>();
		MonoBehaviour.print("> Sending DestroyedMapObjects  " + Registry.DestroyedMapObjects.Count);
		for (int i = 0; i < Registry.DestroyedMapObjects.Count; i++)
		{
			MonoBehaviour.print(Registry.DestroyedMapObjects[i]);
			list2.Add(Registry.DestroyedMapObjects[i]);
			if (i % num2 == 0 || i == Registry.DestroyedMapObjects.Count - 1)
			{
				Networking.RPC<NID[]>(requestee, new RpcSignature<NID[]>(Registry.RecieveDestroyedMapObjectBatch), list2.ToArray(), true);
				list2.Clear();
			}
		}
		Networking.RPC<PID>(requestee, new RpcSignature<PID>(Registry.SyncedWith), PID.MyID, true);
	}

	public static void RecieveDestroyedBlocks(NID[] destroyedBlocks)
	{
		foreach (NID nid in destroyedBlocks)
		{
			try
			{
				Block block = nid.Obj as Block;
				block.DestroyBlockInternal(false);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> RecieveDestroyedBlocks ",
			Useful.Round((float)(destroyedBlocks.Length * 8) * 0.000976562f),
			" kb, ",
			destroyedBlocks.Length,
			" obj"
		}));
	}

	public static void RecieveSync(UnityStream stream)
	{
		int num = 0;
		while (!stream.Finished)
		{
			try
			{
				NID nid = (NID)stream.DeserializeNext();
				byte[] state = (byte[])stream.DeserializeNext();
				nid.NetObject.SetState(state);
				num++;
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		MonoBehaviour.print(string.Concat(new object[]
		{
			"> RecieveSync ",
			Useful.Round((float)stream.ByteArray.Length * 0.000976562f),
			" kb, ",
			num,
			" obj"
		}));
	}

	public static void RecieveDestroyedMapObjectBatch(NID[] nids)
	{
		MonoBehaviour.print("> RecieveDestroyedMapObjectBatch " + nids.Length + " nids");
		for (int i = 0; i < nids.Length; i++)
		{
			try
			{
				if (!Registry.DestroyedMapObjects.Contains(nids[i]) && nids[i].NetObject != null && nids[i].NetObject.gameObject != null)
				{
					if (nids[i].NetObject.OnlyDestroyScriptOnSync)
					{
						UnityEngine.Object.Destroy(nids[i].NetObject);
					}
					else
					{
						UnityEngine.Object.Destroy(nids[i].NetObject.gameObject);
					}
				}
			}
			catch (Exception message)
			{
				UnityEngine.Debug.Log(message);
			}
		}
	}

	public static void SyncedWith(PID playerPID)
	{
		MonoBehaviour.print("> [9] Synced With " + playerPID);
		SingletonMono<Registry>.Instance.playersThatIHaveSyncedWith.Add(playerPID);
		foreach (PID pid in Connect.Layer.playerIdPairs.Keys)
		{
			if (!pid.IsMine && !SingletonMono<Registry>.Instance.playersThatIHaveSyncedWith.Contains(pid))
			{
				return;
			}
		}
		Connect.NotifyThatSceneIsfullySynced();
	}

	private void OnApplicationQuit()
	{
		Registry.IsExiting = true;
	}

	private Dictionary<NID, object> components = new Dictionary<NID, object>();

	private Dictionary<object, NID> keys = new Dictionary<object, NID>();

	private Dictionary<NID, NetworkObject> componentsToSync = new Dictionary<NID, NetworkObject>();

	private static List<NID> destroyedMapObjects = new List<NID>();

	public List<PID> playersThatIHaveSyncedWith = new List<PID>();

	public static bool DebugPrint = false;

	private List<object> registationOrder = new List<object>();

	private static uint DeterminsticGameObjCount = 0u;

	private static uint DynamicallyAllocatedGameObjCount = 0u;

	private int entriesPerPage = 15;

	private bool orderByKey;

	private List<NID> DebugKeyList;

	private List<object> DebugObjList;

	private int currentPage;

	public static bool IsExiting = false;
}
