// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Reflection;
using UnityEngine;

public class RPCObject
{
	public bool IsReady()
	{
		return Connect.GetInterpTime(this.messageInfo.Sender) >= this.messageInfo.TimeStamp;
	}

	public virtual void Execute()
	{
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			base.ToString(),
			" ",
			this.methodInfo,
			" [messageInfo: ",
			this.messageInfo,
			"] "
		});
	}

	protected bool CheckForAcknowledgement(object[] parameters)
	{
		Ack ack = RPCController.ExtractAcknowledgement(parameters);
		if (ack != null)
		{
			if (ack.IsResponse)
			{
				if (!Ack.RPCsAwaitingReplies.ContainsKey(ack.AckID))
				{
					UnityEngine.Debug.Log(string.Concat(new object[]
					{
						"Host has already replied ",
						ack.AckID,
						" ",
						this.methodInfo
					}));
					return false;
				}
				Ack.RPCsAwaitingReplies.Remove(ack.AckID);
			}
			else
			{
				ack.IsResponse = true;
			}
		}
		return true;
	}

	public MethodInfo methodInfo;

	public byte[] parametersasBytes;

	public MessageInfo messageInfo;

	public SID sessionID = new SID(byte.MaxValue);

	public bool executeImmediately;
}
