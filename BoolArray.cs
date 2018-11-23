// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

[Serializable]
public class BoolArray
{
	public BoolArray()
	{
		this.array = new bool[0];
	}

	public BoolArray(int size)
	{
		this.array = new bool[size];
	}

	public BoolArray(bool[] initialValues)
	{
		this.array = new bool[initialValues.Length];
		Array.Copy(initialValues, this.array, initialValues.Length);
	}

	public bool this[int key]
	{
		get
		{
			return this.array[key];
		}
		set
		{
			this.array[key] = value;
		}
	}

	public int Length
	{
		get
		{
			return this.array.Length;
		}
	}

	public void Resize(int size)
	{
		Array.Resize<bool>(ref this.array, size);
	}

	[SerializeField]
	public bool[] array;
}
