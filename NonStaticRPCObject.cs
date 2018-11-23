// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Reflection;
using UnityEngine;

public class NonStaticRPCObject : RPCObject
{
	public override void Execute()
	{
		RPCController.CurrentRPC = this;
		if (this.sessionID != Connect.SessionID && this.sessionID != SID.IgnoreID)
		{
			return;
		}
		if (this.sessionID == SID.IgnoreID)
		{
			this.targetID.OverrideSID(Connect.SessionID);
		}
		object obj = null;
		try
		{
			obj = Registry.GetObject(this.targetID);
			if (obj == null)
			{
				return;
			}
			object[] array = TypeSerializer.DeserializeParameterList(this.parametersasBytes, null);
			if (base.CheckForAcknowledgement(array))
			{
				BindingFlags invokeAttr = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod;
				obj.GetType().InvokeMember(this.methodInfo.Name, invokeAttr, Type.DefaultBinder, obj, array);
			}
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogWarning(string.Concat(new object[]
			{
				"Failed to execute: ",
				this.targetID,
				" ",
				this.methodInfo,
				"   obj: ",
				obj
			}));
			UnityEngine.Debug.LogException(exception);
		}
		RPCController.CurrentRPC = null;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"NonStaticRPCObject  [targetID: ",
			this.targetID,
			"]  ",
			base.ToString()
		});
	}

	public NID targetID = NID.NoID;
}
