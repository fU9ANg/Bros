// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MinibossParachuteCall : MonoBehaviour
{
	private void Start()
	{
		this.mookopterAI = base.GetComponent<MookopterPolyAI>();
		this.mookopter = base.GetComponent<Unit>();
	}

	private void Update()
	{
		if (this.mookopterAI.IsMine && this.mookopter.health > 0 && this.mookopterAI.mentalState == MentalState.Alerted)
		{
			this.counter += Time.deltaTime;
			if (this.lastPattern != this.mookopterAI.kopterPattern)
			{
				if (this.counter > 4f)
				{
					if (this.counter < 24f)
					{
						if (this.mookopterAI.kopterPattern == KopterPattern.MovingLeft)
						{
							this.CallDownMooksMinor(this.mookopter.x - 24f, this.mookopter.y + 200f);
						}
						if (this.mookopterAI.kopterPattern == KopterPattern.MovingRight)
						{
							this.CallDownMooksMinor(this.mookopter.x + 24f, this.mookopter.y + 200f);
						}
					}
					else
					{
						if (this.mookopterAI.kopterPattern == KopterPattern.MovingLeft)
						{
							this.CallDownMooksMajor(this.mookopter.x - 24f, this.mookopter.y + 200f);
						}
						if (this.mookopterAI.kopterPattern == KopterPattern.MovingRight)
						{
							this.CallDownMooksMajor(this.mookopter.x + 24f, this.mookopter.y + 200f);
						}
					}
				}
				this.lastPattern = this.mookopterAI.kopterPattern;
			}
		}
	}

	protected void CallDownMooksMinor(float x, float y)
	{
		for (int i = 0; i < 2; i++)
		{
			UnityEngine.Object @object = Resources.Load("Mooks/ZMook");
			Mook component = (@object as GameObject).GetComponent<Mook>();
			this.SpawnMook(component, x + -16f + (float)(32 * i), y + 24f * UnityEngine.Random.value);
		}
	}

	protected void CallDownMooksMajor(float x, float y)
	{
		for (int i = 0; i < 2; i++)
		{
			UnityEngine.Object @object = Resources.Load("Mooks/ZMook");
			Mook component = (@object as GameObject).GetComponent<Mook>();
			this.SpawnMook(component, x + -32f + (float)(64 * i), y + 24f * UnityEngine.Random.value);
		}
		for (int j = 0; j < 2; j++)
		{
			UnityEngine.Object object2 = Resources.Load("Mooks/ZMookSuicide");
			Mook component2 = (object2 as GameObject).GetComponent<Mook>();
			this.SpawnMook(component2, x + -16f + (float)(32 * j), y + 24f * UnityEngine.Random.value);
		}
	}

	protected void SpawnMook(Mook prefab, float x, float y)
	{
		if (prefab != null)
		{
			MapController.SpawnMook_Networked(prefab, x, y, 0f, 0f, false, false, true, false, false);
		}
		else
		{
			UnityEngine.Debug.LogError("Could not instantiate resource");
		}
	}

	protected MookopterPolyAI mookopterAI;

	protected Unit mookopter;

	protected KopterPattern lastPattern = KopterPattern.Restart;

	protected float counter;
}
