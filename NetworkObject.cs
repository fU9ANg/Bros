// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class NetworkObject : MonoBehaviour
{
	public PID Owner
	{
		get
		{
			return this.owner;
		}
	}

	public bool Syncronize
	{
		get
		{
			return this.syncronize;
		}
	}

	public PropertyInfo[] Props
	{
		get
		{
			if (this.props == null)
			{
				SyncController.GetSyncedPropertyInfos(this, out this.props, out this.PropsToTryInterpolate);
			}
			return this.props;
		}
	}

	private Vector3 Position
	{
		get
		{
			if (this.myTransform == null)
			{
				this.myTransform = base.transform;
			}
			return this.myTransform.position;
		}
	}

	public NID Nid
	{
		get
		{
			if (SingletonMono<Registry>.Instance == null)
			{
				return this.nid;
			}
			if (this.nid == NID.NoID && Registry.Keys.ContainsKey(this))
			{
				this.nid = Registry.Keys[this];
			}
			return this.nid;
		}
	}

	public object[] InstantiationData
	{
		get
		{
			if (this.instantiationData == null)
			{
				if (this.instantiationDataBytes == null)
				{
					this.instantiationData = new object[0];
				}
				else
				{
					this.instantiationData = TypeSerializer.DeserializeParameterList(this.instantiationDataBytes, null);
				}
			}
			return this.instantiationData;
		}
	}

	public void SetOwner(PID newPID)
	{
		this.owner = newPID;
	}

	public bool IsMine
	{
		get
		{
			if (this.owner == PID.TargetServer)
			{
				return Connect.IsHost;
			}
			return this.owner == PID.MyID;
		}
	}

	public virtual void NetworkSetup(PID newOwner)
	{
		this.owner = newOwner;
		if (this.IsMine)
		{
			GameObject gameObject = base.gameObject;
			gameObject.name += " [local]";
		}
		else
		{
			Networking.RPC<PID>(this.owner, new RpcSignature<PID>(this.RequestStateSync), PID.MyID, false);
		}
	}

	public static void PlaceMarker(Vector2 pos)
	{
		if (NetworkObject.ShowInterpolatedPosiontMarkers)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.position = pos;
			gameObject.transform.localScale = Vector3.one * 2f;
			gameObject.AddComponent<Autodestruct>();
		}
	}

	public void AddState(State state)
	{
		if (NetworkObject.ShowInterpolatedPosiontMarkers)
		{
			for (int i = 0; i < state.Values.Length; i++)
			{
				if (state.Values[i] is Vector2)
				{
					NetworkObject.PlaceMarker((Vector2)state.Values[i]);
				}
			}
		}
		this.states.Add(state);
		this.states.Sort(new Comparison<State>(NetworkObject.CompareStates));
	}

	private static int CompareStates(State A, State B)
	{
		if (A.TimeStamp > B.TimeStamp)
		{
			return 1;
		}
		if (A.TimeStamp < B.TimeStamp)
		{
			return -1;
		}
		return 0;
	}

	protected virtual void LateUpdate()
	{
		this.RunSync();
	}

	public void RunSync()
	{
		if (!this.Syncronize)
		{
			return;
		}
		if (this is MammothKopter)
		{
			MonoBehaviour.print("Run Sync " + this);
		}
		if (this.states.Count == 0)
		{
			return;
		}
		if (this.IsMine)
		{
			this.states.Clear();
			return;
		}
		this.DiscardElapsedSerializedStates();
		if (this.states.Count == 1)
		{
			object[] interpolatedValues = this.GetInterpolatedValues(this.states[0], null, 0f);
			this.ApplyState(interpolatedValues);
		}
		else if (this.states.Count > 1)
		{
			State state = this.states[0];
			State state2 = this.states[1];
			double num = (double)(state2.TimeStamp - state.TimeStamp);
			double num2 = ((double)state2.TimeStamp - Connect.GetInterpTime(this.owner)) / num;
			float lerp = 1f - (float)num2;
			object[] interpolatedValues2 = this.GetInterpolatedValues(this.states[0], this.states[1], lerp);
			this.ApplyState(interpolatedValues2);
		}
	}

	private double Truncate(double t, float target)
	{
		int num = 0;
		while (t > (double)target)
		{
			t %= 10.0;
			num++;
		}
		return t;
	}

	protected void DiscardElapsedSerializedStates()
	{
		if (this.states.Count == 0)
		{
			return;
		}
		int num = -1;
		string arg = string.Empty;
		for (int i = 0; i < this.states.Count; i++)
		{
			arg = arg + this.states[i].TimeStamp + "    ";
			if ((double)this.states[i].TimeStamp >= Connect.GetInterpTime(this.owner))
			{
				num = i - 1;
				break;
			}
			num = i;
		}
		if (num >= 0)
		{
			this.states.RemoveRange(0, num);
		}
	}

	private void ApplyState(object[] interpolatedValues)
	{
		try
		{
			for (int i = 0; i < interpolatedValues.Length; i++)
			{
				this.Props[i].SetValue(this, interpolatedValues[i], null);
			}
		}
		catch (Exception exception)
		{
			MonoBehaviour.print("Exception in " + base.gameObject);
			UnityEngine.Debug.LogException(exception);
		}
	}

	private object[] GetInterpolatedValues(State prev, State next, float lerp)
	{
		if (next == null)
		{
			return prev.Values;
		}
		if (prev == null)
		{
			return next.Values;
		}
		object[] array = new object[prev.Values.Length];
		for (int i = 0; i < this.Props.Length; i++)
		{
			array[i] = this.TryInterpolateValue(prev.Values[i], next.Values[i], this.PropsToTryInterpolate[i], lerp);
		}
		return array;
	}

	private object TryInterpolateValue(object prevValue, object nextValue, bool interpolate, float lerp)
	{
		if (interpolate)
		{
			if (prevValue is float)
			{
				return Mathf.Lerp((float)prevValue, (float)nextValue, lerp);
			}
			if (prevValue is Vector3)
			{
				return Vector3.Lerp((Vector3)prevValue, (Vector3)nextValue, lerp);
			}
			if (prevValue is Vector2)
			{
				return Vector2.Lerp((Vector2)prevValue, (Vector2)nextValue, lerp);
			}
			if (prevValue is Vector4)
			{
				return Vector4.Lerp((Vector4)prevValue, (Vector4)nextValue, lerp);
			}
			if (prevValue is Quaternion)
			{
				return Quaternion.Slerp((Quaternion)prevValue, (Quaternion)nextValue, lerp);
			}
			if (prevValue is Color)
			{
				return Color.Lerp((Color)prevValue, (Color)nextValue, lerp);
			}
			if (prevValue is ParentedPosition)
			{
				return ParentedPosition.Lerp((ParentedPosition)prevValue, (ParentedPosition)nextValue, lerp);
			}
		}
		if (lerp > 0.5f)
		{
			return nextValue;
		}
		return prevValue;
	}

	protected void RequestStateSync(PID requestee)
	{
		byte[] state = this.GetState();
		if (state.Length != 0)
		{
			Networking.RPC<byte[]>(requestee, new RpcSignature<byte[]>(this.SetState), state, false);
		}
	}

	public byte[] GetState()
	{
		if (this.Syncronize && this.IsMine)
		{
			this.EnableSyncing(true, false);
		}
		UnityStream unityStream = new UnityStream();
		this.PackState(unityStream);
		return unityStream.ByteArray;
	}

	public void SetState(byte[] bytes)
	{
		UnityStream stream = new UnityStream(bytes);
		this.UnpackState(stream);
	}

	public virtual UnityStream PackState(UnityStream stream)
	{
		return stream;
	}

	public virtual UnityStream UnpackState(UnityStream stream)
	{
		return stream;
	}

	public void DestroyNetworked()
	{
		Networking.RPC<GameObject>(PID.TargetAll, new RpcSignature<GameObject>(UnityEngine.Object.Destroy), base.gameObject, false);
	}

	public void EnableSyncing(bool enabled, bool executeImmediately)
	{
		if (this.IsMine)
		{
			Networking.RPC<bool>(PID.TargetAll, executeImmediately, false, new RpcSignature<bool>(this.SetSyncingInternal), enabled);
		}
		else
		{
			UnityEngine.Debug.LogWarning("Only the owner can set syncing " + base.gameObject);
		}
	}

	protected void SetSyncingInternal(bool enabled)
	{
		if (enabled)
		{
			Registry.ComponentsToSync[this.Nid] = this;
		}
		else if (Registry.ComponentsToSync.ContainsKey(this.Nid))
		{
			Registry.ComponentsToSync.Remove(this.Nid);
		}
		this.syncronize = enabled;
		if (enabled && this != null)
		{
			EnemyAI component = base.GetComponent<EnemyAI>();
			if (component != null)
			{
				component.enabled = this.IsMine;
			}
		}
	}

	protected virtual void OnDestroy()
	{
		if (Registry.IsExiting || Application.isLoadingLevel)
		{
			return;
		}
		if (this.Nid != NID.NoID)
		{
			Registry.DestroyedMapObjects.Add(this.Nid);
		}
		bool flag = false;
		if (this.SyncDestroy)
		{
			flag = true;
		}
		else if (this.Syncronize && PID.MyID == this.Owner)
		{
			flag = true;
		}
		if (flag)
		{
			Networking.RPC<GameObject>(PID.TargetOthers, new RpcSignature<GameObject>(UnityEngine.Object.Destroy), base.gameObject, false);
		}
	}

	public virtual bool ReadyTobeSynced()
	{
		return true;
	}

	private List<State> states = new List<State>();

	private PID owner = PID.TargetServer;

	[HideInInspector]
	public bool SyncDestroy;

	private bool syncronize;

	private bool[] PropsToTryInterpolate;

	private PropertyInfo[] props;

	public bool OnlyDestroyScriptOnSync;

	public static bool ShowInterpolatedPosiontMarkers = true;

	private Transform myTransform;

	private NID nid = NID.NoID;

	[HideInInspector]
	public byte[] instantiationDataBytes;

	private object[] instantiationData;
}
