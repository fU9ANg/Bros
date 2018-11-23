// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AnimatedSprite_test : MonoBehaviour
{
	private void Start()
	{
		this.controller = base.GetComponent<CharacterController>();
		this.animate.TMR = base.GetComponent<Renderer>();
		this.animate.TMR.material = this.spriteMaterial;
		for (int i = 0; i < this.animations.Length; i++)
		{
			this.animations[i].material = this.spriteMaterial;
		}
		this.animate.TM = base.GetComponent<TextMesh>();
		this.animate.TM.font = this.animations[this.animate.anim];
	}

	private void animateSprite()
	{
		if (Time.time > this.animate.lastAnimFrameTime + 1f / (this.animate.frameRate * (this.animations[this.animate.anim].characterInfo[0].uv.width + 1f)))
		{
			this.animate.lastAnimFrameTime = Time.time;
			this.animate.frame = this.animate.frame + this.animate.frameStep;
			if (this.animate.frame > this.animations[this.animate.anim].characterInfo.Length - 2 || (this.animate.frameStep == -1 && (float)this.animate.frame < this.animations[this.animate.anim].characterInfo[0].uv.y))
			{
				switch (this.animations[this.animate.anim].characterInfo[0].index)
				{
				case 0:
					this.animate.frame = (int)this.animations[this.animate.anim].characterInfo[0].uv.y;
					break;
				case 1:
					this.animate.frameStep *= -1;
					this.animate.frame = this.animate.frame + this.animate.frameStep * 2;
					break;
				case 2:
					this.animate.frame = this.animate.frame - this.animate.frameStep;
					break;
				case 3:
					this.changeAnim((AnimatedSprite_test.anims)this.animations[this.animate.anim].characterInfo[0].uv.x, (int)this.animations[this.animate.anim].characterInfo[0].uv.y);
					break;
				}
			}
			this.debugstr = string.Concat(new object[]
			{
				"frame:",
				this.animate.frame,
				" in anim:",
				this.animate.anim
			});
			char c = (char)(this.animate.frame + 33);
			this.animate.TM.text = string.Empty + c;
		}
	}

	private void changeAnim(AnimatedSprite_test.anims a, int f)
	{
		this.animate.anim = (int)a;
		this.animate.TM.font = this.animations[this.animate.anim];
		this.animate.frame = f;
		char c = (char)(this.animate.frame + 33);
		this.animate.TM.text = string.Empty + c;
		this.animate.lastAnimFrameTime = Time.time;
		this.animate.frameStep = 1;
		this.debugstr = string.Concat(new object[]
		{
			"frame:",
			this.animate.frame,
			" in anim:",
			this.animate.anim
		});
	}

	private void Update()
	{
		this.buttons.h = Input.GetAxisRaw("Horizontal");
		this.buttons.v = Input.GetAxisRaw("Vertical");
		if ((double)Mathf.Abs(this.buttons.h) > 0.1)
		{
			this.buttons.movePressed = true;
			if (this.movement.currentDirection != new Vector3(this.buttons.h, 0f, 0f) && !this.movement.crouching)
			{
				this.movement.currentDirection = new Vector3(this.buttons.h, 0f, 0f);
				this.buttons.moveChanged = true;
			}
		}
		else
		{
			this.buttons.movePressed = false;
		}
		if ((double)this.buttons.v > 0.1)
		{
			if (!this.movement.jumping && !this.movement.falling && this.buttons.jumpReleased)
			{
				this.buttons.jumpPressed = true;
				this.buttons.crouchPressed = false;
				this.buttons.jumpReleased = false;
			}
		}
		else if ((double)this.buttons.v < -0.1)
		{
			if (this.controller.isGrounded)
			{
				this.buttons.crouchPressed = true;
				this.buttons.jumpPressed = false;
			}
		}
		else
		{
			this.buttons.crouchPressed = false;
			this.buttons.jumpReleased = true;
			this.buttons.jumpPressed = false;
		}
		if (this.buttons.movePressed)
		{
			if (!this.movement.moving && !this.movement.crouching)
			{
				this.movement.moving = true;
				if (!this.movement.jumping && !this.movement.falling)
				{
					this.changeAnim(AnimatedSprite_test.anims.Run, 0);
				}
			}
			if (this.buttons.moveChanged)
			{
				base.transform.localScale = new Vector3((float)((this.buttons.h <= 0f) ? -1 : 1), base.transform.localScale.y, base.transform.localScale.z);
				this.buttons.moveChanged = false;
				if (!this.movement.jumping && !this.movement.falling)
				{
					this.changeAnim(AnimatedSprite_test.anims.Turn, 0);
				}
			}
		}
		else if (this.movement.moving)
		{
			this.movement.moving = false;
			if (!this.movement.jumping && !this.movement.falling)
			{
				this.changeAnim(AnimatedSprite_test.anims.Stop, 0);
			}
		}
		if (this.controller.isGrounded)
		{
			this.movement.timeLastGrounded = Time.time;
			this.movement.currentVerticalSpeed = -0.5f;
			if (this.movement.falling)
			{
				this.movement.falling = false;
				if (this.movement.moving)
				{
					this.changeAnim(AnimatedSprite_test.anims.Run, 15);
				}
				else
				{
					this.changeAnim(AnimatedSprite_test.anims.Land, 0);
				}
			}
			if (this.buttons.jumpPressed)
			{
				if (!this.movement.jumping)
				{
					this.movement.jumping = true;
					this.movement.currentVerticalSpeed = this.movement.jumpPower;
					this.changeAnim(AnimatedSprite_test.anims.Jump, 0);
					this.buttons.jumpPressed = false;
				}
			}
			else if (this.buttons.crouchPressed)
			{
				if (!this.movement.crouching)
				{
					this.movement.crouching = true;
					this.movement.moving = false;
					this.changeAnim(AnimatedSprite_test.anims.Crouch, 0);
				}
			}
			else if (this.movement.crouching)
			{
				this.movement.crouching = false;
				this.changeAnim(AnimatedSprite_test.anims.UnCrouch, 0);
			}
		}
		else if ((double)Time.time > (double)this.movement.timeLastGrounded + 0.15 && !this.movement.jumping && !this.movement.falling)
		{
			this.changeAnim(AnimatedSprite_test.anims.Fall, 0);
			this.movement.falling = true;
			this.movement.timeLastGrounded = Time.time;
		}
		this.movement.currentVerticalSpeed -= this.movement.gravity * Time.deltaTime;
		if (this.movement.currentVerticalSpeed < 1f && this.movement.jumping)
		{
			this.changeAnim(AnimatedSprite_test.anims.Fall, 0);
			this.movement.jumping = false;
			this.movement.falling = true;
		}
		if (this.movement.moving)
		{
			this.movement.targetSpeed = Mathf.Min(Mathf.Abs(this.buttons.h), 1f);
		}
		else
		{
			this.movement.targetSpeed = 0f;
		}
		this.movement.targetSpeed *= this.movement.maxSpeed;
		this.movement.currentSpeed = Mathf.Lerp(this.movement.currentSpeed, this.movement.targetSpeed, 0.1f);
		Vector3 vector = this.movement.currentDirection * this.movement.currentSpeed + new Vector3(0f, this.movement.currentVerticalSpeed, 0f);
		vector *= Time.deltaTime;
		this.movement.collisionFlags = this.controller.Move(vector);
		this.animateSprite();
		if (Input.GetKey(KeyCode.B))
		{
			Time.timeScale = 0.2f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	private void OnGUI()
	{
		GUILayout.Label(this.debugstr, new GUILayoutOption[0]);
		GUI.color = ((!this.movement.moving) ? Color.red : Color.green);
		GUILayout.Label("moving", new GUILayoutOption[0]);
		GUI.color = ((!this.movement.jumping) ? Color.red : Color.green);
		GUILayout.Label("jumping", new GUILayoutOption[0]);
		GUI.color = ((!this.movement.crouching) ? Color.red : Color.green);
		GUILayout.Label("crouching", new GUILayoutOption[0]);
		GUI.color = ((!this.movement.falling) ? Color.red : Color.green);
		GUILayout.Label("falling", new GUILayoutOption[0]);
		GUI.color = ((!this.buttons.movePressed) ? Color.red : Color.green);
		GUILayout.Label("pressing move", new GUILayoutOption[0]);
		GUI.color = ((!this.buttons.jumpPressed) ? Color.red : Color.green);
		GUILayout.Label("pressing jump", new GUILayoutOption[0]);
		GUI.color = ((!this.buttons.crouchPressed) ? Color.red : Color.green);
		GUILayout.Label("pressing crouch", new GUILayoutOption[0]);
		GUI.color = ((!this.controller.isGrounded) ? Color.green : Color.red);
		GUILayout.Label("Airborn", new GUILayoutOption[0]);
		GUI.color = Color.white;
		GUILayout.Label("ground time:" + this.movement.timeLastGrounded, new GUILayoutOption[0]);
		GUILayout.Label("time:" + Time.time, new GUILayoutOption[0]);
		GUILayout.Label("direction:" + this.movement.currentDirection, new GUILayoutOption[0]);
		GUILayout.Label("Input:" + new Vector3(this.buttons.h, this.buttons.v, 0f), new GUILayoutOption[0]);
	}

	public Material spriteMaterial;

	public Font[] animations;

	public AnimatedSprite_test.MovementClass movement;

	public AnimatedSprite_test.AnimtionClass animate;

	public AnimatedSprite_test.InputClass buttons;

	private string debugstr = string.Empty;

	private CharacterController controller;

	private enum anims
	{
		Idle,
		Run,
		Stop,
		Turn,
		Crouch,
		UnCrouch,
		Jump,
		Land,
		Fall
	}

	private enum LoopBehaviour
	{
		Loop,
		PingPong,
		OnceAndHold,
		OnceAndChange
	}

	[Serializable]
	public class AnimtionClass
	{
		[NonSerialized]
		public TextMesh TM;

		[NonSerialized]
		public Renderer TMR;

		[NonSerialized]
		public int anim;

		[NonSerialized]
		public int frame;

		[NonSerialized]
		public int frameStep = 1;

		public float frameRate = 10f;

		[NonSerialized]
		public float lastAnimFrameTime;
	}

	[Serializable]
	public class MovementClass
	{
		public float maxSpeed = 2f;

		public float jumpPower = 8f;

		public float gravity = 9.8f;

		[NonSerialized]
		public float currentSpeed;

		[NonSerialized]
		public float currentVerticalSpeed;

		[NonSerialized]
		public float targetSpeed;

		[NonSerialized]
		public Vector3 currentDirection = Vector3.right;

		[NonSerialized]
		public bool moving;

		[NonSerialized]
		public bool crouching;

		[NonSerialized]
		public bool jumping;

		[NonSerialized]
		public bool falling;

		[NonSerialized]
		public float timeLastGrounded;

		[NonSerialized]
		public CollisionFlags collisionFlags;
	}

	[Serializable]
	public class InputClass
	{
		[NonSerialized]
		public bool jumpPressed;

		[NonSerialized]
		public bool jumpReleased = true;

		[NonSerialized]
		public bool crouchPressed;

		[NonSerialized]
		public bool movePressed;

		[NonSerialized]
		public bool moveChanged;

		[NonSerialized]
		public float v;

		[NonSerialized]
		public float h;
	}
}
