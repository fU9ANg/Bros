// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

internal class PTestGameVars : PTest
{
	public static void All(Action done)
	{
		UnityEngine.Debug.Log("PTestGameVars.All");
		Playtomic.GameVars.Load(delegate(Dictionary<string, GameVar> gv, PResponse r)
		{
			gv = (gv ?? new Dictionary<string, GameVar>());
			PTest.AssertTrue("PTestGameVars.All", "Request succeeded", r.success);
			PTest.AssertEquals("PTestGameVars.All", "No errorcode", r.errorcode, 0);
			PTest.AssertTrue("PTestGameVars.All", "Has known testvar1", gv.ContainsKey("testvar1"));
			PTest.AssertTrue("PTestGameVars.All", "Has known testvar2", gv.ContainsKey("testvar2"));
			PTest.AssertTrue("PTestGameVars.All", "Has known testvar3", gv.ContainsKey("testvar3"));
			PTest.AssertEquals("PTestGameVars.All", "Has known testvar1 value", "testvalue1", gv["testvar1"].value);
			PTest.AssertEquals("PTestGameVars.All", "Has known testvar2 value", "testvalue2", gv["testvar2"].value);
			PTest.AssertEquals("PTestGameVars.All", "Has known testvar3 value", "testvalue3 and the final gamevar", gv["testvar3"].value);
			done();
		});
	}

	public static void Single(Action done)
	{
		UnityEngine.Debug.Log("PTestGameVars.LoadSingle");
		Playtomic.GameVars.Load("testvar1", delegate(GameVar gv, PResponse r)
		{
			gv = (gv ?? new GameVar());
			PTest.AssertTrue("PTestGameVars.LoadSingle", "Request succeeded", r.success);
			PTest.AssertEquals("PTestGameVars.LoadSingle", "No errorcode", r.errorcode, 0);
			PTest.AssertEquals("PTestGameVars.LoadSingle", "Has known testvar1 name", "testvar1", gv["name"].ToString());
			PTest.AssertEquals("PTestGameVars.LoadSingle", "Has known testvalue1 value", "testvalue1", gv["value"].ToString());
			PTest.AssertFalse("PTestGameVars.LoadSingle", "Does not have testvar2", gv.ContainsKey("testvar2"));
			PTest.AssertFalse("PTestGameVars.LoadSingle", "Does not have testvar3", gv.ContainsKey("testvar3"));
			done();
		});
	}
}
