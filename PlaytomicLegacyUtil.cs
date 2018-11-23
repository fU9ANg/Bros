// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class PlaytomicLegacyUtil
{
	public static Dictionary<K, V> HashtableToDictionary<K, V>(Hashtable table)
	{
		return table.Cast<DictionaryEntry>().ToDictionary((DictionaryEntry kvp) => (K)((object)kvp.Key), (DictionaryEntry kvp) => (V)((object)kvp.Value));
	}
}
