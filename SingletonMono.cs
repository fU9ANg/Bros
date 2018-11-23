// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : Component
{
	public static T Instance
	{
		get
		{
			if (SingletonMono<T>.instance == null)
			{
				SingletonMono<T>.instance = (UnityEngine.Object.FindObjectOfType(typeof(T)) as T);
			}
			return SingletonMono<T>.instance;
		}
	}

	public static void DestroyDuplicates()
	{
		SingletonMono<T>[] array = UnityEngine.Object.FindObjectsOfType(typeof(T)) as SingletonMono<T>[];
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (array[i] != SingletonMono<T>.Instance)
			{
				UnityEngine.Object.DestroyImmediate(array[i].gameObject);
			}
		}
	}

	private static T instance;
}
