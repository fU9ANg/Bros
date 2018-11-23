// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
	private void Start()
	{
		this.TMR = base.GetComponent<Renderer>();
		this.TMR.material = this.spriteMaterial;
		for (int i = 0; i < this.animations.Length; i++)
		{
			this.animations[i].material = this.spriteMaterial;
		}
		this.TM = base.GetComponent<TextMesh>();
		this.TM.font = this.animations[this.anim];
	}

	private void animateSprite()
	{
		if (Time.time > this.lastAnimFrameTime + 0.1f)
		{
			this.lastAnimFrameTime = Time.time;
			this.frame += this.frameStep;
			if (this.frame > this.animations[this.anim].characterInfo.Length - 2 || (this.frameStep == -1 && (float)this.frame < this.animations[this.anim].characterInfo[0].uv.y))
			{
				switch (this.animations[this.anim].characterInfo[0].index)
				{
				case 0:
					this.frame = (int)this.animations[this.anim].characterInfo[0].uv.y;
					break;
				case 1:
					this.frameStep *= -1;
					this.frame += this.frameStep * 2;
					break;
				case 2:
					this.frame -= this.frameStep;
					break;
				case 3:
					this.changeAnim((int)this.animations[this.anim].characterInfo[0].uv.x, (int)this.animations[this.anim].characterInfo[0].uv.y);
					break;
				}
			}
			char c = (char)(this.frame + 33);
			this.TM.text = string.Empty + c;
		}
	}

	private void changeAnim(int a, int f)
	{
		this.anim = a;
		this.TM.font = this.animations[this.anim];
		this.frame = f;
		char c = (char)(this.frame + 33);
		this.TM.text = string.Empty + c;
		this.lastAnimFrameTime = Time.time;
		this.frameStep = 1;
	}

	private void Update()
	{
		this.animateSprite();
	}

	private void OnGUI()
	{
		this.anim = GUI.SelectionGrid(new Rect(10f, 10f, 270f, 60f), this.anim, this.animNames, 3);
		if (GUI.changed)
		{
			this.changeAnim(this.anim, 0);
		}
	}

	public Material spriteMaterial;

	public Font[] animations;

	private string[] animNames = new string[]
	{
		"Idle",
		"Run",
		"Stop",
		"Turn",
		"Crouch",
		"UnCrouch",
		"Jump",
		"Land",
		"Fall"
	};

	private float lastAnimFrameTime;

	private int frame;

	private int frameStep = 1;

	private int anim;

	private TextMesh TM;

	private Renderer TMR;

	private enum theLoopBehaviour
	{
		Loop,
		PingPong,
		OnceAndHold,
		OnceAndChange
	}
}
