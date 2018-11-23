// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

internal struct NullWrapper
{
	public NullWrapper(Type _type)
	{
		this.type = _type;
		if (this.type == null)
		{
			UnityEngine.Debug.LogError("type cannot be null! will cause recursion");
		}
	}

	public Type type;
}
