// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
	public virtual void Setup(int pNum, IntermissionScreen controller)
	{
		this.playerNum = pNum;
		this.controller = controller;
	}

	protected virtual void Update()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		this.GetInput();
	}

	protected void GetInput()
	{
		if (this.playerNum >= 0)
		{
			this.wasUp = this.up;
			this.wasButtonJump = this.buttonJump;
			this.wasDown = this.down;
			this.wasLeft = this.left;
			this.wasRight = this.right;
			this.wasFire = this.fire;
			this.wasSpecial = this.special;
			this.wasHighFive = this.highFive;
			this.wasButtonTaunt = this.buttonTaunt;
			if (HeroController.IsPlaying(this.playerNum))
			{
				HeroController.players[this.playerNum].GetInput(ref this.up, ref this.down, ref this.left, ref this.right, ref this.fire, ref this.buttonJump, ref this.special, ref this.highFive);
			}
			else if (this.playerNum < 2 && !HeroController.playerControllerIDs.Contains(this.playerNum))
			{
				InputReader.GetInput(this.playerNum, ref this.up, ref this.down, ref this.left, ref this.right, ref this.fire, ref this.buttonJump, ref this.special, ref this.highFive);
			}
			if (Application.isEditor && this.playerNum > 1)
			{
				if (this.playerNum == 2)
				{
					if (Input.GetKeyDown(KeyCode.N))
					{
						this.fire = true;
					}
					else
					{
						this.fire = false;
					}
				}
				if (this.playerNum == 3)
				{
					if (Input.GetKeyDown(KeyCode.M))
					{
						this.fire = true;
					}
					else
					{
						this.fire = false;
					}
				}
			}
		}
		else
		{
			UnityEngine.Debug.LogError("No Player Num. playerNum = " + this.playerNum);
		}
	}

	public int playerNum = -1;

	protected bool buttonJump;

	protected bool buttonHighFive;

	protected bool buttonTaunt;

	protected bool up;

	protected bool down;

	protected bool left;

	protected bool right;

	protected bool wasButtonJump;

	protected bool wasButtonHighFive;

	protected bool wasButtonTaunt;

	protected bool wasUp;

	protected bool wasDown;

	protected bool wasLeft;

	protected bool wasRight;

	protected bool special;

	protected bool wasSpecial;

	protected bool fire;

	protected bool wasFire;

	protected bool highFive;

	protected bool wasHighFive;

	protected IntermissionScreen controller;

	protected float t = 0.011f;
}
