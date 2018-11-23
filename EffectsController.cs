// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
	private void Awake()
	{
		EffectsController.groundLayer = 1 << LayerMask.NameToLayer("Ground");
	}

	private void Start()
	{
		if (Map.MapData != null && Map.MapData.theme == LevelTheme.City)
		{
			this.fogObject.gameObject.SetActive(true);
			this.sunObject.gameObject.SetActive(true);
		}
		else
		{
			this.fogObject.gameObject.SetActive(false);
			this.sunObject.gameObject.SetActive(false);
		}
		if (Map.MapData != null && Map.MapData.theme == LevelTheme.BurningJungle && GameModeController.GameMode != GameMode.BroDown)
		{
			this.fireAndAsh = true;
			this.ashHolder.gameObject.SetActive(true);
			this.rainFollowTransform = SortOfFollow.GetInstance().transform;
		}
	}

	public void StartRainAndLightning()
	{
		this.lightningController = base.GetComponent<LightningController>();
		this.lightningController.enabled = true;
		this.rainHolder.gameObject.SetActive(true);
		this.rainFollowTransform = SortOfFollow.GetInstance().transform;
		this.rainAndLightning = true;
		this.fogObject.gameObject.SetActive(true);
		this.fogObject.transform.parent = null;
		if (GameModeController.IsDeathMatchMode)
		{
			this.fogObject.transform.position = new Vector3(this.fogObject.transform.position.x, 70f, this.fogObject.transform.position.z);
		}
	}

	private void Update()
	{
		if (this.rainAndLightning && this.rainFollowTransform != null)
		{
			this.rainHolder.position = new Vector3(this.rainFollowTransform.position.x - 25f, this.rainFollowTransform.position.y + 380f, 0f);
		}
		if (this.fireAndAsh && this.rainFollowTransform != null)
		{
			this.ashHolder.position = new Vector3(this.rainFollowTransform.position.x - 25f, this.rainFollowTransform.position.y + 380f, 0f);
		}
		if (!PlaytomicController.isExpendabrosBuild && GameModeController.GameMode == GameMode.Campaign && GameModeController.IsLevelFinished())
		{
			this.fireWorksCounter += Time.deltaTime;
			if (this.fireWorksCounter > 3f)
			{
				this.fireWorksCount++;
				this.fireWorksCounter -= 0.3f + UnityEngine.Random.value * UnityEngine.Random.value * 0.6f;
				Vector3 position = new Vector3(SortOfFollow.followPos.x - 206f + UnityEngine.Random.value * 200f + (float)(this.fireWorksCount % 3 * 150) + (float)(this.fireWorksCount % 2 * 350), SortOfFollow.minY + 50f + UnityEngine.Random.value * 100f, 34f);
				VictoryFireWork victoryFireWork = UnityEngine.Object.Instantiate(this.fireWorks[(int)((float)this.fireWorksCount * 1.77f) % this.fireWorks.Length], position, Quaternion.identity) as VictoryFireWork;
			}
		}
	}

	public static EffectsController instance
	{
		get
		{
			if (EffectsController.inst == null)
			{
				EffectsController.inst = (UnityEngine.Object.FindObjectOfType(typeof(EffectsController)) as EffectsController);
			}
			return EffectsController.inst;
		}
	}

	public static Color GetBloodColor(BloodColor color)
	{
		if (color == BloodColor.Green)
		{
			return EffectsController.instance.GreenBloodColor;
		}
		return EffectsController.instance.RedBloodColor;
	}

	public static void CreateFreeLifeBubble(float x, float y)
	{
		EffectsController.instance.freeLifeBubble.RestartBubble();
		EffectsController.instance.freeLifeBubble.transform.position = new Vector3(x, y, -9f);
	}

	public static ReactionBubble CreateKickPlayerBubble(float x, float y)
	{
		ReactionBubble reactionBubble = EffectsController.instance.kickBubble.Clone<ReactionBubble>();
		reactionBubble.RestartBubble();
		reactionBubble.transform.position = new Vector3(x, y, -9f);
		return reactionBubble;
	}

	public static void CreateAmmoBubble(float x, float y)
	{
		EffectsController.instance.ammoBubble.RestartBubble();
		EffectsController.instance.ammoBubble.transform.position = new Vector3(x, y, -9f);
	}

	public static void CreateStoneShrapnel(float x, float y, float radius, float count)
	{
		int num = 0;
		while ((float)num < count)
		{
			EffectsController.tempOffset = UnityEngine.Random.onUnitSphere * radius;
			EffectsController.tempOffset.z = 0f;
			Shrapnel shrapnel = UnityEngine.Object.Instantiate(EffectsController.instance.shrapnelStonePrefab) as Shrapnel;
			shrapnel.Launch(EffectsController.tempOffset.x + x, EffectsController.tempOffset.y + y, EffectsController.tempOffset.x, EffectsController.tempOffset.y);
			num++;
		}
	}

	public static void CreateShrapnel(Shrapnel shrapnelPrefab, float x, float y, float radius, float force, float count, float xI, float yI)
	{
		if (shrapnelPrefab != null)
		{
			int num = 0;
			while ((float)num < count)
			{
				EffectsController.tempOffset = UnityEngine.Random.onUnitSphere * radius;
				EffectsController.tempOffset.z = 0f;
				Shrapnel shrapnel = UnityEngine.Object.Instantiate(shrapnelPrefab) as Shrapnel;
				shrapnel.Launch(EffectsController.tempOffset.x + x, EffectsController.tempOffset.y + y, EffectsController.tempOffset.x * force / radius + xI, EffectsController.tempOffset.y * force / radius + yI);
				num++;
			}
		}
	}

	public static void CreateShrapnelCircle(Shrapnel shrapnelPrefab, float x, float y, float radius, float force, float count, float xI, float yI)
	{
		if (shrapnelPrefab != null)
		{
			float num = 3.14159274f * Mathf.Clamp(0.1f + count * 0.18f, 0f, 2f);
			float num2 = num / count;
			float num3 = 1.57079637f - num * 0.5f + UnityEngine.Random.value * num2;
			int num4 = 0;
			while ((float)num4 < count)
			{
				EffectsController.tempOffset = global::Math.Point3OnCircle(num3 + num2 * (float)num4, 1f);
				Shrapnel shrapnel = UnityEngine.Object.Instantiate(shrapnelPrefab) as Shrapnel;
				shrapnel.Launch(EffectsController.tempOffset.x * radius + x, EffectsController.tempOffset.y * radius + y, EffectsController.tempOffset.x * force + xI, EffectsController.tempOffset.y * force + yI);
				num4++;
			}
		}
	}

	public static void CreateShrapnel(Shrapnel shrapnelPrefab, Material sharedMaterial, float x, float y, float radius, float force, float count, float xI, float yI)
	{
		int num = 0;
		while ((float)num < count)
		{
			EffectsController.tempOffset = UnityEngine.Random.onUnitSphere * radius;
			EffectsController.tempOffset.z = 0f;
			Shrapnel shrapnel = UnityEngine.Object.Instantiate(shrapnelPrefab) as Shrapnel;
			shrapnel.GetComponent<Renderer>().sharedMaterial = sharedMaterial;
			shrapnel.Launch(EffectsController.tempOffset.x + x, EffectsController.tempOffset.y + y, EffectsController.tempOffset.x * force / radius + xI, EffectsController.tempOffset.y * force / radius + yI);
			num++;
		}
	}

	public static void CreateShrapnelBlindStar(float x, float y, float radius, float force, float count, float xI, float yI, Transform unitTransform)
	{
		int num = 0;
		while ((float)num < count)
		{
			EffectsController.stunnedStarsCount += 1 + UnityEngine.Random.Range(0, 2);
			int num2 = EffectsController.stunnedStarsCount % 3;
			if (num2 != 0)
			{
				if (num2 != 1)
				{
					PuffTwoLayer puffTwoLayer = EffectsController.CreateEffect(EffectsController.instance.stunnedStars3Prefab, x, y, 0f, new Vector3(xI, yI, 0f), BloodColor.None);
					puffTwoLayer.transform.parent = unitTransform;
				}
				else
				{
					PuffTwoLayer puffTwoLayer = EffectsController.CreateEffect(EffectsController.instance.stunnedStars2Prefab, x, y, 0f, new Vector3(xI, yI, 0f), BloodColor.None);
					puffTwoLayer.transform.parent = unitTransform;
				}
			}
			else
			{
				PuffTwoLayer puffTwoLayer = EffectsController.CreateEffect(EffectsController.instance.stunnedStars1Prefab, x, y, 0f, new Vector3(xI, yI, 0f), BloodColor.None);
				puffTwoLayer.transform.parent = unitTransform;
			}
			num++;
		}
	}

	public static void CreateProjectilePopEffect(float x, float y)
	{
		UnityEngine.Object.Instantiate(EffectsController.instance.projectileFlickPuff, new Vector3(x, y, 0f), Quaternion.identity);
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashSmallGlowPrefab, x, y, 0f);
	}

	public static void CreateProjectileLargePopEffect(float x, float y)
	{
		UnityEngine.Object.Instantiate(EffectsController.instance.projectileLargeFlickPuff, new Vector3(x, y, 0f), Quaternion.identity);
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashSmallGlowPrefab, x, y, 0f);
	}

	public static void CreateProjectilePopWhiteEffect(float x, float y)
	{
		UnityEngine.Object.Instantiate(EffectsController.instance.projectileWhiteFlickPuff, new Vector3(x, y, 0f), Quaternion.identity);
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashSmallGlowPrefab, x, y, 0f);
	}

	public static void CreateEffect(FlickerFader flickerFaderPrefab, float x, float y)
	{
		UnityEngine.Object.Instantiate(flickerFaderPrefab, new Vector3(x, y, 0f), Quaternion.identity);
	}

	public static void CreateEffect(Puff puffPrefab, float x, float y, float z, int xScale, int yScale, BloodColor bloodColor = BloodColor.None)
	{
		Puff puff = UnityEngine.Object.Instantiate(puffPrefab, new Vector3(x, y, z), Quaternion.identity) as Puff;
		if (bloodColor == BloodColor.Red)
		{
			puff.SetColor(EffectsController.inst.RedBloodColor);
		}
		else if (bloodColor == BloodColor.Green)
		{
			puff.SetColor(EffectsController.inst.GreenBloodColor);
		}
		puff.transform.localScale = new Vector3((float)xScale, (float)yScale, 1f);
	}

	public static void CreateEffect(Puff puffPrefab, float x, float y, float z, float delay, int xScale, int yScale, Vector3 velocity)
	{
		Puff puff = EffectsController.CreateEffect(puffPrefab, x, y, delay, velocity, BloodColor.None);
		puff.transform.localScale = new Vector3((float)xScale, (float)yScale, 1f);
	}

	public static void CreateEffect(FlickerFader flickerFaderPrefab, float x, float y, float z, float delay, int xScale, int yScale, Vector3 velocity)
	{
		FlickerFader flickerFader = EffectsController.CreateEffect(flickerFaderPrefab, x, y, delay, velocity);
		flickerFader.transform.localScale = new Vector3((float)xScale, (float)yScale, 1f);
	}

	public static void CreateEffect(FlickerFader flickerFaderPrefab, float x, float y, float z)
	{
		UnityEngine.Object.Instantiate(flickerFaderPrefab, new Vector3(x, y, z), Quaternion.identity);
	}

	public static void CreateBlindedEffect(float x, float y, float delay, Vector3 velocity)
	{
		EffectsController.stunnedStarsCount++;
		int num = EffectsController.stunnedStarsCount % 3;
		if (num != 0)
		{
			if (num != 1)
			{
				EffectsController.CreateEffect(EffectsController.instance.stunnedStars3Prefab, x, y, 0f, Vector3.zero, BloodColor.None);
			}
			else
			{
				EffectsController.CreateEffect(EffectsController.instance.stunnedStars2Prefab, x, y, 0f, Vector3.zero, BloodColor.None);
			}
		}
		else
		{
			EffectsController.CreateEffect(EffectsController.instance.stunnedStars1Prefab, x, y, 0f, Vector3.zero, BloodColor.None);
		}
	}

	public static void CreateSmoke(float x, float y, float delay, Vector3 velocity)
	{
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 0:
			EffectsController.CreateEffect(EffectsController.instance.smokeSmall1Prefab, x, y, delay, velocity, BloodColor.None);
			break;
		case 1:
			EffectsController.CreateEffect(EffectsController.instance.smokeSmall2Prefab, x, y, delay, velocity, BloodColor.None);
			break;
		case 2:
			EffectsController.CreateEffect(EffectsController.instance.smokeSmall3Prefab, x, y, delay, velocity, BloodColor.None);
			break;
		}
	}

	public static void CreateWhiteFlashPop(float x, float y)
	{
		EffectsController.CreateEffect(EffectsController.instance.whiteFlashPopPrefab, x, y, 0f, Vector3.zero, BloodColor.None);
	}

	public static void CreateWhiteFlashPopSmall(float x, float y)
	{
		EffectsController.CreateEffect(EffectsController.instance.whiteFlashPopSmallPrefab, x, y, 0f, Vector3.zero, BloodColor.None);
	}

	public static void CreatePunchPopEffect(float x, float y, Vector3 velocity)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.whitePunchPopPrefab, x, y, 0f, velocity, BloodColor.None);
		if (velocity.x < 0f)
		{
			puff.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	public static void CreateDistortionWobbleRingEffect(float x, float y, float delay)
	{
		EffectsController.CreateEffect(EffectsController.instance.distortionWobbleGrowPrefab, x, y, delay);
	}

	public static void CreateDistortionWobblePinchEffect(float x, float y, float delay)
	{
		EffectsController.CreateEffect(EffectsController.instance.distortionPinchGrowPrefab, x, y, delay);
	}

	public static void CreateAirDashPoofEffect(float x, float y, Vector3 velocity)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.airDashPoofPrefab, x, y, 0f, velocity, BloodColor.None);
		if (velocity.x < 0f)
		{
			puff.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		if (velocity.y != 0f)
		{
			puff.transform.eulerAngles = new Vector3(0f, 0f, 90f);
		}
	}

	public static void CreateFootPoofEffect(float x, float y, float delay, Vector3 velocity)
	{
		int num = UnityEngine.Random.Range(0, 3);
		if (num != 0)
		{
			if (num != 1)
			{
				EffectsController.CreateEffect(EffectsController.instance.footPoof3Prefab, x, y, delay, velocity, BloodColor.None);
			}
			else
			{
				EffectsController.CreateEffect(EffectsController.instance.footPoof2Prefab, x, y, delay, velocity, BloodColor.None);
			}
		}
		else
		{
			EffectsController.CreateEffect(EffectsController.instance.footPoof1Prefab, x, y, delay, velocity, BloodColor.None);
		}
	}

	public static void CreateJumpPoofEffect(float x, float y, int direction)
	{
		Puff puff = null;
		int num = UnityEngine.Random.Range(0, 2);
		if (num != 0)
		{
			if (num == 1)
			{
				puff = EffectsController.CreateEffect(EffectsController.instance.jumpPoof2Prefab, x, y, 0f, Vector3.zero, BloodColor.None);
			}
		}
		else
		{
			puff = EffectsController.CreateEffect(EffectsController.instance.jumpPoof1Prefab, x, y, 0f, Vector3.zero, BloodColor.None);
		}
		if (puff != null && direction < 0)
		{
			puff.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	public static void CreateLandPoofEffect(float x, float y, int direction)
	{
		Puff puff = null;
		int num = UnityEngine.Random.Range(0, 2);
		if (num != 0)
		{
			if (num == 1)
			{
				puff = EffectsController.CreateEffect(EffectsController.instance.landPoof2Prefab, x, y, 0f, Vector3.zero, BloodColor.None);
			}
		}
		else
		{
			puff = EffectsController.CreateEffect(EffectsController.instance.landPoof1Prefab, x, y, 0f, Vector3.zero, BloodColor.None);
		}
		if (puff != null && direction < 0)
		{
			puff.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	public static void CreateDashPoofEffect_Networked(float x, float y, int direction)
	{
		Networking.RPC<float, float, int>(PID.TargetAll, new RpcSignature<float, float, int>(EffectsController.CreateDashPoofEffect_Local), x, y, direction, false);
	}

	public static void CreateDashPoofEffect_Local(float x, float y, int direction)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.dashPoof1Prefab, x, y, 0f, Vector3.zero, BloodColor.None);
		if (direction < 0)
		{
			puff.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	public static void CreateGroundExplodePoofEffect(float x, float y, int direction)
	{
		EffectsController.CreateGroundExplodePoofEffect(EffectsController.instance.groundExplodePoofPrefab, x, y, direction);
	}

	public static Puff CreateGroundExplodePoofEffect(Puff poofPrefab, float x, float y, int direction)
	{
		Puff puff = EffectsController.CreateEffect(poofPrefab, x, y, 0f, Vector3.zero, BloodColor.None);
		if (puff != null && direction < 0)
		{
			puff.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		return puff;
	}

	public static void CreateBulletPoofEffect(float x, float y)
	{
		EffectsController.CreateEffect(EffectsController.instance.bulletPoofPrefab, x, y, 0f, Vector3.zero);
	}

	public static void CreateBackgroundFlameParticle(float x, float y)
	{
		EffectsController.CreateBackgroundFlameParticle(x, y, -12f);
	}

	public static void CreateBackgroundFlameParticle(float x, float y, float z)
	{
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
			EffectsController.CreateParticle(EffectsController.instance.flameSheetBackgroundParticle1, x, y, z);
			break;
		case 1:
			EffectsController.CreateParticle(EffectsController.instance.flameSheetBackgroundParticle2, x, y, z);
			break;
		case 2:
			EffectsController.CreateParticle(EffectsController.instance.flameSheetBackgroundParticle3, x, y, z);
			break;
		case 3:
			EffectsController.CreateParticle(EffectsController.instance.flameSheetBackgroundParticle4, x, y, z);
			break;
		}
	}

	public static void CreateFlameParticle(float x, float y)
	{
		EffectsController.CreateFlameParticle(x, y, -12f);
	}

	public static void CreateFlameParticle(float x, float y, float z)
	{
		switch (UnityEngine.Random.Range(0, 4))
		{
		case 0:
			EffectsController.CreateParticle(EffectsController.instance.flameSheetParticle1, x, y, z);
			break;
		case 1:
			EffectsController.CreateParticle(EffectsController.instance.flameSheetParticle2, x, y, z);
			break;
		case 2:
			EffectsController.CreateParticle(EffectsController.instance.flameSheetParticle3, x, y, z);
			break;
		case 3:
			EffectsController.CreateParticle(EffectsController.instance.flameSheetParticle4, x, y, z);
			break;
		}
	}

	public static void CreatePlumes(float x, float y, int count, float radius, float force, float xI, float yI)
	{
		EffectsController.CreateShrapnelCircle(EffectsController.instance.plumeShrapnelPrefab, x, y, radius, force, (float)count, xI, yI);
	}

	public static void CreateBlackPlumeParticle(float x, float y, float force, float xI, float yI, float lifeM, float sizeM)
	{
		int num = UnityEngine.Random.Range(0, 2);
		if (num != 0)
		{
			if (num == 1)
			{
				EffectsController.CreateParticle(EffectsController.instance.plumePuffParticleBlack2, x, y, force, xI, yI, lifeM, sizeM);
			}
		}
		else
		{
			EffectsController.CreateParticle(EffectsController.instance.plumePuffParticleBlack1, x, y, force, xI, yI, lifeM, sizeM);
		}
	}

	public static void CreatePitchBlackPlumeParticle(float x, float y, float force, float xI, float yI, float lifeM, float sizeM)
	{
		EffectsController.CreateParticle(EffectsController.instance.plumePuffParticlePitchBlack, x, y, force, xI, yI, lifeM, sizeM);
	}

	public static void CreatePlumeParticle(float x, float y, float force, float xI, float yI, float lifeM, float sizeM)
	{
		EffectsController.CreatePlumeParticle(x, y, 0f, force, xI, yI, lifeM, sizeM);
	}

	public static void CreatePlumeParticle(float x, float y, float z, float force, float xI, float yI, float lifeM, float sizeM)
	{
		if (sizeM < 2f)
		{
			if (UnityEngine.Random.value > 0.5f)
			{
				EffectsController.CreateParticle(EffectsController.instance.plumePuffParticle1, x, y, z, force, xI, yI, lifeM, sizeM);
			}
			else
			{
				EffectsController.CreateParticle(EffectsController.instance.plumePuffParticle2, x, y, z, force, xI, yI, lifeM, sizeM);
			}
		}
		else if (UnityEngine.Random.value > 0.5f)
		{
			EffectsController.CreateParticle(EffectsController.instance.plumePuffBigParticle1, x, y, z, force, xI, yI, lifeM, sizeM / 2f);
		}
		else
		{
			EffectsController.CreateParticle(EffectsController.instance.plumePuffBigParticle2, x, y, z, force, xI, yI, lifeM, sizeM / 2f);
		}
	}

	public static void CreateMetalParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI)
	{
		EffectsController.CreateSandParticles(EffectsHolderTheme.GetDotsDirtEmitter(), x, y, count / 2, offsetRadius, force * 1.4f, xI, yI);
		EffectsController.CreateParticles(EffectsHolderTheme.GetSmallMetalEmitter1(), x, y, count / 2, offsetRadius, force, xI, yI, Color.white, 0f, 0f);
	}

	public static void CreateSandParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI)
	{
		EffectsController.CreateSandParticles(EffectsHolderTheme.GetDotsSandEmitter(), x, y, count, offsetRadius, force * 1.4f, xI, yI);
		EffectsController.CreateParticles(EffectsHolderTheme.GetSmallSandEmitter1(), x, y, count / 3, offsetRadius, force, xI, yI, Color.white, 0f, 0f);
	}

	public static void CreateAlienParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI)
	{
		EffectsController.CreateParticles(EffectsHolderTheme.Instance.particlesAlienEarthDots, x, y, count / 2, offsetRadius, force * 1.4f, xI, yI);
	}

	public static void CreateDirtParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI)
	{
		EffectsController.CreateDustParticles(EffectsHolderTheme.GetDotsDirtEmitter(), x, y, count / 2, offsetRadius, force * 1.4f, xI, yI);
		EffectsController.CreateParticles(EffectsHolderTheme.GetSmallDirtEmitter1(), x, y, count / 3, offsetRadius, force, xI, yI, Color.white, 0f, 0f);
		EffectsController.CreateParticles(EffectsHolderTheme.GetSmallDirtEmitter2(), x, y, count / 3, offsetRadius, force, xI, yI, Color.white, 0f, 0f);
		EffectsController.CreateParticles(EffectsHolderTheme.GetSmallDirtEmitter3(), x, y, count / 3, offsetRadius, force * 1.1f, xI, yI, Color.white, 0f, 0f);
	}

	public static void CreateBackgroundWoodParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI)
	{
		EffectsController.CreateShrapnel(EffectsController.instance.backgroundWoodShrapnel1, x, y, offsetRadius, force, (float)(count / 4 + 1), xI, yI);
		EffectsController.CreateShrapnel(EffectsController.instance.backgroundWoodShrapnel2, x, y, offsetRadius, force, (float)(count / 4 + 1), xI, yI);
		EffectsController.CreateShrapnel(EffectsController.instance.backgroundWoodShrapnel3, x, y, offsetRadius, force, (float)(count / 4), xI, yI);
		EffectsController.CreateShrapnel(EffectsController.instance.backgroundWoodShrapnel4, x, y, offsetRadius, force, (float)(count / 4), xI, yI);
	}

	public static void CreateWoodParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI, float angularVelocity)
	{
		EffectsController.CreateWoodParticles(x, y, count, offsetRadius, offsetRadius, force, xI, yI, angularVelocity);
	}

	public static void CreateWoodParticles(float x, float y, int count, float xOffsetRadius, float yOffsetRadius, float force, float xI, float yI, float angularVelocity)
	{
		EffectsController.CreateParticles(EffectsHolderTheme.GetWoodEmitter1(), x, y, count / 3, xOffsetRadius, yOffsetRadius, force * 1.2f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetWoodEmitter2(), x, y, count / 3, xOffsetRadius, yOffsetRadius, force * 1.1f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetWoodEmitter3(), x, y, count / 2, xOffsetRadius, yOffsetRadius, force * 1f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetWoodEmitterDots(), x, y, count / 2, xOffsetRadius, yOffsetRadius, 40f + force * 1.6f, xI, yI, EffectsHolderTheme.GetRandomWoodColor(), 0f, 0f);
	}

	public static void CreateMetalParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI, float angularVelocity)
	{
		EffectsController.CreateMetalParticles(x, y, count, offsetRadius, offsetRadius, force, xI, yI, angularVelocity);
	}

	public static void CreateMetalParticles(float x, float y, int count, float xOffsetRadius, float yOffsetRadius, float force, float xI, float yI, float angularVelocity)
	{
		EffectsController.CreateParticles(EffectsHolderTheme.GetMetalEmitter1(), x, y, 1 + count / 2, xOffsetRadius, yOffsetRadius, force * 1.2f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetMetalEmitter2(), x, y, count / 2, xOffsetRadius, yOffsetRadius, force * 1.1f, xI, yI, Color.white, -angularVelocity, angularVelocity);
	}

	public static void CreateScrapParticles(float x, float y, int count, float xOffsetRadius, float yOffsetRadius, float force, float xI, float yI, float angularVelocity)
	{
		EffectsController.CreateParticles(EffectsHolderTheme.GetScrapEmitter1(), x, y, 1 + count / 7, xOffsetRadius, yOffsetRadius, force * 1f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetScrapEmitter2(), x, y, count / 7, xOffsetRadius, yOffsetRadius, force * 1.1f, xI, yI, Color.white, -angularVelocity * 1.5f, angularVelocity * 1.5f);
		EffectsController.CreateParticles(EffectsHolderTheme.GetScrapEmitter3(), x, y, count / 6, xOffsetRadius, yOffsetRadius, force * 1f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetScrapEmitter4(), x, y, count / 6, xOffsetRadius, yOffsetRadius, force * 1f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetScrapEmitter5(), x, y, count / 6, xOffsetRadius, yOffsetRadius, force * 1f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetDotsDirtEmitter(), x, y, count / 6, xOffsetRadius, yOffsetRadius, force * 1.4f, xI, yI, Color.grey, 0f, 0f);
		EffectsController.CreateParticles(EffectsHolderTheme.GetScrapEmitterTiny1(), x, y, count / 5, xOffsetRadius, yOffsetRadius, force * 1.6f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetScrapEmitterTiny2(), x, y, count / 5, xOffsetRadius, yOffsetRadius, force * 1.6f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetMetalEmitter1(), x, y, count / 8, xOffsetRadius, yOffsetRadius, force * 1.6f, xI, yI, Color.white, -angularVelocity, angularVelocity);
		EffectsController.CreateParticles(EffectsHolderTheme.GetMetalEmitter2(), x, y, count / 8, xOffsetRadius, yOffsetRadius, force * 1.6f, xI, yI, Color.white, -angularVelocity, angularVelocity);
	}

	public static void CreateDustParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI)
	{
		EffectsController.CreateDustParticles(EffectsHolderTheme.GetDotsDirtEmitter(), x, y, count, offsetRadius, force, xI, yI);
	}

	public static void CreateDustParticles(ParticleEmitter emitter, float x, float y, int count, float offsetRadius, float force, float xI, float yI)
	{
		for (int i = 0; i < count; i++)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			float x2 = x + offsetRadius * insideUnitCircle.x;
			float y2 = y + offsetRadius * insideUnitCircle.y;
			Color randomDirtColor = EffectsHolderTheme.GetRandomDirtColor();
			emitter.Emit(new Vector3(x2, y2, 1f), new Vector3(xI + force * insideUnitCircle.x, yI + force * insideUnitCircle.y, 0f), Mathf.Lerp(emitter.minSize, emitter.maxSize, UnityEngine.Random.value), Mathf.Lerp(emitter.minEnergy, emitter.maxEnergy, UnityEngine.Random.value), randomDirtColor);
		}
	}

	public static void CreateParticles(ParticleEmitter emitter, float x, float y, int count, float offsetRadius, float force, float xI, float yI)
	{
		for (int i = 0; i < count; i++)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			float x2 = x + offsetRadius * insideUnitCircle.x;
			float y2 = y + offsetRadius * insideUnitCircle.y;
			emitter.Emit(new Vector3(x2, y2, 1f), new Vector3(xI + force * insideUnitCircle.x, yI + force * insideUnitCircle.y, 0f), Mathf.Lerp(emitter.minSize, emitter.maxSize, UnityEngine.Random.value), Mathf.Lerp(emitter.minEnergy, emitter.maxEnergy, UnityEngine.Random.value), Color.white);
		}
	}

	public static void CreateSandParticles(ParticleEmitter emitter, float x, float y, int count, float offsetRadius, float force, float xI, float yI)
	{
		for (int i = 0; i < count; i++)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			float x2 = x + offsetRadius * insideUnitCircle.x;
			float y2 = y + offsetRadius * insideUnitCircle.y;
			Color randomSandColor = EffectsHolderTheme.GetRandomSandColor();
			emitter.Emit(new Vector3(x2, y2, 1f), new Vector3(xI + force * insideUnitCircle.x, yI + force * insideUnitCircle.y, 0f), Mathf.Lerp(emitter.minSize, emitter.maxSize, UnityEngine.Random.value), Mathf.Lerp(emitter.minEnergy, emitter.maxEnergy, UnityEngine.Random.value), randomSandColor);
		}
	}

	public static void CreateDustParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI, Color color)
	{
		EffectsController.CreateDustParticles(EffectsHolderTheme.GetDotsDirtEmitter(), x, y, count, offsetRadius, force, xI, yI, color);
	}

	public static void CreateDustParticles(ParticleEmitter emitter, float x, float y, int count, float offsetRadius, float force, float xI, float yI, Color color)
	{
		for (int i = 0; i < count; i++)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			float x2 = x + offsetRadius * insideUnitCircle.x;
			float y2 = y + offsetRadius * insideUnitCircle.y;
			emitter.Emit(new Vector3(x2, y2, 1f), new Vector3(xI + force * insideUnitCircle.x, yI + force * insideUnitCircle.y, 0f), Mathf.Lerp(emitter.minSize, emitter.maxSize, UnityEngine.Random.value), Mathf.Lerp(emitter.minEnergy, emitter.maxEnergy, UnityEngine.Random.value), color);
		}
	}

	public static void CreateParticle(ParticleEmitter emitter, float x, float y, float z)
	{
		emitter.Emit(new Vector3(x, y, z), Vector3.zero, Mathf.Lerp(emitter.minSize, emitter.maxSize, UnityEngine.Random.value), Mathf.Lerp(emitter.minEnergy, emitter.maxEnergy, UnityEngine.Random.value), Color.white);
	}

	public static void CreateParticle(ParticleEmitter emitter, float x, float y, float force, float xI, float yI)
	{
		Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
		emitter.Emit(new Vector3(x, y, 1f), new Vector3(xI + force * insideUnitCircle.x, yI + force * insideUnitCircle.y, 0f), Mathf.Lerp(emitter.minSize, emitter.maxSize, UnityEngine.Random.value), Mathf.Lerp(emitter.minEnergy, emitter.maxEnergy, UnityEngine.Random.value), Color.white);
	}

	public static void CreateParticle(ParticleEmitter emitter, float x, float y, float force, float xI, float yI, float lifeM, float sizeM)
	{
		EffectsController.CreateParticle(emitter, x, y, 1f, force, xI, yI, lifeM, sizeM);
	}

	public static void CreateParticle(ParticleEmitter emitter, float x, float y, float z, float force, float xI, float yI, float lifeM, float sizeM)
	{
		Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
		emitter.Emit(new Vector3(x, y, z), new Vector3(xI + force * insideUnitCircle.x, yI + force * insideUnitCircle.y, 0f), Mathf.Lerp(emitter.minSize, emitter.maxSize, UnityEngine.Random.value) * sizeM, Mathf.Lerp(emitter.minEnergy, emitter.maxEnergy, UnityEngine.Random.value) * lifeM, Color.white);
	}

	public static void CreateParticles(ParticleEmitter emitter, float x, float y, int count, float offsetRadius, float force, float xI, float yI, Color color, float angularVelocityMin, float angularVelocityMax)
	{
		EffectsController.CreateParticles(emitter, x, y, count, offsetRadius, offsetRadius, force, xI, yI, color, angularVelocityMin, angularVelocityMax);
	}

	public static void CreateParticles(ParticleEmitter emitter, float x, float y, int count, float xOffsetRadius, float yOffsetRadius, float force, float xI, float yI, Color color, float angularVelocityMin, float angularVelocityMax)
	{
		EffectsController.CreateParticles(emitter, x, y, 0f, count, xOffsetRadius, yOffsetRadius, force, xI, yI, color, angularVelocityMin, angularVelocityMax);
	}

	public static void CreateParticles(ParticleEmitter emitter, float x, float y, float z, int count, float xOffsetRadius, float yOffsetRadius, float force, float xI, float yI, Color color, float angularVelocityMin, float angularVelocityMax)
	{
		for (int i = 0; i < count; i++)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			float x2 = x + xOffsetRadius * insideUnitCircle.x;
			float y2 = y + yOffsetRadius * insideUnitCircle.y;
			float num = Mathf.Lerp(angularVelocityMin, angularVelocityMax, UnityEngine.Random.value);
			float value = UnityEngine.Random.value;
			emitter.Emit(new Vector3(x2, y2, 1f), new Vector3(xI + force * insideUnitCircle.x, yI + force * insideUnitCircle.y, z), Mathf.Lerp(emitter.minSize, emitter.maxSize, value), Mathf.Lerp(emitter.minEnergy, emitter.maxEnergy, value), color, num * 0.2f, num);
		}
	}

	public static void CreateFireSparks(float x, float y, int count, float offsetRadius, float force, float xI, float yI, float redM)
	{
		EffectsController.CreateSparkParticles(EffectsController.instance.sparkParticleFire, x, y, count, offsetRadius, force, xI, yI, redM);
	}

	public static void CreateSuddenSparkShower(float x, float y, int count, float offsetRadius, float force, float xI, float yI, float redM)
	{
		EffectsController.CreateSparkParticles(EffectsController.instance.sparkParticleFloat, x, y, count, offsetRadius, force, xI, yI, redM, 0.2f + UnityEngine.Random.value * 0.2f);
	}

	public static void CreateSparkShower(float x, float y, int count, float offsetRadius, float force, float xI, float yI, float redM, float particlesFloatM)
	{
		EffectsController.CreateSparkParticles(EffectsController.instance.sparkParticleShower, x, y, (int)((float)count * (1f - particlesFloatM)), offsetRadius, force, xI, yI, redM);
		EffectsController.CreateSparkParticles(EffectsController.instance.sparkParticleFloat, x, y, (int)((float)count * particlesFloatM), offsetRadius, force, xI, yI, redM);
	}

	public static void CreateSparkParticle(float x, float y, float offsetRadius, float force, float xI, float yI, float redM)
	{
		EffectsController.CreateSparkParticle(x, y, offsetRadius, force, xI, yI, redM, 1f);
	}

	public static void CreateSparkParticle(float x, float y, float offsetRadius, float force, float xI, float yI, float redM, float lifeM)
	{
		Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
		float x2 = x + offsetRadius * insideUnitCircle.x;
		float y2 = y + offsetRadius * insideUnitCircle.y;
		float t = (1f - UnityEngine.Random.value) * redM;
		Color color = Color.Lerp(new Color(1f, 1f, 0.3f, 1f), new Color(1f, 0f, 0f, 1f), t);
		EffectsController.instance.sparkParticleFloat.Emit(new Vector3(x2, y2, 1f), new Vector3(xI + force * insideUnitCircle.x, yI + force * insideUnitCircle.y, 0f), Mathf.Lerp(EffectsController.instance.sparkParticleFloat.minSize, EffectsController.instance.sparkParticleFloat.maxSize, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.sparkParticleFloat.minEnergy, EffectsController.instance.sparkParticleFloat.maxEnergy, UnityEngine.Random.value) * lifeM, color);
	}

	public static void CreateSparkParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI, float redM)
	{
		for (int i = 0; i < count; i++)
		{
			EffectsController.CreateSparkParticle(x, y, offsetRadius, force, xI, yI, redM);
		}
	}

	public static void CreateSparkParticles(float x, float y, int count, float offsetRadius, float force, float xI, float yI, float redM, float lifeM)
	{
		for (int i = 0; i < count; i++)
		{
			EffectsController.CreateSparkParticle(x, y, offsetRadius, force, xI, yI, redM, lifeM);
		}
	}

	public static void CreateSparkParticles(ParticleEmitter emitter, float x, float y, int count, float offsetRadius, float force, float xI, float yI, float redM)
	{
		EffectsController.CreateSparkParticles(emitter, x, y, count, offsetRadius, force, xI, yI, redM, 1f);
	}

	public static void CreateSparkParticles(ParticleEmitter emitter, float x, float y, int count, float offsetRadius, float force, float xI, float yI, float redM, float lifeM)
	{
		for (int i = 0; i < count; i++)
		{
			Vector2 insideUnitCircle = UnityEngine.Random.insideUnitCircle;
			float x2 = x + offsetRadius * insideUnitCircle.x;
			float y2 = y + offsetRadius * insideUnitCircle.y;
			float t = (1f - UnityEngine.Random.value) * redM;
			Color color = Color.Lerp(new Color(1f, 1f, 0.3f, 1f), new Color(1f, 0f, 0f, 1f), t);
			emitter.Emit(new Vector3(x2, y2, 1f), new Vector3(xI + force * insideUnitCircle.x, yI + force * insideUnitCircle.y, 0f), Mathf.Lerp(emitter.minSize, emitter.maxSize, UnityEngine.Random.value), Mathf.Lerp(emitter.minEnergy, emitter.maxEnergy, UnityEngine.Random.value) * lifeM, color);
		}
	}

	public static void CreateGibs(GibHolder gibs, Material material, float x, float y, float xForce, float yForce, float xI, float yI)
	{
		for (int i = 0; i < gibs.transform.childCount; i++)
		{
			EffectsController.CreateGib(gibs.transform.GetChild(i).GetComponent<Gib>(), material, x, y, xForce * (0.8f + UnityEngine.Random.value * 0.4f), yForce * (0.8f + UnityEngine.Random.value * 0.4f), xI, yI, 1);
		}
	}

	public static void CreateGibs(GibHolder gibs, float x, float y, float xForce, float yForce, float xI, float yI)
	{
		for (int i = 0; i < gibs.transform.childCount; i++)
		{
			EffectsController.CreateGib(gibs.transform.GetChild(i).GetComponent<Gib>(), gibs.transform.GetChild(i).GetComponent<Renderer>().sharedMaterial, x, y, xForce * (0.8f + UnityEngine.Random.value * 0.4f), yForce * (0.8f + UnityEngine.Random.value * 0.4f), xI, yI, 1);
		}
	}

	public static void CreateGibs(GibHolder gibs, int count, float x, float y, float xForce, float yForce, float xI, float yI)
	{
		EffectsController.CreateGibs(gibs, count, x, y, xForce, yForce, xI, yI, 1);
	}

	public static void CreateGibs(GibHolder gibs, int count, float x, float y, float xForce, float yForce, float xI, float yI, int flipDirection)
	{
		for (int i = 0; i < count; i++)
		{
			int index = UnityEngine.Random.Range(0, gibs.transform.childCount);
			EffectsController.CreateGib(gibs.transform.GetChild(index).GetComponent<Gib>(), gibs.transform.GetChild(index).GetComponent<Renderer>().sharedMaterial, x, y, xForce * (0.8f + UnityEngine.Random.value * 0.4f), yForce * (0.8f + UnityEngine.Random.value * 0.4f), xI, yI, flipDirection);
		}
	}

	public static void CreateGib(Gib gibPrefab, Material material, float x, float y, float xForce, float yForce, float xI, float yI, int flipDirection)
	{
		if (gibPrefab != null)
		{
			Gib gib = UnityEngine.Object.Instantiate(gibPrefab) as Gib;
			gib.GetComponent<Renderer>().sharedMaterial = material;
			gib.SetupSprite(gibPrefab.doesRotate, gibPrefab.GetLowerLeftPixel(), gibPrefab.GetPixelDimensions(), gibPrefab.GetSpriteOffset(), gibPrefab.rotateFrames);
			float xI2 = gibPrefab.transform.localPosition.x * (float)flipDirection / 16f * xForce + xI;
			gib.Launch(x + gibPrefab.transform.localPosition.x * (float)flipDirection, y + gibPrefab.transform.localPosition.y, xI2, gibPrefab.transform.localPosition.y / 16f * yForce + yI);
		}
	}

	public static void CreateDoodadGib(SpriteSM baseSprite, DoodadGibsType gibType, Material material, float x, float y, float xForce, float yForce, float xI, float yI, int flipDirection)
	{
		if (gibType == DoodadGibsType.Filecabinet)
		{
			EffectsController.CreateDoodadGib(baseSprite, EffectsController.instance.fileCabinetGibPrefab, material, x, y, xForce, yForce, xI, yI, flipDirection);
		}
		else if (gibType == DoodadGibsType.Metal)
		{
			EffectsController.CreateDoodadGib(baseSprite, EffectsController.instance.metalDoodadGibPrefab, material, x, y, xForce, yForce, xI, yI, flipDirection);
		}
		else if (gibType == DoodadGibsType.SmallMolotov)
		{
			EffectsController.CreateDoodadGib(baseSprite, EffectsController.instance.blankDoodadGibPrefab, material, x, y, xForce, yForce, xI, yI, flipDirection);
			Networking.RPC<float, float, float, int>(PID.TargetAll, new RpcSignature<float, float, float, int>(MapController.BurnGround_Local), 32f, x, y, EffectsController.groundLayer, false);
			Map.HitLivingUnits(null, 15, 3, DamageType.Fire, 12f, x, y, xI, yI, true, false);
		}
		else
		{
			EffectsController.CreateDoodadGib(baseSprite, EffectsController.instance.blankDoodadGibPrefab, material, x, y, xForce, yForce, xI, yI, flipDirection);
		}
	}

	public static void CreateDoodadGib(SpriteSM baseSprite, DoodadPiece doodadGibPrefab, Material material, float x, float y, float xForce, float yForce, float xI, float yI, int flipDirection)
	{
		if (doodadGibPrefab != null)
		{
			DoodadPiece doodadPiece = UnityEngine.Object.Instantiate(doodadGibPrefab, new Vector3(x, y, 0f), Quaternion.identity) as DoodadPiece;
			doodadPiece.SetupSprite(material, baseSprite.lowerLeftPixel, baseSprite.pixelDimensions, baseSprite.offset, baseSprite.width, baseSprite.height);
			doodadPiece.gameObject.SetActive(true);
		}
	}

	public static void CreateBloodParticles(BloodColor color, float x, float y, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		EffectsController.CreateBloodParticles(color, x, y, 1f, count, offsetXRadius, offsetYRadius, force, xI, yI);
	}

	public static void CreateBloodParticles(BloodColor color, float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		EffectsController.CreateBloodParticlesBig(color, x, y, z, count / 3, offsetXRadius * 0.1f, offsetYRadius * 0.1f, force, xI, yI);
		EffectsController.CreateBloodParticlesSmall(color, x, y, z, count / 2 + 1, offsetXRadius * 0.1f, offsetYRadius * 0.1f, force, xI, yI);
		EffectsController.CreateBloodParticlesDots(color, x, y, z, count / 2 + 1, offsetXRadius * 0.1f, offsetYRadius * 0.1f, force, xI, yI, 1f);
	}

	public static void CreateSemenParticles(BloodColor color, float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		EffectsController.CreateSemenParticlesSmall(color, x, y, z, count / 2 + 1, offsetXRadius * 0.1f, offsetYRadius * 0.1f, force, xI, yI);
		EffectsController.CreateSemenParticlesDots(color, x, y, z, count / 2 + 1, offsetXRadius * 0.1f, offsetYRadius * 0.1f, force, xI, yI, 1f);
	}

	public static void CreateBloodParticlesBig(BloodColor color, float x, float y, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		EffectsController.CreateBloodParticlesBig(color, x, y, 1f, count, offsetXRadius, offsetYRadius, force, xI, yI);
	}

	public static void CreateBloodParticlesBig(BloodColor color, float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		for (int i = 0; i < count; i++)
		{
			float num = -1f + UnityEngine.Random.value * 2f;
			float num2 = -1f + UnityEngine.Random.value * 2f;
			float x2 = x + offsetXRadius * num;
			float y2 = y + offsetYRadius * num2;
			if ((EffectsController.instance == null || !EffectsController.instance.gameObject.activeInHierarchy) && CutsceneEffectsController.instance != null)
			{
				CutsceneEffectsController.instance.bloodParticlesBig.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesBig.minSize, CutsceneEffectsController.instance.bloodParticlesBig.maxSize, UnityEngine.Random.value), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesBig.minEnergy, CutsceneEffectsController.instance.bloodParticlesBig.maxEnergy, UnityEngine.Random.value), Color.white * (0.8f + UnityEngine.Random.value * 0.2f));
			}
			else if (color == BloodColor.Green)
			{
				EffectsController.instance.GreenBloodParticlesBig.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(EffectsController.instance.GreenBloodParticlesBig.minSize, EffectsController.instance.GreenBloodParticlesBig.maxSize, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.GreenBloodParticlesBig.minEnergy, EffectsController.instance.GreenBloodParticlesBig.maxEnergy, UnityEngine.Random.value), Color.white * (0.8f + UnityEngine.Random.value * 0.2f));
			}
			else
			{
				EffectsController.instance.RedBloodParticlesBig.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(EffectsController.instance.RedBloodParticlesBig.minSize, EffectsController.instance.RedBloodParticlesBig.maxSize, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.RedBloodParticlesBig.minEnergy, EffectsController.instance.RedBloodParticlesBig.maxEnergy, UnityEngine.Random.value), Color.white * (0.8f + UnityEngine.Random.value * 0.2f));
			}
		}
	}

	public static void CreateBloodParticlesSmall(BloodColor color, float x, float y, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		EffectsController.CreateBloodParticlesSmall(color, x, y, 1f, count, offsetXRadius, offsetYRadius, force, xI, yI);
	}

	public static void CreateBloodParticlesSmall(BloodColor color, float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 2f / force;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			float x2;
			float y2;
			if (!flag || UnityEngine.Random.value > 0.6f)
			{
				num = -1f + UnityEngine.Random.value * 2f;
				num2 = -1f + UnityEngine.Random.value * 2f;
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
				flag = true;
			}
			else
			{
				flag = false;
				if (num > 0.7f)
				{
					num -= num3;
				}
				else
				{
					num += -num3;
				}
				if (num2 > 0f)
				{
					num2 -= num3;
				}
				else
				{
					num2 += -num3;
				}
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
			}
			if (color == BloodColor.Green)
			{
				EffectsController.instance.GreenBloodParticlesSmall.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(EffectsController.instance.GreenBloodParticlesSmall.minSize, EffectsController.instance.GreenBloodParticlesSmall.maxSize, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.GreenBloodParticlesSmall.minEnergy, EffectsController.instance.GreenBloodParticlesSmall.maxEnergy, UnityEngine.Random.value), Color.white * (0.8f + UnityEngine.Random.value * 0.2f));
			}
			else
			{
				EffectsController.instance.RedBloodParticlesSmall.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(EffectsController.instance.RedBloodParticlesSmall.minSize, EffectsController.instance.RedBloodParticlesSmall.maxSize, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.RedBloodParticlesSmall.minEnergy, EffectsController.instance.RedBloodParticlesSmall.maxEnergy, UnityEngine.Random.value), Color.white * (0.8f + UnityEngine.Random.value * 0.2f));
			}
		}
	}

	public static void CreateBloodParticlesDots(BloodColor color, float x, float y, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI, float scaleM)
	{
		EffectsController.CreateBloodParticlesDots(color, x, y, 1f, count, offsetXRadius, offsetYRadius, force, xI, yI, scaleM);
	}

	public static void CreateBloodParticlesDots(BloodColor color, float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI, float scaleM)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 2f / force;
		bool flag = false;
		if (UnityEngine.Random.value > 0.5f)
		{
			count++;
		}
		for (int i = 0; i < count; i++)
		{
			float x2;
			float y2;
			if (!flag || UnityEngine.Random.value > 0.6f)
			{
				num = -1f + UnityEngine.Random.value * 2f;
				num2 = -1f + UnityEngine.Random.value * 2f;
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
				flag = true;
			}
			else
			{
				flag = false;
				if (num > 0f)
				{
					num -= num3;
				}
				else
				{
					num += num3;
				}
				if (num2 > 0f)
				{
					num2 -= num3;
				}
				else
				{
					num2 += num3;
				}
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
			}
			if ((EffectsController.instance == null || !EffectsController.instance.gameObject.activeInHierarchy) && CutsceneEffectsController.instance != null)
			{
				CutsceneEffectsController.instance.bloodParticlesDots.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesDots.minSize * scaleM, CutsceneEffectsController.instance.bloodParticlesDots.maxSize * scaleM, UnityEngine.Random.value), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesDots.minEnergy, CutsceneEffectsController.instance.bloodParticlesDots.maxEnergy, UnityEngine.Random.value), new Color(1f, 1f, 1f, 0.7f + UnityEngine.Random.value * 0.4f));
			}
			else if (color == BloodColor.Green)
			{
				EffectsController.instance.GreenBloodParticlesDots.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(EffectsController.instance.GreenBloodParticlesDots.minSize * scaleM, EffectsController.instance.GreenBloodParticlesDots.maxSize * scaleM, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.GreenBloodParticlesDots.minEnergy, EffectsController.instance.GreenBloodParticlesDots.maxEnergy, UnityEngine.Random.value), new Color(1f, 1f, 1f, 0.7f + UnityEngine.Random.value * 0.4f));
			}
			else
			{
				EffectsController.instance.RedBloodParticlesDots.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(EffectsController.instance.RedBloodParticlesDots.minSize * scaleM, EffectsController.instance.RedBloodParticlesDots.maxSize * scaleM, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.RedBloodParticlesDots.minEnergy, EffectsController.instance.RedBloodParticlesDots.maxEnergy, UnityEngine.Random.value), new Color(1f, 1f, 1f, 0.7f + UnityEngine.Random.value * 0.4f));
			}
		}
	}

	public static void CreateSemenParticlesSmall(BloodColor color, float x, float y, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		EffectsController.CreateBloodParticlesSmall(color, x, y, 1f, count, offsetXRadius, offsetYRadius, force, xI, yI);
	}

	public static void CreateSemenParticlesSmall(BloodColor color, float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 2f / force;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			float x2;
			float y2;
			if (!flag || UnityEngine.Random.value > 0.6f)
			{
				num = -1f + UnityEngine.Random.value * 2f;
				num2 = -1f + UnityEngine.Random.value * 2f;
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
				flag = true;
			}
			else
			{
				flag = false;
				if (num > 0.7f)
				{
					num -= num3;
				}
				else
				{
					num += -num3;
				}
				if (num2 > 0f)
				{
					num2 -= num3;
				}
				else
				{
					num2 += -num3;
				}
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
			}
			EffectsController.instance.semenParticlesSmall.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(EffectsController.instance.semenParticlesSmall.minSize, EffectsController.instance.semenParticlesSmall.maxSize, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.semenParticlesSmall.minEnergy, EffectsController.instance.semenParticlesSmall.maxEnergy, UnityEngine.Random.value), Color.white * (0.8f + UnityEngine.Random.value * 0.2f));
		}
	}

	public static void CreateSemenParticlesDots(BloodColor color, float x, float y, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI, float scaleM)
	{
		EffectsController.CreateBloodParticlesDots(color, x, y, 1f, count, offsetXRadius, offsetYRadius, force, xI, yI, scaleM);
	}

	public static void CreateSemenParticlesDots(BloodColor color, float x, float y, float z, int count, float offsetXRadius, float offsetYRadius, float force, float xI, float yI, float scaleM)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 2f / force;
		bool flag = false;
		if (UnityEngine.Random.value > 0.5f)
		{
			count++;
		}
		for (int i = 0; i < count; i++)
		{
			float x2;
			float y2;
			if (!flag || UnityEngine.Random.value > 0.6f)
			{
				num = -1f + UnityEngine.Random.value * 2f;
				num2 = -1f + UnityEngine.Random.value * 2f;
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
				flag = true;
			}
			else
			{
				flag = false;
				if (num > 0f)
				{
					num -= num3;
				}
				else
				{
					num += num3;
				}
				if (num2 > 0f)
				{
					num2 -= num3;
				}
				else
				{
					num2 += num3;
				}
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
			}
			if ((EffectsController.instance == null || !EffectsController.instance.gameObject.activeInHierarchy) && CutsceneEffectsController.instance != null)
			{
				CutsceneEffectsController.instance.bloodParticlesDots.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesDots.minSize * scaleM, CutsceneEffectsController.instance.bloodParticlesDots.maxSize * scaleM, UnityEngine.Random.value), Mathf.Lerp(CutsceneEffectsController.instance.bloodParticlesDots.minEnergy, CutsceneEffectsController.instance.bloodParticlesDots.maxEnergy, UnityEngine.Random.value), new Color(1f, 1f, 1f, 0.7f + UnityEngine.Random.value * 0.4f));
			}
			else
			{
				EffectsController.instance.semenParticlesDots.Emit(new Vector3(x2, y2, z), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(EffectsController.instance.semenParticlesDots.minSize * scaleM, EffectsController.instance.RedBloodParticlesDots.maxSize * scaleM, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.semenParticlesDots.minEnergy, EffectsController.instance.semenParticlesDots.maxEnergy, UnityEngine.Random.value), new Color(1f, 1f, 1f, 0.7f + UnityEngine.Random.value * 0.4f));
			}
		}
	}

	public static void CreateBloodParticlesDrip(float x, float y, float offsetXRadius, float offsetYRadius, float scaleM, BloodColor color)
	{
		float num = -1f + UnityEngine.Random.value * 2f;
		float num2 = -1f + UnityEngine.Random.value * 2f;
		float x2 = x + offsetXRadius * num;
		float y2 = y + offsetYRadius * num2;
		if (color == BloodColor.Green)
		{
			EffectsController.instance.bloodParticlesDripsGreen.Emit(new Vector3(x2, y2, 1f), Vector3.zero, Mathf.Lerp(EffectsController.instance.bloodParticlesDripsGreen.minSize * scaleM, EffectsController.instance.bloodParticlesDripsGreen.maxSize * scaleM, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.bloodParticlesDripsGreen.minEnergy, EffectsController.instance.bloodParticlesDripsGreen.maxEnergy, UnityEngine.Random.value), Color.white);
		}
		else
		{
			EffectsController.instance.bloodParticlesDripsRed.Emit(new Vector3(x2, y2, 1f), Vector3.zero, Mathf.Lerp(EffectsController.instance.bloodParticlesDripsRed.minSize * scaleM, EffectsController.instance.bloodParticlesDripsRed.maxSize * scaleM, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.bloodParticlesDripsRed.minEnergy, EffectsController.instance.bloodParticlesDripsRed.maxEnergy, UnityEngine.Random.value), Color.white);
		}
	}

	public static void CreateBubbles(float x, float y, float z, int count, float offsetXRadius, float offsetYRadius)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 10f;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			float x2;
			float y2;
			if (!flag || UnityEngine.Random.value > 0.6f)
			{
				num = -1f + UnityEngine.Random.value * 2f;
				num2 = -1f + UnityEngine.Random.value * 2f;
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
				flag = true;
			}
			else
			{
				flag = false;
				if (num > 0.7f)
				{
					num -= num3;
				}
				else
				{
					num += -num3;
				}
				if (num2 > 0f)
				{
					num2 -= num3;
				}
				else
				{
					num2 += -num3;
				}
				x2 = x + offsetXRadius * num;
				y2 = y + offsetYRadius * num2;
			}
			EffectsController.instance.bubbleParticles.Emit(new Vector3(x2, y2, z), new Vector3(0f, num3, 0f), Mathf.Lerp(EffectsController.instance.RedBloodParticlesSmall.minSize, EffectsController.instance.RedBloodParticlesSmall.maxSize, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.RedBloodParticlesSmall.minEnergy, EffectsController.instance.RedBloodParticlesSmall.maxEnergy, UnityEngine.Random.value), Color.white * (0.8f + UnityEngine.Random.value * 0.2f));
		}
	}

	public static void CreateGrenadeTrailEffect(float x, float y, float z, float maxLife, float velocity, float maxVelocity, float lagDistance, ref float previousX, ref float previousY, Color color, ref float lastTrailAlphaM)
	{
		float num = Mathf.Clamp(velocity / maxVelocity, 0f, 1f);
		if (num > 0.05f)
		{
			float to = maxLife * num;
			int num2 = (int)previousX;
			int num3 = (int)previousY;
			float num4 = x - previousX;
			float num5 = y - previousY;
			float f = Mathf.Sqrt(num4 * num4 + num5 * num5);
			int num6 = Mathf.CeilToInt(f);
			for (int i = 0; i < num6; i++)
			{
				float num7 = (float)i / (float)num6;
				int num8 = Mathf.RoundToInt((float)num2 + num4 * num7);
				int num9 = Mathf.RoundToInt((float)num3 + num5 * num7);
				if ((float)num8 != previousX || (float)num9 != previousY)
				{
					previousX = (float)num8;
					previousY = (float)num9;
					FaderSprite faderSprite = EffectsController.CreateEffect(EffectsController.instance.grenadeTrailDot, (float)num8, (float)num9, z);
					lastTrailAlphaM = Mathf.Lerp(lastTrailAlphaM, to, 0.07f);
					faderSprite.maxLife = lastTrailAlphaM;
					faderSprite.SetColor(color);
				}
			}
		}
	}

	public static void CreateRedWarningEffect(float x, float y, Transform parentedTransform)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.redWarningPulsePrefab, x, y, 0f, Vector3.zero, BloodColor.None);
		if (puff != null)
		{
			puff.transform.parent = parentedTransform;
		}
	}

	public static void CreateRedWarningDiamondHuge(float x, float y, Transform parentedTransform)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.redWarningDiamondHugePrefab, x, y, 0f, Vector3.zero, BloodColor.None);
		if (puff != null)
		{
			puff.transform.parent = parentedTransform;
		}
	}

	public static void CreateRedWarningDiamondLargege(float x, float y, Transform parentedTransform)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.redWarningDiamondLargePrefab, x, y, 0f, Vector3.zero, BloodColor.None);
		if (puff != null)
		{
			puff.transform.parent = parentedTransform;
		}
	}

	public static void CreateReviveZombieEffect(float x, float y, Transform parentedTransform)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.reviveZombiePrefab, x, y, 0f, Vector3.zero, BloodColor.None);
		if (puff != null)
		{
			puff.transform.parent = parentedTransform;
		}
	}

	public static void CreateRevivedZombiePassiveEffect(float x, float y, float z, Transform parentedTransform)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.revivedZombiePassivePrefab, x, y, z, Vector3.zero, BloodColor.None);
		if (puff != null)
		{
			puff.transform.parent = parentedTransform;
		}
	}

	public static void CreatePuffDisappearEffect(float x, float y, float xI, float yI)
	{
		EffectsController.CreateEffect(EffectsController.instance.puffDisappearPrefab, x, y, 0f, Vector3.zero, BloodColor.None);
		EffectsController.CreateEffect(EffectsController.instance.puffDisappearRingPrefab, x, y, 0.25f, Vector3.zero, BloodColor.None);
	}

	public static void CreateMuzzleFlashEffect(float x, float y, float z, float xI, float yI, Transform parent)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.muzzleFlashPrefab, x, y, z, new Vector3(xI, yI, 0f), BloodColor.None);
		puff.transform.parent = parent;
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashGlowPrefab, x, y, z);
	}

	public static void CreateMuzzleFlashMediumEffect(float x, float y, float z, float xI, float yI, Transform parent)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.muzzleFlashMediumPrefab, x, y, z, new Vector3(xI, yI, 0f), BloodColor.None);
		puff.transform.parent = parent;
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashGlowPrefab, x, y, z);
	}

	public static void CreateMuzzleFlashRoundEffect(float x, float y, float z, float xI, float yI, Transform parent)
	{
		Puff puff = EffectsController.CreateEffect(EffectsController.instance.muzzleFlashRoundPrefab, x, y, z, new Vector3(xI, yI, 0f), BloodColor.None);
		puff.transform.parent = parent;
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashGlowPrefab, x, y, z);
	}

	public static void CreateMuzzleFlashBigEffect(float x, float y, float z, float xI, float yI)
	{
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashBigPrefab, x, y, z, new Vector3(xI, yI, 0f), BloodColor.None);
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashBigGlowPrefab, x, y, z);
	}

	public static Puff CreatePuffDisappearRingEffect(float x, float y, float xI, float yI)
	{
		return EffectsController.CreateEffect(EffectsController.instance.puffDisappearRingPrefab, x, y, 0f, Vector3.zero, BloodColor.None);
	}

	public static Puff CreatePuffPeckShineEffect(float x, float y, float xI, float yI)
	{
		return EffectsController.CreateEffect(EffectsController.instance.puffPeckShinePrefab, x, y, 0f, Vector3.zero, BloodColor.None);
	}

	public static void CreateBloodSplashEffect(BloodColor bloodColor, float x, float y, float xI, float yI)
	{
		int num = UnityEngine.Random.Range(0, 2);
		if (num != 0)
		{
			if (num == 1)
			{
				EffectsController.CreateEffect(EffectsController.instance.bloodSplash2Prefab, x, y, 0f, Vector3.zero, bloodColor);
			}
		}
		else
		{
			EffectsController.CreateEffect(EffectsController.instance.bloodSplash1Prefab, x, y, 0f, Vector3.zero, bloodColor);
		}
	}

	public static void CreateMeleeStrikeEffect(float x, float y, float xI, float yI)
	{
		EffectsController.CreateEffect(EffectsController.instance.meleeStrikePrefab1, x, y, 0f, (xI >= 0f) ? 1 : -1, EffectsController.meleeStrikeYScale, BloodColor.None);
		EffectsController.meleeStrikeYScale *= -1;
	}

	public static void CreateBloodGushEffect(BloodColor color, float x, float y, float xI, float yI)
	{
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 0:
			EffectsController.CreateEffect(EffectsController.instance.bloodGush1Prefab, x, y, 0f, (xI >= 0f) ? 1 : -1, 1, color);
			break;
		case 1:
			EffectsController.CreateEffect(EffectsController.instance.bloodGush2Prefab, x, y, 0f, (xI >= 0f) ? 1 : -1, 1, color);
			break;
		case 2:
			EffectsController.CreateEffect(EffectsController.instance.bloodGush3Prefab, x, y, 0f, (xI >= 0f) ? 1 : -1, 1, color);
			break;
		}
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 0:
			EffectsController.CreateEffect(EffectsController.instance.bloodSpurt1Prefab, x, y, 0f, (xI >= 0f) ? 1 : -1, 1, color);
			break;
		case 1:
			EffectsController.CreateEffect(EffectsController.instance.bloodSpurt2Prefab, x, y, 0f, (xI >= 0f) ? 1 : -1, 1, color);
			break;
		case 2:
			EffectsController.CreateEffect(EffectsController.instance.bloodSpurt3Prefab, x, y, 0f, (xI >= 0f) ? 1 : -1, 1, color);
			break;
		}
		EffectsController.CreateBloodParticlesDots(color, x, y, 3 + UnityEngine.Random.Range(0, 3), 4f, 5f, 90f, -xI * 0.5f, yI + 40f, 1f);
	}

	public static void CreateBloodSmallSplashEffect(BloodColor color, float x, float y, float xI, float yI)
	{
		int num = UnityEngine.Random.Range(0, 2);
		if (num != 0)
		{
			if (num == 1)
			{
				EffectsController.CreateEffect(EffectsController.instance.bloodSmallSplash2Prefab, x, y, 0f, Vector3.zero, color);
			}
		}
		else
		{
			EffectsController.CreateEffect(EffectsController.instance.bloodSmallSplash1Prefab, x, y, 0f, Vector3.zero, color);
		}
	}

	public static void CreateFlameEffect(float x, float y, float delay, Vector3 velocity)
	{
		EffectsController.CreateFlameParticle(x, y);
		switch (UnityEngine.Random.Range(0, 12))
		{
		case 0:
			EffectsController.CreateFireSparks(x, y, 1, 10f, 7f, 0f, 15f, 0.65f);
			break;
		case 1:
			EffectsController.CreateFireSparks(x, y, 1, 10f, 7f, 0f, 15f, 0.65f);
			break;
		}
	}

	public static FlickerFader CreateEffect(FlickerFader flickerFaderPrefab, float x, float y, float delay, Vector3 velocity)
	{
		FlickerFader flickerFader = UnityEngine.Object.Instantiate(flickerFaderPrefab, new Vector3(x, y, 0f), Quaternion.identity) as FlickerFader;
		if (velocity != Vector3.zero)
		{
			velocity.z = 0f;
			flickerFader.SetVelocity(velocity);
		}
		if (delay > 0f)
		{
			flickerFader.Delay(delay);
		}
		return flickerFader;
	}

	public static void CreateEffect(FlickerFader flickerFaderPrefab, float x, float y, float z, float delay, Vector3 velocity)
	{
		FlickerFader flickerFader = UnityEngine.Object.Instantiate(flickerFaderPrefab, new Vector3(x, y, z), Quaternion.identity) as FlickerFader;
		if (velocity != Vector3.zero)
		{
			velocity.z = 0f;
			flickerFader.SetVelocity(velocity);
		}
		if (delay > 0f)
		{
			flickerFader.Delay(delay);
		}
	}

	public static DistortionGrow CreateEffect(DistortionGrow distortionPrefab, float x, float y, float delay)
	{
		DistortionGrow distortionGrow = UnityEngine.Object.Instantiate(distortionPrefab, new Vector3(x, y, 0f), Quaternion.identity) as DistortionGrow;
		if (delay > 0f)
		{
			distortionGrow.delay = delay;
		}
		return distortionGrow;
	}

	public static Puff CreateEffect(Puff puffPrefab, float x, float y, float delay, Vector3 velocity, BloodColor bloodColor = BloodColor.None)
	{
		Puff puff = UnityEngine.Object.Instantiate(puffPrefab, new Vector3(x, y, 0f), Quaternion.identity) as Puff;
		if (velocity != Vector3.zero)
		{
			velocity.z = 0f;
			puff.SetVelocity(velocity);
		}
		if (delay > 0f)
		{
			puff.Delay(delay);
		}
		if (bloodColor == BloodColor.Red)
		{
			puff.SetColor(EffectsController.inst.RedBloodColor);
		}
		else if (bloodColor == BloodColor.Green)
		{
			puff.SetColor(EffectsController.inst.GreenBloodColor);
		}
		return puff;
	}

	public static PuffTwoLayer CreateEffect(PuffTwoLayer puffPrefab, float x, float y, float delay, Vector3 velocity, BloodColor bloodColor = BloodColor.None)
	{
		PuffTwoLayer puffTwoLayer = UnityEngine.Object.Instantiate(puffPrefab, new Vector3(x, y, 0f), Quaternion.identity) as PuffTwoLayer;
		if (velocity != Vector3.zero)
		{
			velocity.z = 0f;
			puffTwoLayer.SetVelocity(velocity);
		}
		if (delay > 0f)
		{
			puffTwoLayer.Delay(delay);
		}
		if (bloodColor == BloodColor.Red)
		{
			puffTwoLayer.SetColor(EffectsController.inst.RedBloodColor);
		}
		else if (bloodColor == BloodColor.Green)
		{
			puffTwoLayer.SetColor(EffectsController.inst.GreenBloodColor);
		}
		return puffTwoLayer;
	}

	public static FaderSprite CreateEffect(FaderSprite prefab, float x, float y, float z)
	{
		return UnityEngine.Object.Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity) as FaderSprite;
	}

	public static Puff CreateEffect(Puff puffPrefab, float x, float y, float z, float delay, Vector3 velocity)
	{
		Puff puff = UnityEngine.Object.Instantiate(puffPrefab, new Vector3(x, y, z), Quaternion.identity) as Puff;
		if (velocity != Vector3.zero)
		{
			velocity.z = 0f;
			puff.SetVelocity(velocity);
		}
		if (delay > 0f)
		{
			puff.Delay(delay);
		}
		return puff;
	}

	public static void CreateProjectilePuff(float x, float y)
	{
		EffectsController.CreateEffect(EffectsController.instance.projectileFlickPuff, x, y);
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashSmallGlowPrefab, x, y, 0f);
	}

	public static void CreateGlassShards(float x, float y, int count, float offsetXRadius, float offsetYRadius, float disturbRange, float force, float xI, float yI, float shakeM, float shakeSpeedM, float volume)
	{
		for (int i = 0; i < count; i++)
		{
			float num = -1f + UnityEngine.Random.value * 2f;
			float num2 = -1f + UnityEngine.Random.value * 2f;
			float x2 = x + offsetXRadius * num;
			float y2 = y + offsetYRadius * num2;
			EffectsController.instance.particlesGlassShards.Emit(new Vector3(x2, y2, 1f), new Vector3(xI + force * num, yI + force * num2, 0f), Mathf.Lerp(EffectsController.instance.particlesGlassShards.minSize, EffectsController.instance.particlesGlassShards.maxSize, UnityEngine.Random.value), Mathf.Lerp(EffectsController.instance.particlesGlassShards.minEnergy, EffectsController.instance.particlesGlassShards.maxEnergy, UnityEngine.Random.value), Color.white);
		}
		if (shakeM > 0f)
		{
			SortOfFollow.Shake(shakeM, shakeSpeedM, new Vector3(x, y, 0f));
		}
		EffectsController.PlayGlassShatterSound(x, y, volume);
		if (disturbRange > 0f)
		{
			Map.DisturbWildLife(x, y, disturbRange, 5);
		}
	}

	public static void CreateLeafBurst(float x, float y, float z, int count, float range, float xI, float yI, float force)
	{
		EffectsController.CreateShrapnel(EffectsController.instance.leaf1Prefab, x, y, range, force, (float)(count / 2), xI, yI);
		EffectsController.CreateShrapnel(EffectsController.instance.leaf2Prefab, x, y, range, force, (float)(count / 2 + 1), xI, yI);
	}

	public static void CreateSmallExplosion(float x, float y, float z, float shakeM, float volume)
	{
		switch (UnityEngine.Random.Range(0, 3))
		{
		case 0:
			EffectsController.CreateEffect(EffectsController.instance.explosion, x, y, z, 0f, Vector3.zero);
			break;
		case 1:
			EffectsController.CreateEffect(EffectsController.instance.explosionBig, x, y, z, 0f, Vector3.zero);
			break;
		case 2:
			EffectsController.CreateEffect(EffectsController.instance.explosion, x, y, z, 0f, Vector3.zero);
			break;
		}
		Vector3 vector = UnityEngine.Random.insideUnitCircle;
		vector.z = 0f;
		switch (UnityEngine.Random.Range(0, 5))
		{
		case 1:
			EffectsController.CreateEffect(EffectsController.instance.flame1Prefab, x + vector.x * 5f, y + vector.y * 5f, z, 0f + UnityEngine.Random.value * 0.3f, vector);
			EffectsController.CreateEffect(EffectsController.instance.flame2Prefab, x - vector.x * 5f, y - vector.y * 3f, z, 0f + UnityEngine.Random.value * 0.3f, -vector);
			break;
		case 2:
			EffectsController.CreateEffect(EffectsController.instance.flame3Prefab, x + vector.x * 6f, y + vector.y * 6f, z, 0f + UnityEngine.Random.value * 0.3f, vector);
			break;
		case 3:
		case 4:
			EffectsController.CreateEffect(EffectsController.instance.flame1Prefab, x + vector.x * 5f, y + vector.y * 5f, z, 0f + UnityEngine.Random.value * 0.4f, vector);
			EffectsController.CreateEffect(EffectsController.instance.flame2Prefab, x - vector.x * 5f, y - vector.y * 3f, z, 0f + UnityEngine.Random.value * 0.4f, -vector);
			EffectsController.CreateEffect(EffectsController.instance.flame3Prefab, x, y + vector.y * 6f, z, 0f + UnityEngine.Random.value * 0.5f, vector.y * Vector3.up);
			break;
		}
		SortOfFollow.Shake(shakeM, new Vector3(x, y, 0f));
		if (volume > 0f)
		{
			EffectsController.PlayExplosionSound(x, y, volume);
		}
	}

	public static void CreateExplosion(float x, float y, float offsetXRadius, float offsetYRadius, float disturbRange, float delayM, float puffSpeed, float shakeM, float volume, bool groundWave)
	{
		EffectsController.CreateExplosion(x, y, offsetXRadius, offsetYRadius, disturbRange, delayM, puffSpeed, shakeM, volume, groundWave, true);
	}

	public static void CreateExplosion(float x, float y, float offsetXRadius, float offsetYRadius, float disturbRange, float delayM, float puffSpeed, float shakeM, float volume, bool groundWave, bool complexExplosion)
	{
		EffectsController.CreateSparkShower(x, y, 40, (offsetXRadius + offsetYRadius) / 3f, 200f, 0f, 250f, 0.65f, 0.25f);
		Vector3 a = UnityEngine.Random.insideUnitCircle;
		if (complexExplosion)
		{
			EffectsController.CreateEffect(EffectsController.instance.smokeSmall1Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, UnityEngine.Random.value * 0.5f, a * puffSpeed, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(EffectsController.instance.smokeSmall1Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, UnityEngine.Random.value * 0.5f, a * puffSpeed, BloodColor.None);
			EffectsController.CreateEffect(EffectsController.instance.smokeSmall1Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, UnityEngine.Random.value * 0.2f, UnityEngine.Random.insideUnitSphere * puffSpeed, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(EffectsController.instance.smokeSmall2Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, UnityEngine.Random.value * 0.5f, a * puffSpeed, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(EffectsController.instance.smokeSmall2Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, UnityEngine.Random.value * 0.5f, a * puffSpeed, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(EffectsController.instance.smokeSmall3Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, UnityEngine.Random.value * 0.2f, a * puffSpeed, BloodColor.None);
		}
		if (complexExplosion)
		{
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(EffectsController.instance.explosion, x + a.x * offsetXRadius, y + a.y * offsetYRadius, UnityEngine.Random.value * 0.5f, a * puffSpeed, BloodColor.None);
			a = UnityEngine.Random.insideUnitCircle;
			EffectsController.CreateEffect(EffectsController.instance.explosionBig, x + a.x * offsetXRadius, y + a.y * offsetYRadius, UnityEngine.Random.value * 0.5f, a * puffSpeed * 0.5f, BloodColor.None);
		}
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.explosion, x + a.x * offsetXRadius, y + a.y * offsetYRadius, UnityEngine.Random.value * 0.2f, a * puffSpeed * 0.5f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.explosionBig, x + a.x * offsetXRadius, y + a.y * offsetYRadius, 0f, a * puffSpeed * 0.3f, BloodColor.None);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.flame1Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, 0.1f + UnityEngine.Random.value * 0.2f, a * puffSpeed * 0.5f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.flame2Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, 0.1f + UnityEngine.Random.value * 0.2f, a * puffSpeed * 0.5f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.flame3Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, 0.1f + UnityEngine.Random.value * 0.2f, a * puffSpeed * 0.5f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.flame2Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, 0.1f + UnityEngine.Random.value * 0.2f, a * puffSpeed * 0.5f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.flame1Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, 0.1f + UnityEngine.Random.value * 0.2f, a * puffSpeed * 0.5f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.flame3Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, 0.1f + UnityEngine.Random.value * 0.2f, a * puffSpeed * 0.5f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.flame2Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, 0.25f + UnityEngine.Random.value * 0.2f, a * puffSpeed * 0.5f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.flame1Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, 0.25f + UnityEngine.Random.value * 0.2f, a * puffSpeed * 0.5f);
		a = UnityEngine.Random.insideUnitCircle;
		EffectsController.CreateEffect(EffectsController.instance.flame3Prefab, x + a.x * offsetXRadius, y + a.y * offsetYRadius, 0.25f + UnityEngine.Random.value * 0.2f, a * puffSpeed * 0.5f);
		if (groundWave)
		{
			ExplosionGroundWave explosionGroundWave = UnityEngine.Object.Instantiate(EffectsController.instance.explosionGroundWavePrefab, new Vector3(x, y, 0f), Quaternion.identity) as ExplosionGroundWave;
			explosionGroundWave.range = 96f;
		}
		Map.ShakeTrees(x, y, 256f, 64f, 128f);
		if (shakeM > 0f)
		{
			SortOfFollow.Shake(shakeM, new Vector3(x, y, 0f));
		}
		if (volume > 0f)
		{
			EffectsController.PlayExplosionSound(x, y, volume);
		}
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashBigGlowPrefab, x, y, 0f);
		Map.DisturbWildLife(x, y, disturbRange, 5);
	}

	public static void CreateExplosionInRectangle(Vector3 bottonLeft, int widthInBlocks, int heightInBlocks, float randomOffset, float delayM, float puffSpeed, float xi, float yi, float shakeM, float volume, bool groundWave, bool debugDraw)
	{
		if (debugDraw)
		{
			Vector3 origin = bottonLeft + new Vector3((float)widthInBlocks, (float)heightInBlocks) * 16f / 2f - new Vector3(8f, 8f, 0f);
			Extensions.DrawRect(origin, (float)(widthInBlocks * 16), (float)(heightInBlocks * 16), Color.green, 10f);
		}
		Vector2 a = Vector2.zero;
		for (int i = 0; i < widthInBlocks; i++)
		{
			for (int j = 0; j < heightInBlocks; j++)
			{
				a = bottonLeft;
				a.x += (float)(i * 16);
				a.y += (float)(j * 16);
				a += UnityEngine.Random.insideUnitCircle * randomOffset;
				EffectsController.CreateEffect(EffectsController.instance.explosion, a.x, a.y, UnityEngine.Random.value * 0.2f, UnityEngine.Random.insideUnitCircle * puffSpeed * 0.5f + new Vector2(xi, yi), BloodColor.None);
			}
		}
	}

	public static void CreateGroundWave(float x, float y, float range)
	{
		ExplosionGroundWave explosionGroundWave = UnityEngine.Object.Instantiate(EffectsController.instance.explosionGroundWavePrefab, new Vector3(x, y, 0f), Quaternion.identity) as ExplosionGroundWave;
		explosionGroundWave.range = range;
	}

	public static ExplosionGroundWave CreateShockWave(float x, float y, float range)
	{
		ExplosionGroundWave explosionGroundWave = UnityEngine.Object.Instantiate(EffectsController.instance.explosionShockWavePrefab, new Vector3(x, y, 0f), Quaternion.identity) as ExplosionGroundWave;
		explosionGroundWave.range = range;
		return explosionGroundWave;
	}

	public static ExplosionGroundWave CreateHugeShockWave(float x, float y, float range)
	{
		ExplosionGroundWave explosionGroundWave = UnityEngine.Object.Instantiate(EffectsController.instance.explosionShockWaveHugePrefab, new Vector3(x, y, 0f), Quaternion.identity) as ExplosionGroundWave;
		explosionGroundWave.range = range;
		return explosionGroundWave;
	}

	public static void CreateHugeExplosion(float x, float y, float offsetXRadius, float offsetYRadius, float disturbRange, float delayM, float puffSpeed, float shakeM, float volume, int extraExplosionsCount, int shrapnelCount, float shrapnelForce, float groundWaveDistance, float lowPassFrequencyLossM, float flashEffectM)
	{
		EffectsController.CreateEffect(EffectsController.instance.explosionHuge, x, y, 0f, Vector3.zero, BloodColor.None);
		if (extraExplosionsCount > 0)
		{
			for (int i = 0; i < extraExplosionsCount; i++)
			{
				Vector3 a = UnityEngine.Random.insideUnitCircle;
				EffectsController.CreateEffect(EffectsController.instance.explosion, x + a.x * offsetXRadius, y + a.y * offsetYRadius, UnityEngine.Random.value * 0.5f, a * puffSpeed, BloodColor.None);
			}
		}
		EffectsController.CreateSparkShower(x, y, shrapnelCount, (offsetXRadius + offsetYRadius) / 3f, shrapnelForce, 0f, shrapnelForce, 0.65f, 0.25f);
		SortOfFollow.Shake(shakeM, new Vector3(x, y, 0f));
		if (lowPassFrequencyLossM > 0f)
		{
			Sound.SuddenLowPass(lowPassFrequencyLossM, new Vector3(x, y, 0f));
		}
		if (volume > 0f)
		{
			EffectsController.PlayExplosionHugeSound(x, y, volume, lowPassFrequencyLossM > 0f);
		}
		if (disturbRange > 0f)
		{
			Map.DisturbWildLife(x, y, disturbRange, 5);
		}
		if (groundWaveDistance > 16f)
		{
			ExplosionGroundWave explosionGroundWave = UnityEngine.Object.Instantiate(EffectsController.instance.explosionGroundWavePrefab, new Vector3(x, y, 0f), Quaternion.identity) as ExplosionGroundWave;
			explosionGroundWave.range = groundWaveDistance;
		}
		if (flashEffectM > 0f)
		{
			FullScreenFlashEffect.FlashHot(flashEffectM, new Vector3(x, y, 0f));
		}
		EffectsController.CreateEffect(EffectsController.instance.muzzleFlashHugeGlowPrefab, x, y, 0f);
	}

	protected static void PlayExplosionHugeSound(float x, float y, float volume, bool ignoreFilters)
	{
		Sound instance = Sound.GetInstance();
		instance.PlaySoundEffectAt(EffectsController.instance.explosionHugeSoundHolder.deathSounds, volume, new Vector3(x, y, 0f), 1f, true, ignoreFilters);
	}

	protected static void PlayExplosionSound(float x, float y, float volume)
	{
		Sound instance = Sound.GetInstance();
		instance.PlaySoundEffectAt(EffectsController.instance.explosionSoundHolder.deathSounds, volume, new Vector3(x, y, 0f));
	}

	protected static void PlayGlassShatterSound(float x, float y, float volume)
	{
		Sound instance = Sound.GetInstance();
		instance.PlaySoundEffectAt(EffectsController.instance.glassSoundHolder.deathSounds, volume, new Vector3(x, y, 0f));
	}

	public static Transform GetBloodOverlay()
	{
        SpriteSM sm = UnityEngine.Object.Instantiate(EffectsController.instance.bloodBlockOverlayPrefabs[UnityEngine.Random.Range(0, EffectsController.instance.bloodBlockOverlayPrefabs.Length)]);
        return sm.GetComponent<Transform>();
		//return UnityEngine.Object.Instantiate(EffectsController.instance.bloodBlockOverlayPrefabs[UnityEngine.Random.Range(0, EffectsController.instance.bloodBlockOverlayPrefabs.Length)]) as Transform;
	}

	public static void AttachLight(Unit unit)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(EffectsController.instance.lightObject, unit.transform.position + Vector3.up * 6f, Quaternion.identity) as GameObject;
		gameObject.transform.parent = unit.transform;
	}

	public Color RedBloodColor = Color.red;

	public Color GreenBloodColor = Color.green;

	public Shrapnel shrapnelStonePrefab;

	public FlickerFader flame1Prefab;

	public FlickerFader flame2Prefab;

	public FlickerFader flame3Prefab;

	public Shrapnel fireSpark1Prefab;

	public Shrapnel fireSpark2Prefab;

	public Shrapnel leaf1Prefab;

	public Shrapnel leaf2Prefab;

	public Puff flameCandle1Prefab;

	public Puff flameCandle2Prefab;

	public Puff flameCandle3Prefab;

	public Puff flameCandle4Prefab;

	public ParticleEmitter RedBloodParticlesBig;

	public ParticleEmitter RedBloodParticlesSmall;

	public ParticleEmitter RedBloodParticlesDots;

	public ParticleEmitter GreenBloodParticlesBig;

	public ParticleEmitter GreenBloodParticlesSmall;

	public ParticleEmitter GreenBloodParticlesDots;

	public ParticleEmitter bloodParticlesDripsRed;

	public ParticleEmitter bloodParticlesDripsGreen;

	public ParticleEmitter semenParticlesDots;

	public ParticleEmitter semenParticlesSmall;

	public ParticleEmitter sparkParticleShower;

	public ParticleEmitter sparkParticleFloat;

	public ParticleEmitter sparkParticleFire;

	public ParticleEmitter plumePuffParticle1;

	public ParticleEmitter plumePuffParticle2;

	public ParticleEmitter plumePuffBigParticle1;

	public ParticleEmitter plumePuffBigParticle2;

	public ParticleEmitter flameSheetParticle1;

	public ParticleEmitter flameSheetParticle2;

	public ParticleEmitter flameSheetParticle3;

	public ParticleEmitter flameSheetParticle4;

	public ParticleEmitter bubbleParticles;

	public ParticleEmitter flameSheetBackgroundParticle1;

	public ParticleEmitter flameSheetBackgroundParticle2;

	public ParticleEmitter flameSheetBackgroundParticle3;

	public ParticleEmitter flameSheetBackgroundParticle4;

	public ParticleEmitter plumePuffParticleBlack1;

	public ParticleEmitter plumePuffParticleBlack2;

	public ParticleEmitter plumePuffParticlePitchBlack;

	public Shrapnel plumeShrapnelPrefab;

	public ParticleEmitter dustParticles;

	public Gib blankGibPrefab;

	public DoodadPiece blankDoodadGibPrefab;

	public DoodadPiece fileCabinetGibPrefab;

	public DoodadPiece metalDoodadGibPrefab;

	public Puff puffDisappearPrefab;

	public Puff puffDisappearRingPrefab;

	public Puff puffPeckShinePrefab;

	public Puff muzzleFlashPrefab;

	public Puff muzzleFlashMediumPrefab;

	public Puff muzzleFlashBigPrefab;

	public Puff muzzleFlashRoundPrefab;

	public DistortionGrow muzzleFlashGlowPrefab;

	public DistortionGrow muzzleFlashBigGlowPrefab;

	public DistortionGrow muzzleFlashHugeGlowPrefab;

	public DistortionGrow muzzleFlashSmallGlowPrefab;

	public FaderSprite grenadeTrailDot;

	public Puff bloodSplash1Prefab;

	public Puff bloodSplash2Prefab;

	public Puff bloodSmallSplash1Prefab;

	public Puff bloodSmallSplash2Prefab;

	public Puff bloodGush1Prefab;

	public Puff bloodGush2Prefab;

	public Puff bloodGush3Prefab;

	public Puff bloodSpurt1Prefab;

	public Puff bloodSpurt2Prefab;

	public Puff bloodSpurt3Prefab;

	public Puff bloodGushGreen1Prefab;

	public Puff bloodGushGreen2Prefab;

	public Puff bloodGushGreen3Prefab;

	public Puff meleeStrikePrefab1;

	public Puff smokeSmall1Prefab;

	public Puff smokeSmall2Prefab;

	public Puff smokeSmall3Prefab;

	public PuffTwoLayer stunnedStars1Prefab;

	public PuffTwoLayer stunnedStars2Prefab;

	public PuffTwoLayer stunnedStars3Prefab;

	protected static int stunnedStarsCount;

	public Puff redWarningPulsePrefab;

	public Puff redWarningDiamondLargePrefab;

	public Puff redWarningDiamondHugePrefab;

	public Puff whiteFlashPopPrefab;

	public Puff whiteFlashPopSmallPrefab;

	public Puff whitePunchPopPrefab;

	public Puff airDashPoofPrefab;

	public Puff reviveZombiePrefab;

	public Puff revivedZombiePassivePrefab;

	public DistortionGrow distortionWobbleGrowPrefab;

	public DistortionGrow distortionPinchGrowPrefab;

	public Puff footPoof1Prefab;

	public Puff footPoof2Prefab;

	public Puff footPoof3Prefab;

	public Puff jumpPoof1Prefab;

	public Puff jumpPoof2Prefab;

	public Puff landPoof1Prefab;

	public Puff landPoof2Prefab;

	public Puff dashPoof1Prefab;

	public Puff groundExplodePoofPrefab;

	public Puff shockWaveExplodePoofPrefab;

	public FlickerFader bulletPoofPrefab;

	public Shrapnel explosionSparkShrapnel;

	public Puff explosion;

	public Puff explosionBig;

	public Puff explosionHuge;

	public SoundHolder explosionSoundHolder;

	public SoundHolder explosionHugeSoundHolder;

	public ExplosionGroundWave explosionGroundWavePrefab;

	public ExplosionGroundWave explosionShockWavePrefab;

	public ExplosionGroundWave explosionShockWaveHugePrefab;

	public ReactionBubble freeLifeBubble;

	public ReactionBubble ammoBubble;

	public ReactionBubble kickBubble;

	public Material kickdDefaultMaterial;

	public Material kickConfirmMaterial;

	public Material kickConfirmedMaterial;

	public ParticleEmitter particlesGlassShards;

	public SoundHolder glassSoundHolder;

	public FlickerFader projectileFlickPuff;

	public FlickerFader projectileWhiteFlickPuff;

	public FlickerFader projectileLargeFlickPuff;

	protected LightningController lightningController;

	public FullScreenFlashEffect flashEffect;

	protected bool rainAndLightning;

	public Transform rainHolder;

	public Transform fogObject;

	public Transform sunObject;

	public bool fireAndAsh;

	public Transform ashHolder;

	protected Transform rainFollowTransform;

	protected static LayerMask groundLayer;

	public Shrapnel backgroundWoodShrapnel1;

	public Shrapnel backgroundWoodShrapnel2;

	public Shrapnel backgroundWoodShrapnel3;

	public Shrapnel backgroundWoodShrapnel4;

	public GameObject lightObject;

	public VictoryFireWork[] fireWorks;

	protected float fireWorksCounter;

	protected int fireWorksCount;

	public SpriteSM[] bloodBlockOverlayPrefabs;

	private static EffectsController inst;

	protected static Vector3 tempOffset;

	protected static int meleeStrikeYScale = 1;
}
