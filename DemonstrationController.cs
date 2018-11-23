// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DemonstrationController : MonoBehaviour
{
	private void Awake()
	{
		Demonstration.projectilesHitWalls = this.projectilesHitWalls;
		Demonstration.mooksStartUnknowing = this.mooksStartUnknowing;
		Demonstration.bulletsHurtWalls = this.bulletsHurtWalls;
		Demonstration.bulletsAreFast = this.bulletsAreFast;
		Demonstration.enemiesSeeThroughWalls = this.enemiesSeeThroughWalls;
		Demonstration.herosClimbWalls = this.herosClimbWalls;
		Demonstration.enemiesHaveDelayOnAlert = this.enemiesHaveDelayOnAlert;
		Demonstration.canPushBlocks = this.canPushBlocks;
		Demonstration.enemiesSetOnFire = this.enemiesSetOnFire;
		Demonstration.enemiesSpreadFire = this.enemiesSpreadFire;
		Demonstration.enemiesWanderFar = this.enemiesWanderFar;
		Demonstration.infiniteLives = this.infiniteLives;
		Demonstration.enemiesAlreadyAware = this.enemiesAlreadyAware;
	}

	private void Update()
	{
	}

	public bool projectilesHitWalls = true;

	public bool mooksStartUnknowing = true;

	public bool bulletsHurtWalls = true;

	public bool bulletsAreFast = true;

	public bool enemiesSeeThroughWalls;

	public bool herosClimbWalls = true;

	public bool enemiesHaveDelayOnAlert = true;

	public bool canPushBlocks = true;

	public bool enemiesSetOnFire = true;

	public bool enemiesSpreadFire = true;

	public bool enemiesWanderFar = true;

	public bool infiniteLives = true;

	public bool enemiesAlreadyAware;
}
