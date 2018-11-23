// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FollowerMinion : MonoBehaviour
{
	private void Start()
	{
		this.pathAgent = base.GetComponent<PathAgent>();
		this.unit = base.GetComponent<TestVanDammeAnim>();
		this.moveDelay = UnityEngine.Random.value; this.pathingDelay = (this.moveDelay );
	}

	public bool GetInput(ref bool left, ref bool right, ref bool up, ref bool down, ref bool jump, ref bool fire)
	{
		fire = false; up = (down = (left = (right = (jump = (fire )))));
		if (this.leader != null)
		{
			this.pathingDelay -= Time.deltaTime;
			if (this.pathAgent.CurrentPath == null && this.pathingDelay < 0f && this.unit.actionState != ActionState.Jumping && this.leader.actionState != ActionState.Jumping)
			{
				CheckPoint nearestCheckPointToRight = Map.GetNearestCheckPointToRight(this.unit.x - 32f, this.unit.y, true);
				if (nearestCheckPointToRight != null)
				{
					this.pathAgent.GetPath(Map.GetCollumn(nearestCheckPointToRight.x) + UnityEngine.Random.Range(-2, 1), Map.GetRow(nearestCheckPointToRight.y), 10f);
				}
				this.pathingDelay = 0.1f + UnityEngine.Random.value * 2f;
			}
			if (this.pathAgent.CurrentPath != null && (this.moveDelay -= Time.deltaTime) < 0f)
			{
				LevelEditorGUI.displayPath = this.pathAgent.CurrentPath;
				this.pathAgent.GetMove(ref left, ref right, ref up, ref down, ref jump);
			}
			if (this.CanSeeEnemy())
			{
				right = false; left = (right );
				if (this.enemyDirection > 0 && this.unit.transform.localScale.x < 0f)
				{
					right = true;
				}
				if (this.enemyDirection < 0 && this.unit.transform.localScale.x > 0f)
				{
					left = true;
				}
				fire = true;
				this.moveDelay = 0.1f + UnityEngine.Random.value * 1.3f;
			}
			else
			{
				fire = false;
			}
			return true;
		}
		return false;
	}

	internal void FindLeader()
	{
		int num = -1;
		HeroController.GetNearestPlayer(this.unit.x, this.unit.y, 128f, 128f, ref num);
		if (num >= 0)
		{
			this.leader = HeroController.players[num].character;
			base.gameObject.SetOwnerNetworked(this.leader.Owner);
			this.leaderPlayerNo = num;
			HeroController.players[num].RegisterMinion(this.unit);
		}
	}

	private void OnDestroy()
	{
		if (this.leaderPlayerNo >= 0 && HeroController.players[this.leaderPlayerNo] != null)
		{
			HeroController.players[this.leaderPlayerNo].DeRegisterMinion(this.unit);
		}
	}

	private bool CanSeeEnemy()
	{
		Unit unit = Map.GeLivingtUnit(this.unit.playerNum, this.visRange, 1f, this.unit.x + base.transform.localScale.x * this.visRange * 0.5f, this.unit.y + 6f);
		if (unit != null)
		{
			if (unit.GetComponent<Animal>() != null)
			{
				return false;
			}
			if (unit.x > this.unit.x)
			{
				this.enemyDirection = 1;
			}
			else
			{
				this.enemyDirection = -1;
			}
			Vector3 position = base.transform.position;
			position.y += 4f;
			Vector3 position2 = unit.transform.position;
			position2.y += 4f;
			if (!Physics.Raycast(new Ray(position, position2 - position), Vector3.Distance(position, position2), 1 << LayerMask.NameToLayer("Terrain")))
			{
				return true;
			}
		}
		return false;
	}

	private PathAgent pathAgent;

	public Unit leader;

	private float visRange = 64f;

	private TestVanDammeAnim unit;

	private float pathingDelay;

	private int enemyDirection;

	private float moveDelay;

	private int leaderPlayerNo;
}
