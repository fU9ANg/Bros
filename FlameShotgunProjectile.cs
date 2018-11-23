// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

public class FlameShotgunProjectile : Projectile
{
	protected override void Start()
	{
		base.Start();
		this.direction = (int)Mathf.Sign(this.xI);
		this.lastPuffPos = base.transform.position;
		EffectsController.CreateEffect(this.flamePuffs[this.random.Range(0, this.flamePuffs.Length)], this.x, this.y + 6f, 0f, 0f, (int)Mathf.Sign(this.xI), 1, new Vector3(this.xI * 0.2f, 0f, -0.1f));
	}

	protected override void Update()
	{
		base.Update();
		if (Vector3.Distance(this.lastPuffPos, base.transform.position) > 8f)
		{
			EffectsController.CreateEffect(this.flamePuffs[this.random.Range(0, this.flamePuffs.Length)], this.x, this.y + 6f + (float)this.random.Range(-4, 4), 0f, 0f, (int)Mathf.Sign(this.xI), 1, new Vector3(this.xI * 0.2f, (float)UnityEngine.Random.Range(-50, 50), -0.1f));
			this.lastPuffPos = base.transform.position;
		}
	}

	protected override void MakeEffects(bool particles, float x, float y, bool useRayCast, Vector3 hitNormal, Vector3 hitPoint)
	{
	}

	public Puff[] flamePuffs;

	private Vector3 lastPuffPos;

	private int direction;
}
