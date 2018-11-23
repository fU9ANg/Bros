// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class CongaBrain : MonoBehaviour
{
	private void Awake()
	{
		this.unit = base.GetComponent<Unit>();
	}

	public void GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool buttonJump, ref bool fire)
	{
		if (this.unit.health <= 0)
		{
			return;
		}
		this.TrackInputStateChanges();
		this.CopyInputStateChanges(this.inputChanges);
		this.CopyInputStateChanges(this.fireInputChanges);
		fire = this.trackedInputState.fire;
		if (this.followMode == BroFollowMode.CopyCat)
		{
			this.GetCopyCatInput(ref left, ref right, ref up, ref down, ref buttonJump, ref fire);
		}
		else if (this.followMode == BroFollowMode.Calibrate)
		{
			this.GetCalibrateInput(ref left, ref right, ref up, ref down, ref buttonJump, ref fire);
		}
	}

	protected void TrackInputStateChanges()
	{
		this.t = Mathf.Clamp(Time.deltaTime, 0f, 0.0333334f);
		if (this.leadingBro == null || this.leadingBro.isOnHelicopter)
		{
			return;
		}
		if ((this.leadingBro.up || this.leadingBro.down || this.leadingBro.left || this.leadingBro.right || this.leadingBro.buttonJump) && (this.arbitraryTrackingDelay -= this.t) < 0f)
		{
			this.arbitraryTrackingDelay = 0.02f;
			this.inputChanges.Add(new InputStateChange
			{
				newState = this.leadingBro.up,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.up && !this.leadingBro.wasUp)
		{
			this.inputChanges.Add(new InputStateChange
			{
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.down && !this.leadingBro.wasDown)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.down,
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.left && !this.leadingBro.wasLeft)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.left,
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.right && !this.leadingBro.wasRight)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.right,
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.buttonJump && !this.leadingBro.wasButtonJump)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.jump,
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.fire && !this.leadingBro.wasFire)
		{
			this.fireInputChanges.Add(new InputStateChange
			{
				key = InputKey.fire,
				newState = true,
				timeLeft = 0.2f + 0.1f * (float)this.position * UnityEngine.Random.value,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (this.leadingBro.dashing && !this.leadBroWasDashing)
		{
			this.fireInputChanges.Add(new InputStateChange
			{
				key = InputKey.dash,
				newState = true,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
			this.leadBroWasDashing = true;
		}
		if (!this.leadingBro.up && this.leadingBro.wasUp)
		{
			this.inputChanges.Add(new InputStateChange
			{
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.down && this.leadingBro.wasDown)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.down,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.left && this.leadingBro.wasLeft)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.left,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.right && this.leadingBro.wasRight)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.right,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.buttonJump && this.leadingBro.wasButtonJump)
		{
			this.inputChanges.Add(new InputStateChange
			{
				key = InputKey.jump,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.fire && this.leadingBro.wasFire)
		{
			this.fireInputChanges.Add(new InputStateChange
			{
				key = InputKey.fire,
				timeLeft = 0.2f + 0.1f * (float)this.position * UnityEngine.Random.value,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
		}
		if (!this.leadingBro.dashing && this.leadBroWasDashing)
		{
			this.fireInputChanges.Add(new InputStateChange
			{
				key = InputKey.dash,
				timeLeft = this.inputDelay,
				pos = this.leadingBro.transform.position,
				xI = this.leadingBro.xI,
				yI = this.leadingBro.yI
			});
			this.leadBroWasDashing = false;
		}
	}

	private void CopyInputStateChanges(List<InputStateChange> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i].timeLeft -= this.t;
		}
		if (list.Count > 0 && this.followMode != BroFollowMode.Calibrate)
		{
			while (list.Count > 0 && list[0].timeLeft <= 0f)
			{
				switch (list[0].key)
				{
				case InputKey.up:
					this.trackedInputState.up = list[0].newState;
					break;
				case InputKey.down:
					this.trackedInputState.down = list[0].newState;
					break;
				case InputKey.left:
					this.trackedInputState.left = list[0].newState;
					break;
				case InputKey.right:
					this.trackedInputState.right = list[0].newState;
					break;
				case InputKey.jump:
					this.trackedInputState.jump = list[0].newState;
					break;
				case InputKey.fire:
					this.trackedInputState.fire = list[0].newState;
					break;
				case InputKey.dash:
					this.trackedInputState.dashing = list[0].newState;
					break;
				}
				if (list[0].key != InputKey.fire)
				{
					if (this.followMode != BroFollowMode.Calibrate)
					{
						if (Vector3.SqrMagnitude(base.transform.position - list[0].pos) > this.copyCatDistanceSquared)
						{
							this.DoCatchup();
						}
						else if (this.followMode != BroFollowMode.CopyCat || this.unit.actionState != ActionState.Jumping)
						{
						}
						list.RemoveAt(0);
						this.repositionDelay = 0.2f;
					}
				}
				else
				{
					list.RemoveAt(0);
				}
			}
		}
	}

	private void DoCatchup()
	{
		if (this.inputChanges.Count > 0)
		{
			this.unit.x = (this.inputChanges[0].pos.x + this.unit.x) / 2f;
			this.unit.y = (this.inputChanges[0].pos.y + this.unit.y) / 2f;
			this.unit.xI = this.inputChanges[0].xI;
			this.unit.yI = this.inputChanges[0].yI;
			float timeLeft = this.inputChanges[0].timeLeft;
			this.inputChanges[0].timeLeft = 0f;
			for (int i = 1; i < this.inputChanges.Count; i++)
			{
				this.inputChanges[i].timeLeft -= timeLeft;
			}
		}
	}

	protected void GetCalibrateInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool buttonJump, ref bool fire)
	{
		if (this.inputChanges.Count > 0)
		{
			Vector3 pos = this.inputChanges[0].pos;
			if (Vector3.Distance(base.transform.position, pos) > 4f)
			{
				this.calibrateTimer = 0f;
			}
			if ((this.calibrateTimer -= this.t) > 0f)
			{
				return;
			}
			this.repositionTimeLeft = 0.5f;
			if (this.inputChanges[0].pos.x - base.transform.position.x < 1f)
			{
				left = true;
			}
			else if (this.inputChanges[0].pos.x - base.transform.position.x > -1f)
			{
				right = true;
			}
			if (this.inputChanges[0].pos.y - base.transform.position.y < 1f)
			{
				down = true;
			}
			else if (this.inputChanges[0].pos.y - base.transform.position.y > -1f)
			{
				buttonJump = true;
				up = true;
			}
			if (Mathf.Abs(base.transform.position.x - pos.x) < 2f || Mathf.Abs(base.transform.position.x + this.unit.xI * this.t - pos.x) < 2f)
			{
				this.followMode = BroFollowMode.CopyCat;
				this.inputDelay -= this.inputChanges[0].timeLeft;
				for (int i = 1; i < this.inputChanges.Count; i++)
				{
					this.inputChanges[i].timeLeft -= this.inputChanges[0].timeLeft;
				}
				this.inputChanges[0].timeLeft = 0f;
			}
			if (Vector3.Distance(base.transform.position, pos) > 32f)
			{
				this.DoCatchup();
			}
		}
		else
		{
			this.followMode = BroFollowMode.CopyCat;
		}
	}

	private void GetCopyCatInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool buttonJump, ref bool fire)
	{
		up = this.trackedInputState.up;
		down = this.trackedInputState.down;
		left = this.trackedInputState.left;
		right = this.trackedInputState.right;
		buttonJump = this.trackedInputState.jump;
		if (!up && !down && !left && !right && !buttonJump && this.unit.actionState != ActionState.Jumping && !this.trackedInputState.up && !this.trackedInputState.down && !this.trackedInputState.left && !this.trackedInputState.right && !this.trackedInputState.jump)
		{
			if (this.inputChanges.Count == 0)
			{
				if ((this.repositionDelay -= this.t) < 0f && this.inputChanges.Count == 0 && Mathf.Abs(this.leadingBro.xI * this.leadingBro.yI) < 20f)
				{
					if (this.leadingBro.transform.localScale.x > 0f)
					{
						if (base.transform.position.x > this.leadingBro.transform.position.x - (4f * (float)this.position + 2f) && (this.repositionTimeLeft -= this.t) > 0f)
						{
							left = true;
						}
						else
						{
							base.transform.localScale = Vector3.one;
							this.unit.xI = 0.0001f;
						}
					}
					else if (base.transform.position.x < this.leadingBro.transform.position.x + (4f * (float)this.position + 2f) && (this.repositionTimeLeft -= this.t) > 0f)
					{
						right = true;
					}
					else
					{
						base.transform.localScale = new Vector3(-1f, 1f, 1f);
						this.unit.xI = -0.0001f;
					}
				}
			}
			else if (this.repositionDelay < 0f)
			{
				this.followMode = BroFollowMode.Calibrate;
			}
		}
	}

	public TestVanDammeAnim leadingBro;

	private Unit unit;

	public BroFollowMode followMode;

	private float arbitraryTrackingDelay;

	private float inputDelay;

	private float repositionDelay = 0.2f;

	private float repositionTimeLeft = 0.4f;

	private float calibrateTimer;

	private InputState trackedInputState = new InputState();

	private List<InputStateChange> inputChanges = new List<InputStateChange>();

	private List<InputStateChange> fireInputChanges = new List<InputStateChange>();

	private bool leadBroWasDashing;

	private float copyCatDistanceSquared = 36f;

	public int position;

	private float t;
}
