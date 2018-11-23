// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class MinibossEndCheck : MonoBehaviour
{
	private void Start()
	{
		this.miniBossUnit = base.GetComponent<Unit>();
	}

	private void Update()
	{
		if (this.miniBossUnit.health <= 0)
		{
			if (this.isAlive)
			{
				this.isAlive = false;
				HeroController.SetAllHeroesInvulnerable(30f);
			}
			if (this.explosionDeathCount > 0)
			{
				SortOfFollow.ControlledByTriggerAction = true;
				SortOfFollow.followPos = Vector3.Lerp(SortOfFollow.followPos, base.transform.position + Vector3.up * 24f, Time.deltaTime * 7f);
				SortOfFollow.SetZoomLevel(SortOfFollow.zoomLevel);
				this.explosionDeathCounter += Time.deltaTime;
				if (this.explosionDeathCounter > 0.33f)
				{
					this.explosionDeathCounter -= 0.26f - UnityEngine.Random.value * 0.12f;
					this.explosionDeathCount--;
					EffectsController.CreateExplosion(base.transform.position.x, base.transform.position.y + 36f, 24f, 24f, 100f, 1f, 30f, 0.5f, 0.4f, false);
					if (this.explosionDeathCount <= 0)
					{
						GameModeController.LevelFinish(LevelResult.Success);
					}
				}
			}
			else
			{
				SortOfFollow.ControlledByTriggerAction = true;
				SortOfFollow.followPos += Vector3.up * Time.deltaTime * 60f;
				SortOfFollow.SetZoomLevel(SortOfFollow.zoomLevel);
			}
		}
	}

	protected float explosionDeathCounter;

	protected int explosionDeathCount = 15;

	protected Unit miniBossUnit;

	protected bool isAlive = true;
}
