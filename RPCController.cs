// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public class RPCController : MonoBehaviour
{
	public static PID LastSender
	{
		get
		{
			if (RPCController.CurrentRPC != null)
			{
				return RPCController.CurrentRPC.messageInfo.Sender;
			}
			return null;
		}
	}

	public static double LastTimeStamp
	{
		get
		{
			if (RPCController.CurrentRPC != null)
			{
				return RPCController.CurrentRPC.messageInfo.TimeStamp;
			}
			return 0.0;
		}
	}

	public void Reset()
	{
		RPCController.RPCsToProcess.Clear();
		RPCController.RPCsToDispatch.Clear();
		RPCController.delayedRPCs.Clear();
		RPCController.rpcsAwaitingExecution.Clear();
	}

	public static RPCController Instance
	{
		get
		{
			if (RPCController.instance == null)
			{
				RPCController.instance = (UnityEngine.Object.FindObjectOfType(typeof(RPCController)) as RPCController);
			}
			return RPCController.instance;
		}
	}

	public static Ack ExtractAcknowledgement(params object[] parameters)
	{
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i] is Ack)
			{
				return parameters[i] as Ack;
			}
		}
		return null;
	}

	public static RPCObject CreateRPC(PID targetID, object targetObject, MethodInfo methodInfo, bool addExecutionDelay, bool immediate, bool ignoreSessionID, params object[] parameters)
	{
		List<Type> list = new List<Type>();
		foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
		{
			list.Add(parameterInfo.ParameterType);
		}
		byte[] parametersasBytes = TypeSerializer.SerializeParameterList(list.ToArray(), parameters);
		double time = Connect.Time;
		RPCObject rpcobject;
		if (methodInfo.IsStatic)
		{
			rpcobject = new StaticRPCObject();
		}
		else
		{
			rpcobject = new NonStaticRPCObject();
			((NonStaticRPCObject)rpcobject).targetID = Registry.GetNID((UnityEngine.Object)targetObject);
		}
		rpcobject.methodInfo = methodInfo;
		rpcobject.executeImmediately = immediate;
		rpcobject.messageInfo = new MessageInfo(targetID, time);
		rpcobject.parametersasBytes = parametersasBytes;
		if (ignoreSessionID)
		{
			rpcobject.sessionID = SID.IgnoreID;
		}
		else
		{
			rpcobject.sessionID = Connect.SessionID;
		}
		if (addExecutionDelay)
		{
			rpcobject.messageInfo.TimeStamp += 0.10000000149011612;
		}
		return rpcobject;
	}

	public static void SendRPC(PID targetID, object targetObject, MethodInfo methodInfo, bool addExecutionDelay, bool immediate, bool useReliableStream, bool ignoreSessionID, params object[] parameters)
	{
		RPCObject rpcobject = RPCController.CreateRPC(targetID, targetObject, methodInfo, addExecutionDelay, immediate, ignoreSessionID, parameters);
		Ack ack = RPCController.ExtractAcknowledgement(parameters);
		if (ack != null && !ack.IsResponse)
		{
			if (!Ack.RPCsAwaitingReplies.ContainsKey(ack.AckID))
			{
				Ack.RPCsAwaitingReplies.Add(ack.AckID, rpcobject);
			}
			else
			{
				MonoBehaviour.print(string.Concat(new object[]
				{
					"Ack already exists",
					ack.AckID,
					" ",
					rpcobject.methodInfo
				}));
			}
		}
		RPCController.Instance.RouteRPCObject(rpcobject, useReliableStream);
	}

	public void RouteRPCObject(RPCObject rpcObject, bool useReliableStream)
	{
		bool flag = false;
		bool flag2 = false;
		if (rpcObject == null)
		{
			return;
		}
		if (rpcObject.messageInfo.Destination == PID.TargetServer)
		{
			if (PID.MyID == PID.ServerID)
			{
				flag = true;
			}
			else
			{
				flag2 = true;
			}
		}
		else if (rpcObject.messageInfo.Destination == PID.TargetAll)
		{
			flag = true;
			flag2 = true;
		}
		else if (rpcObject.messageInfo.Destination == PID.TargetOthers)
		{
			flag2 = true;
		}
		else if (rpcObject.messageInfo.Destination == PID.MyID)
		{
			flag = true;
		}
		else
		{
			flag2 = true;
		}
		if (flag)
		{
			if (rpcObject.messageInfo.Sender == PID.MyID)
			{
				rpcObject.Execute();
			}
			else
			{
				RPCController.rpcsAwaitingExecution.Add(rpcObject);
			}
		}
		if (flag2)
		{
			this.SendRPCObject(rpcObject);
		}
	}

	private void SendRPCObject(RPCObject rpcObject)
	{
		if (RPCController.DebugMode)
		{
			MonoBehaviour.print("About to send " + rpcObject.methodInfo);
		}
		byte[] array = this.SerializeRPCObject(rpcObject);
		RPCBatcher.Send(rpcObject);
	}

	private byte[] SerializeRPCObject(RPCObject rpcObject)
	{
		UnityStream unityStream = new UnityStream();
		if (rpcObject is NonStaticRPCObject)
		{
			unityStream.Serialize<NonStaticRPCObject>((NonStaticRPCObject)rpcObject);
		}
		else
		{
			unityStream.Serialize<StaticRPCObject>((StaticRPCObject)rpcObject);
		}
		return unityStream.ByteArray;
	}

	private void RecieveRPCObject(RPCObject rpcObject)
	{
		Analytics.AddRpcIn(rpcObject);
		RPCController.DelayedRPC delayedRPC = new RPCController.DelayedRPC();
		delayedRPC.rpcObject = rpcObject;
		float num = Mathf.Abs((float)(rpcObject.messageInfo.TimeStamp - Connect.Time));
		if (num > 25f && this.timeDifferenceWarningTimer <= 0)
		{
			this.timeDifferenceWarningTimer = 30;
			UnityEngine.Debug.Log(rpcObject.methodInfo + " Rpc timestamp difference is over 25 seconds! " + num);
		}
		this.timeDifferenceWarningTimer--;
		if (RPCController.DebugMode)
		{
			MonoBehaviour.print("Recieved " + rpcObject.methodInfo);
		}
		RPCController.delayedRPCs.Add(delayedRPC);
	}

	private void Update()
	{
		for (int i = 0; i < 100; i++)
		{
			RPCObject rpcobject = ConnectionLayer.RecieveNext();
			if (rpcobject == null)
			{
				break;
			}
			this.RecieveRPCObject(rpcobject);
		}
		this.ProcessSimulatedlatency();
		this.ProcessRPCQueue();
	}

	private void ProcessSimulatedlatency()
	{
		for (int i = 0; i < RPCController.delayedRPCs.Count; i++)
		{
			RPCController.delayedRPCs[i].delayTime -= (double)NetworkTimeSync.RealDeltaTime;
		}
		while (RPCController.delayedRPCs.Count > 0)
		{
			if (RPCController.DebugMode)
			{
				MonoBehaviour.print(string.Concat(new object[]
				{
					"delayedRPCs ",
					RPCController.delayedRPCs[0].rpcObject.methodInfo,
					"   ",
					RPCController.delayedRPCs[0].delayTime
				}));
			}
			if (RPCController.delayedRPCs[0].delayTime > 0.0)
			{
				break;
			}
			RPCController.rpcsAwaitingExecution.Add(RPCController.delayedRPCs[0].rpcObject);
			RPCController.delayedRPCs.RemoveAt(0);
		}
	}

	private void ProcessRPCQueue()
	{
		List<RPCObject> list = new List<RPCObject>();
		for (int i = 0; i < RPCController.rpcsAwaitingExecution.Count; i++)
		{
			if (Networking.StreamIsPaused)
			{
				break;
			}
			RPCObject rpcobject = RPCController.rpcsAwaitingExecution[i];
			if (rpcobject.executeImmediately)
			{
				rpcobject.Execute();
				list.Add(rpcobject);
			}
			else if (rpcobject.messageInfo.TimeStamp <= Connect.GetInterpTime(rpcobject.messageInfo.Sender))
			{
				rpcobject.Execute();
				list.Add(rpcobject);
			}
		}
		foreach (RPCObject item in list)
		{
			RPCController.rpcsAwaitingExecution.Remove(item);
		}
	}

	private void CheckForNullReferences()
	{
		List<KeyValuePair<NID, object>> list = new List<KeyValuePair<NID, object>>();
		foreach (KeyValuePair<NID, object> item in Registry.Components)
		{
			if (item.Value == null)
			{
				list.Add(item);
			}
		}
		foreach (KeyValuePair<NID, object> keyValuePair in list)
		{
			Registry.Components.Remove(keyValuePair.Key);
			Registry.Keys.Remove(keyValuePair.Value);
		}
	}

	public void InvokeNonStaticMethod(object target, string methodName, params object[] parameters)
	{
	}

	public void InvokeStaticMethod(Type type, string methodName, params object[] parameters)
	{
	}

	private void Awake()
	{
		RPCController.SetStaticVariabe<int>(() => this.aa);
	}

	private static void SetStaticVariabe<T>(Expression<Func<T>> expr)
	{
		MemberExpression memberExpression = (MemberExpression)expr.Body;
		MonoBehaviour.print(memberExpression.Member.Name);
		MonoBehaviour.print(memberExpression.Member.DeclaringType);
		object value = ((ConstantExpression)memberExpression.Expression).Value;
		FieldInfo fieldInfo = (FieldInfo)memberExpression.Member;
		MonoBehaviour.print("Value is: " + fieldInfo);
		MonoBehaviour.print("Value is: " + fieldInfo.GetValue(value));
	}

	private static PID TranslateMode(RPCMode mode)
	{
		switch (mode)
		{
		case RPCMode.Server:
			return PID.TargetServer;
		case RPCMode.Others:
			return PID.TargetOthers;
		case RPCMode.All:
			return PID.TargetAll;
		case RPCMode.OthersBuffered:
			return PID.TargetOthers;
		case RPCMode.AllBuffered:
			return PID.TargetAll;
		}
		return PID.TargetAll;
	}

	public static bool MustExecute(PID target)
	{
		return target == PID.MyID || target == PID.TargetAll || target == PID.ServerID || target == PID.TargetServer;
	}

	public static string CurrentRPCName = string.Empty;

	public static RPCObject CurrentRPC = null;

	public static List<byte[]> RPCsToProcess = new List<byte[]>();

	public static List<byte[]> RPCsToDispatch = new List<byte[]>();

	private static List<RPCController.DelayedRPC> delayedRPCs = new List<RPCController.DelayedRPC>();

	private static List<RPCObject> rpcsAwaitingExecution = new List<RPCObject>();

	public static bool DebugMode = false;

	private static RPCController instance;

	private int timeDifferenceWarningTimer;

	private int aa = 4;

	private class DelayedRPC
	{
		public RPCObject rpcObject;

		public double delayTime;
	}
}
