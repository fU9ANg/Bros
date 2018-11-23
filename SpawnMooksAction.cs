// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class SpawnMooksAction : TriggerAction
{
	public override TriggerActionInfo Info
	{
		get
		{
			return this.info;
		}
		set
		{
			this.info = (SpawnMooksActionInfo)value;
		}
	}

	public override void Start()
	{
		this.state = TriggerActionState.Done;
		float blocksX = Map.GetBlocksX(this.info.targetPoint.collumn - Map.lastXLoadOffset);
		float blocksY = Map.GetBlocksY(this.info.targetPoint.row - Map.lastYLoadOffset);
		for (int i = 0; i < this.info.mooksCount; i++)
		{
			UnityEngine.Object @object = Resources.Load("Mooks/ZMook");
			Mook component = (@object as GameObject).GetComponent<Mook>();
			this.SpawnMook(component, blocksX + ((this.info.mooksCount <= 1) ? ((float)(-(float)this.info.xRange / 2) + (float)this.info.xRange * UnityEngine.Random.value) : (-((float)this.info.xRange * 0.5f) + (float)(this.info.xRange / (this.info.mooksCount - 1) * i))), blocksY + ((this.info.mooksCount <= 1) ? ((float)(-(float)this.info.yRange / 2) + (float)this.info.yRange * UnityEngine.Random.value) : (-((float)this.info.yRange * 0.5f) + (float)this.info.yRange * UnityEngine.Random.value)));
		}
		for (int j = 0; j < this.info.mooksSuicideCount; j++)
		{
			UnityEngine.Object object2 = Resources.Load("Mooks/ZMookSuicide");
			Mook component2 = (object2 as GameObject).GetComponent<Mook>();
			this.SpawnMook(component2, blocksX + ((this.info.mooksSuicideCount <= 1) ? ((float)(-(float)this.info.xRange / 2) + (float)this.info.xRange * UnityEngine.Random.value) : (-((float)this.info.xRange * 0.5f) + (float)(this.info.xRange / (this.info.mooksSuicideCount - 1) * j))), blocksY + ((this.info.mooksSuicideCount <= 1) ? ((float)(-(float)this.info.yRange / 2) + (float)this.info.yRange * UnityEngine.Random.value) : (-((float)this.info.yRange * 0.5f) + (float)this.info.yRange * UnityEngine.Random.value)));
		}
		if (this.info.spawnTruck)
		{
			UnityEngine.Object object3 = Resources.Load("Vehicles/MookTruck");
			MookTruck component3 = (object3 as GameObject).GetComponent<MookTruck>();
			this.SpawnTruck(component3, blocksX, blocksY);
		}
	}

	protected void SpawnTruck(MookTruck prefab, float x, float y)
	{
		if (prefab != null)
		{
			float groundHeight = Map.GetGroundHeight(x, y + 16f);
			if (SortOfFollow.GetScreenMaxX() > x - 24f)
			{
				float groundHeight2 = Map.GetGroundHeight(SortOfFollow.GetScreenMaxX() + 24f, y + 16f);
				if (groundHeight2 == groundHeight)
				{
					x = SortOfFollow.GetScreenMaxX() + 24f;
				}
				else if (SortOfFollow.IsItSortOfVisible(x, y))
				{
					return;
				}
			}
			y = groundHeight + 8f;
			Networking.InstantiateBuffered<GameObject>(prefab.gameObject, new Vector3(x, y, 0f), Quaternion.identity, null, false);
		}
		else
		{
			UnityEngine.Debug.LogError("Could not instantiate resource");
		}
	}

	protected void SpawnMook(Mook prefab, float x, float y)
	{
		if (prefab != null)
		{
			MapController.SpawnMook_Networked(prefab, x, y, 0f, 0f, this.info.tumble, false, this.info.parachute, this.info.isOnFire, this.info.isAlert);
		}
		else
		{
			UnityEngine.Debug.LogError("Could not instantiate resource");
		}
	}

	public override void Update()
	{
	}

	public SpawnMooksActionInfo info;
}
