// dnSpy decompiler from Assembly-CSharp.dll
using System;

public class Singleton<T> where T : class, new()
{
	public static T Instance
	{
		get
		{
			if (Singleton<T>.instance == null)
			{
				Singleton<T>.instance = Activator.CreateInstance<T>();
			}
			return Singleton<T>.instance;
		}
	}

	private static T instance;
}
