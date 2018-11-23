// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.IO;
using UnityEngine;

public class LogStream
{
	public LogStream()
	{
		this.Path = Application.dataPath + "/Broforce Logs";
		Directory.CreateDirectory(this.Path);
		string text = string.Concat(new object[]
		{
			"Dumpfile - Ver: ",
			861L,
			" Date: ",
			DateTime.Now,
			" Game: "
		});
		text = text.Replace("/", "-");
		text = text.Replace(":", "-");
		text += ".csv";
		this.Path = this.Path + "\\" + text;
	}

	public void Write(string message, string stackTrace)
	{
		this.writer = new StreamWriter(this.Path, true);
		string text = LogStream.PrepareString(message);
		text = text + ", " + LogStream.PrepareString(stackTrace);
		this.writer.WriteLine(text);
		this.writer.Flush();
		this.writer.Close();
	}

	private static string PrepareString(string str)
	{
		return "\"" + str.Replace("\"", "\"\"") + "\"";
	}

	private StreamWriter writer;

	private string Path;
}
