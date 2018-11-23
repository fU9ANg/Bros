// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class AlienSpawner : Doodad
{
	public override void Collapse()
	{
		this.isDestroyed = true;
	}

	protected virtual void Update()
	{
		if (Map.isEditing || CutsceneController.isInCutscene)
		{
			return;
		}
		base.GetComponent<Renderer>().enabled = false;
		float num = Mathf.Clamp(Time.deltaTime, 0f, 0.0334f);
		if (!this.isDestroyed && SortOfFollow.IsItSortOfVisible(this.x, this.y, 80f, 32f) && !SortOfFollow.IsItSortOfVisible(this.x, this.y, 8f, 8f))
		{
			this.spawningCounter += num;
			if (this.spawningCounter > this.spawningRate)
			{
				this.spawningCounter -= this.spawningRate;
				this.SpawnAlien();
			}
		}
	}

	protected void SpawnAlien()
	{
		int nearestPlayer = HeroController.GetNearestPlayer(this.x, this.y, 400f, 200f);
		if (nearestPlayer < 0)
		{
			UnityEngine.Debug.LogError("Could not find player to spawn at");
			return;
		}
		if (!Connect.IsHost)
		{
			MonoBehaviour.print("Only master client should be responsible for spawning new mooks");
		}
		string str = "ZAlienXenomorph";
		Vector3 vector = base.transform.position + Vector3.up * 6f;
		GameObject gameObject = Resources.Load("Aliens/" + str) as GameObject;
		UnityEngine.Debug.Log("Found Alien Prefab " + (gameObject != null));
		Mook component = gameObject.GetComponent<Mook>();
		Mook arg = MapController.SpawnMook_Networked(component, vector.x, vector.y, 0f, 0f, false, false, false, false, false);
		Networking.RPC<Mook, float, float, bool, int>(PID.TargetAll, new RpcSignature<Mook, float, float, bool, int>(this.ReleaseAlien), arg, 0f, 0f, false, nearestPlayer, false);
	}

	public void ReleaseAlien(Mook newAlien, float xI, float yI, bool onFire, int seenPlayer)
	{
		if (newAlien != null)
		{
			newAlien.FullyAlert(this.x, this.y, seenPlayer);
			if (onFire)
			{
				Map.KnockAndDamageUnit(this, newAlien, 3, DamageType.Fire, 0f, 0f, 0, false);
			}
		}
	}

	public bool isDestroyed;

	protected float spawningCounter = 1f;

	protected float spawningRate = 1.2f;
}
