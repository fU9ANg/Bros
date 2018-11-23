// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.Reflection;

public class State
{
	public State(NID _Key, NetworkObject netObj)
	{
		this.key = _Key;
		this.timeStamp = Connect.Timef;
		PropertyInfo[] props = netObj.Props;
		List<Type> list = new List<Type>();
		foreach (PropertyInfo propertyInfo in props)
		{
			list.Add(propertyInfo.PropertyType);
		}
		this.values = SyncController.GetPropertyValues(props, netObj);
		this.types = list.ToArray();
	}

	public State(NID _Key, float _timeStamp, object[] _values)
	{
		this.key = _Key;
		this.timeStamp = _timeStamp;
		this.values = _values;
	}

	public NID Key
	{
		get
		{
			return this.key;
		}
	}

	public float TimeStamp
	{
		get
		{
			return this.timeStamp;
		}
	}

	public object[] Values
	{
		get
		{
			return this.values;
		}
	}

	public Type[] Types
	{
		get
		{
			return this.types;
		}
	}

	private NID key = NID.NoID;

	private float timeStamp;

	private object[] values;

	private Type[] types;
}
