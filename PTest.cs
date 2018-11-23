// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

internal class PTest : MonoBehaviour
{
	public static void Setup()
	{
		PTest.successes = new List<string>();
		PTest.failures = new List<string>();
		PTest.results = new List<string>();
	}

	public static bool AssertEquals(string section, string name, bool expected, bool received)
	{
		if (expected == received)
		{
			PTest.Record(true, section, name, expected, received);
			return true;
		}
		PTest.Record(false, section, name, expected, received);
		return false;
	}

	public static bool AssertEquals(string section, string name, int expected, int received)
	{
		if (expected == received)
		{
			PTest.Record(true, section, name, expected, received);
			return true;
		}
		PTest.Record(false, section, name, expected, received);
		return false;
	}

	public static bool AssertEquals(string section, string name, string expected, string received)
	{
		if (expected == received)
		{
			PTest.Record(true, section, name, expected, received);
			return true;
		}
		PTest.Record(false, section, name, expected, received);
		return false;
	}

	public static bool AssertTrue(string section, string name, bool value)
	{
		return PTest.AssertEquals(section, name, value, true);
	}

	public static bool AssertFalse(string section, string name, bool value)
	{
		return PTest.AssertEquals(section, name, value, false);
	}

	private static void Record(bool success, string section, string message, object expected, object received)
	{
		string text = "[" + section + "] " + message;
		if (success)
		{
			PTest.successes.Add(text);
		}
		else
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" (",
				expected,
				" vs ",
				received,
				")"
			});
			PTest.failures.Add(text);
		}
		PTest.results.Add(text);
	}

	public static void Render()
	{
		if (PTest.failures.Count > 0)
		{
			UnityEngine.Debug.LogError("[Playtomic.PTest] --------------------------------      errors      --------------------------------");
			foreach (string str in PTest.failures)
			{
				UnityEngine.Debug.LogError("[Playtomic.PTest] " + str);
			}
		}
		if (PTest.failures.Count == 0)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"[Playtomic.PTest] ",
				PTest.successes.Count,
				" tests passed out of ",
				PTest.results.Count,
				" total"
			}));
		}
	}

	protected static List<string> successes;

	protected static List<string> failures;

	protected static List<string> results;
}
