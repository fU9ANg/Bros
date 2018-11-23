// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogToFile
{
	public static void RefreshGamePath()
	{
		LogToFile.Path = Application.dataPath + "/Broforce Logs";
		Directory.CreateDirectory(LogToFile.Path);
		string text = string.Concat(new object[]
		{
			"Ver: ",
			861L,
			" Date: ",
			DateTime.Now,
			" Game: "
		});
		text = text.Replace("/", "-");
		text = text.Replace(":", "-");
		text += ".csv";
		LogToFile.Path = LogToFile.Path + "\\" + text;
	}

	public static void SetToDefaultPath()
	{
		LogToFile.Path = Application.dataPath + "/Broforce Logs";
		Directory.CreateDirectory(LogToFile.Path);
		string text = string.Concat(new object[]
		{
			"Ver: ",
			861L,
			" Date: ",
			DateTime.Now,
			" Default"
		});
		text = text.Replace("/", "-");
		text = text.Replace(":", "-");
		text += ".csv";
		LogToFile.Path = LogToFile.Path + "\\" + text;
	}

	public static StreamWriter GetStream()
	{
		if (LogToFile.Path == string.Empty)
		{
			LogToFile.RefreshGamePath();
		}
		return new StreamWriter(LogToFile.Path);
	}

	public static void AppendConnectionLogToFile()
	{
		try
		{
			StreamWriter stream = LogToFile.GetStream();
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			Analytics.PrintKbpsLog();
			stream.WriteLine("\"Connection Log\"");
			foreach (string str in NetworkLog.messages)
			{
				string value = LogToFile.PrepareString(str);
				stream.WriteLine(value);
			}
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.WriteLine(string.Empty);
			stream.Close();
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	public static void WriteConnectionLogToFile()
	{
		try
		{
			StreamWriter stream = LogToFile.GetStream();
			stream.WriteLine(string.Empty);
			Analytics.PrintKbpsLog();
			stream.WriteLine("\"Connection Log\"");
			foreach (string str in NetworkLog.messages)
			{
				string value = LogToFile.PrepareString(str);
				stream.WriteLine(value);
			}
			stream.Close();
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	public static void AppendMessagesToFile(List<ScreenDebug.Message> messages)
	{
		UnityEngine.Debug.Log("Writing to path " + LogToFile.Path);
		try
		{
			StreamWriter stream = LogToFile.GetStream();
			foreach (ScreenDebug.Message message in messages)
			{
				string text = LogToFile.PrepareString(message.text);
				if (message.text.ToLower().Contains("exception"))
				{
					text = text + ", " + LogToFile.PrepareString(message.stackTrace);
				}
				stream.WriteLine(text);
			}
			stream.Close();
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private static string PrepareString(string str)
	{
		return "\"" + str.Replace("\"", "\"\"") + "\"";
	}

	private static string Path = string.Empty;
}
