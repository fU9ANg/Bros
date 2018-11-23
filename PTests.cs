// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class PTests : MonoBehaviour
{
	public void Start()
	{
		Playtomic.Initialize("broforce", "IndieDarlings5001", this.testURL);
		PTest.Setup();
		PTestLeaderboards.rnd = (PTestPlayerLevels.rnd = (PTestAchievements.rnd = PTests.RND()));
		this._tests = new List<Action<Action>>
		{
			new Action<Action>(PTestPlayerLevels.Create),
			new Action<Action>(PTestPlayerLevels.List),
			new Action<Action>(PTestPlayerLevels.Load),
			new Action<Action>(PTestPlayerLevels.Rate)
		};
		this.Next();
	}

	private void Next()
	{
		if (this._tests.Count == 0)
		{
			PTest.Render();
			return;
		}
		Action<Action> action = this._tests[0];
		this._tests.RemoveAt(0);
		action(new Action(this.Next));
	}

	private static int RND()
	{
		System.Random random = new System.Random();
		return random.Next(int.MaxValue);
	}

	public string testURL = "http://broforcebeta.herokuapp.com/";

	private List<Action<Action>> _tests;
}
