// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class DamageableScenery : MonoBehaviour
{
	private void Start()
	{
		Map.RegisterDamageableScenerye(this);
		this.x = base.transform.position.x;
		this.y = base.transform.position.y;
	}

	public bool Knock(float xI, float yI)
	{
		if (Time.time - this.lastHitTime > this.hitDelay)
		{
			if (this.chanceToActuallyShake > UnityEngine.Random.value)
			{
				float num = Mathf.Abs(xI) + Mathf.Abs(yI);
				if (this.affectedFolliage != null)
				{
					this.affectedFolliage.Shake(xI / num, yI / num, num * this.speedAbsorbM, 1f);
				}
			}
			this.lastHitTime = Time.time + UnityEngine.Random.value * this.randomExtraDelay;
			return true;
		}
		return false;
	}

	public TreeFoliage affectedFolliage;

	public float x;

	public float y;

	public float xRadius = 32f;

	public float yRadius = 32f;

	public float speedAbsorbM = 0.2f;

	protected float lastHitTime;

	public float hitDelay = 0.33f;

	public float randomExtraDelay = 0.3f;

	public float chanceToActuallyShake = 0.333f;
}
