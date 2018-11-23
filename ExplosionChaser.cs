// dnSpy decompiler from Assembly-CSharp.dll
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionChaser : MonoBehaviour
{
	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteSM>();
	}

	private void Start()
	{
		this.x = base.transform.position.x;
	}

	private void Update()
	{
		if (Map.isEditing)
		{
			return;
		}
		if (this.delay > 0f)
		{
			this.delay -= Time.deltaTime;
			return;
		}
		if (this.x < (SortOfFollow.minX + SortOfFollow.maxX) / 2f)
		{
			this.x += this.speed * Time.deltaTime;
		}
		else
		{
			this.x += this.speed * Time.deltaTime * 0.7f;
		}
		this.spriteSin += Time.deltaTime;
		this.sprite.SetColor(new Color(0.7f, 0f, 0f, 0.2f + Mathf.Sin(this.spriteSin * 16f) * 0.1f));
		if (this.x > 16f)
		{
			if (this.x > SortOfFollow.maxX - 16f)
			{
				this.x = SortOfFollow.maxX - 16f;
			}
			this.explosionCounter += Time.deltaTime;
			if (this.explosionCounter > 0.5f)
			{
				this.explosionCounter -= 0.4f + 0.2f * UnityEngine.Random.value;
				float eY = this.FindGroundY(this.x);
				this.CreateExplosion(this.x, eY);
			}
			if (this.x > SortOfFollow.minX + 80f)
			{
				this.explosionCounter2 += Time.deltaTime;
				if (this.explosionCounter2 > 0.5f)
				{
					this.explosionCounter2 -= 0.333f + 0.333f * UnityEngine.Random.value;
					float eY2 = this.FindGroundY(this.x - 64f);
					this.CreateExplosion(this.x - 64f, eY2);
				}
			}
			if (this.x > SortOfFollow.minX + 144f)
			{
				this.explosionCounter3 += Time.deltaTime;
				if (this.explosionCounter3 > 0.5f)
				{
					this.explosionCounter3 -= 0.4f + 0.1f * UnityEngine.Random.value;
					float eY3 = this.FindGroundY(this.x - 128f);
					this.CreateExplosion(this.x - 128f, eY3);
				}
			}
			if (this.x > SortOfFollow.minX + 212f)
			{
				this.explosionCounter4 += Time.deltaTime;
				if (this.explosionCounter4 > 1f)
				{
					this.explosionCounter4 -= 0.8f + 0.4f * UnityEngine.Random.value;
					float eY4 = this.FindGroundY(this.x - 192f);
					this.CreateExplosion(this.x - 192f, eY4);
				}
			}
		}
		base.transform.position = new Vector3(this.x, this.lastExplosionY, 0f);
	}

	protected void CreateExplosion(float eX, float eY)
	{
		this.lastExplosionY = eY;
		if (UnityEngine.Random.value > 0.25f)
		{
			Map.HitUnits(this, 5, DamageType.Explosion, 24f, eX, eY, 0f, 140f, true, true);
			MapController.DamageGround(this, 15, DamageType.Explosion, 24f, eX, eY, null);
			EffectsController.CreateExplosion(eX, eY, 16f, 16f, 120f, 1f, 150f, 0.6f, 0.3f, false);
		}
		else
		{
			Map.HitUnits(this, 5, DamageType.Explosion, 40f, eX, eY, 0f, 140f, true, true);
			MapController.DamageGround(this, 15, DamageType.Explosion, 40f, eX, eY, null);
			EffectsController.CreateHugeExplosion(eX, eY, 40f, 40f, 200f, 1f, 200f, 1f, 0.6f, 6, 90, 270f, 0f, 0f, 0.5f);
		}
	}

	protected float FindGroundY(float explosionX)
	{
		List<float> list = new List<float>();
		for (float num = SortOfFollow.minY - 24f; num < SortOfFollow.maxY + 16f; num += 24f)
		{
			if (Physics.OverlapSphere(new Vector3(this.x, num, 0f), 24f, Map.groundLayer).Length > 0)
			{
				list.Add(num);
			}
		}
		if (list.Count > 0)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		return SortOfFollow.minY - 64f + (SortOfFollow.maxY - SortOfFollow.minY + 128f) * UnityEngine.Random.value;
	}

	public float speed = 80f;

	protected float x;

	protected float explosionCounter;

	protected float explosionCounter2;

	protected float explosionCounter3;

	protected float explosionCounter4;

	protected float lastExplosionY;

	protected float delay = 1f;

	protected SpriteSM sprite;

	protected float spriteSin;
}
