// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Reflection;
using UnityEngine;

public class StaticRPCObject : RPCObject
{
	public override void Execute()
	{
		RPCController.CurrentRPC = this;
		if (this.sessionID != Connect.SessionID && this.sessionID != SID.IgnoreID)
		{
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
			{
				"Recieved Rpc [",
				this.methodInfo,
				" ",
				this.methodInfo.DeclaringType,
				"] from another session: ",
				this.sessionID,
				". Current SID ",
				Connect.SessionID,
				". RPC will not executed"
			}));
			return;
		}
		try
		{
			object[] array = TypeSerializer.DeserializeParameterList(this.parametersasBytes, null);
			if (!base.CheckForAcknowledgement(array))
			{
				return;
			}
			BindingFlags invokeAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod;
			this.methodInfo.DeclaringType.InvokeMember(this.methodInfo.Name, invokeAttr, Type.DefaultBinder, null, array);
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning("Failed to execute: " + this.methodInfo);
			UnityEngine.Debug.Log(message);
		}
		RPCController.CurrentRPC = null;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"NonStaticRPCObject  [Type: ",
			this.methodInfo.DeclaringType,
			"]  ",
			base.ToString()
		});
	}
}
