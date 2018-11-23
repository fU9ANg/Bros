// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.IO;
using UnityEngine;

public class UnityStream
{
	public UnityStream()
	{
		this.isWriting = true;
		this.stream = new MemoryStream();
		this.reader = new BinaryReader(this.stream);
		this.writer = new BinaryWriter(this.stream);
	}

	public UnityStream(byte[] ByteArray)
	{
		this.isWriting = false;
		this.stream = new MemoryStream(ByteArray);
		this.reader = new BinaryReader(this.stream);
		this.writer = new BinaryWriter(this.stream);
	}

	public bool IsWriting
	{
		get
		{
			return this.isWriting;
		}
	}

	public byte[] ByteArray
	{
		get
		{
			return this.stream.ToArray();
		}
	}

	public bool Finished
	{
		get
		{
			return this.reader.BaseStream.Position == this.reader.BaseStream.Length;
		}
	}

	public void Serialize<T>(T obj)
	{
		if (TypeSerializer.DebugMode)
		{
			UnityEngine.Debug.Log("== " + obj.GetType());
		}
		if (this.isWriting)
		{
			this.writer = TypeSerializer.Serialize<T>(obj, this.writer);
		}
		else
		{
			UnityEngine.Debug.LogWarning("This stream is write only");
		}
	}

	public object DeserializeNext()
	{
		if (this.isWriting)
		{
			UnityEngine.Debug.LogWarning("This stream is read only");
			return null;
		}
		if (this.reader.PeekChar() == -1)
		{
			return null;
		}
		return TypeSerializer.Deserialize(this.reader);
	}

	public static void PrintBytes(byte[] bytes)
	{
		string text = string.Empty;
		for (int i = 0; i < bytes.Length; i++)
		{
			text += bytes[i];
		}
		UnityEngine.Debug.Log(text);
	}

	private MemoryStream stream;

	private BinaryReader reader;

	private BinaryWriter writer;

	private bool isWriting;
}
